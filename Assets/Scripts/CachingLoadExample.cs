using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CachingLoadExample : MonoBehaviour {

#if UNITY_EDITOR
    // AssetBundleファイル格納先
    //string bundleURL = "http://localhost:8000/asset/StreamingAssets/sprites";
    string bundleURL = "http://github.com/yoriiy/nativeplugin2d/raw/master/Assets/StreamingAssets/sprites";
    //string bundleURL = "file:///C:/Users/Yoriy/Desktop/nativeplugin2D/Assets/StreamingAssets/sprites";
#elif UNITY_ANDROID
    string bundleURL = "http://github.com/yoriiy/nativeplugin2d/raw/master/Assets/StreamingAssets/Android/sprites";
#endif
    string assetName = "Cat";   // AssetBundle内のアセットファイル
    int version = 0;

	// アセットバンドルのダウンロード タイムアウトまでの時間
    // readonly書き換え不能
	private readonly static float LIMIT_TIME = 5f;

    // Use this for initialization
    void Start () {
        StartCoroutine(DownloadAndCache());	
	}
	
    IEnumerator DownloadAndCache()
    {
        while(!Caching.ready)
        {
            yield return null;
        }

        // サーバーからキャッシュデータをロード
        using (WWW www = WWW.LoadFromCacheOrDownload(bundleURL, version))
        {
            yield return www;

            // ダウンロード完了待ち
            var startTime = Time.realtimeSinceStartup;
            while (www.isDone == false)
            {
                yield return 0;
                if( www.progress == 0f && Time.realtimeSinceStartup - startTime > LIMIT_TIME)
                {
                    // タイムアウト
                    break;
                }
            }

            // 接続できなくエラーが発生したら
            if (www.error != null)
            {
                // エラー表示
                throw new UnityException("WWWダウンロードでエラーが発生しました。" + www.error);
            }

            if(string.IsNullOrEmpty(www.error) && www.isDone)
            {
                // ダウンロードの成功
                Debug.Log("ロード成功！");
                // アセットバンドルからファイルを呼び出し
                var assetbundle = www.assetBundle;

                // この場合はCatのGameObject(prefabファイル)を呼び出し
                var resultObject = assetbundle.LoadAssetAsync<GameObject>(assetName);

                // AssetBundle内のprefabファイルが読み込み終わったら次の処理へ
                yield return new WaitWhile(() => resultObject.isDone == false);

                // 新規にGameObjectを登録
                GameObject.Instantiate(resultObject.asset);
                assetbundle.Unload(false);  // アセットバンドル終了

                yield break;
            }
            
            // ダウンロード失敗時
            if(www.isDone != false)
            {
                Debug.LogError("タイムアウト：ロード失敗しました。");
            }
            else if(string.IsNullOrEmpty(www.error) == false)
            {
                Debug.LogError(bundleURL + "\n" + www.error);
            }
            else
            {
                Debug.LogError("AssetBundleでエラーが発生しました。");
            }
        }
    }
}
