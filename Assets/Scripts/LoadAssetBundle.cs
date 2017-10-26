using UnityEngine;
using System.Collections;

public class LoadAssetBundle : MonoBehaviour
{
    string assetName = "Cat";   // AssetBundle内のアセットファイル

    string AssetPath    // アセットのパス
    {
        // アクセサ設定
        get
        {
#if UNITY_ANDROID   // ANDROID用
            return Application.streamingAssetsPath + "/sprites";    // ファイルパスを返す
#elif UNITY_IPHONE  // IPHONE用
            return Application.streamingAssetsPath + "/sprites";    // ファイルパスを返す
#else   // Windows用
            return Application.streamingAssetsPath + "/sprites";    // ファイルパスを返す
#endif
        }
    }

    IEnumerator Start()
    {
        // 非同期でAssetBundleを読み込み
        var resultAssetBundle = AssetBundle.LoadFromFileAsync(AssetPath);

        // AssetBundleファイルが読み込み終わったら次の処理へ
        yield return new WaitWhile(() => resultAssetBundle.isDone == false);

        // アセットバンドルからファイルを呼び出し
        // この場合はCatのGameObject(prefabファイル)を呼び出し
        var assetbundle = resultAssetBundle.assetBundle;
        var resultObject = assetbundle.LoadAssetAsync<GameObject>(assetName);

        // AssetBundle内のprefabファイルが読み込み終わったら次の処理へ
        yield return new WaitWhile(() => resultObject.isDone == false);

        // 新規にGameObjectを登録
        GameObject.Instantiate(resultObject.asset);
        assetbundle.Unload(false);  // アセットバンドル終了
    }
}