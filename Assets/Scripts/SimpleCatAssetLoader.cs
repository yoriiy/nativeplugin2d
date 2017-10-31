using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCatAssetLoader : AssetLoaderBase {

    public Vector3[] catPosList;

    private void Awake()
    {
        downloadABList = new string[] { "sprites" };
    }
	// Use this for initialization
	IEnumerator Start () {
        // マニフェストファイルのロード
        yield return StartCoroutine(DownloadManifest());

        _loadedObjectList = new List<GameObject>();

        foreach(var path in downloadABList)
        {
            yield return StartCoroutine(Load(path, "Cat"));

            // AssetBundleのアンロード
            AssetBundleManager.UnloadAssetBundle(path);
        }

        // ロード済みなので使用可能
        int i = 0;
        foreach(var prefab in _loadedObjectList)
        {
            var go = Instantiate(prefab);
            go.transform.localPosition = catPosList[i];
            // shader再設定
            ShaderFind(go);
            i++;
        }

        Destroy(gameObject, 1f);
	}

    // AssetBundleのロード
    protected override IEnumerator Load(string assetBundleName, string assetName)
    {
        Debug.Log("load開始" + assetName + " 経過frame " + Time.frameCount);

        // AssetBundleからAssetをロード
        AssetBundleLoadBase request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(GameObject));
        // ロードできなかった場合
        if(request == null)
        {
            yield break;
        }

        // リクエスト処理を制限時間ありでイテレーション
        yield return StartCoroutine(this.CoroutineTimeOutCheck(request, 5f));

        // GameObject指定
        GameObject prefab = request.GetAsset<GameObject>();
        if(prefab != null)
        {
            _loadedObjectList.Add(prefab);
        }
    }

    // シェーダー再設定
    private void ShaderFind(GameObject go )
    {
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>(true);
        foreach(Renderer renderer in renderers)
        {
            Material mt = renderer.material;
            mt.shader = Shader.Find(mt.shader.name);
        }
    }
}
