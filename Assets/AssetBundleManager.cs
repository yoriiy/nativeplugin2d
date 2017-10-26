using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleManager : MonoBehaviour {

    // AssetBundleのキャッシュ
    private AssetBundle assetBundleCache;

    // Asset Bundleをロードするコルーチン
    public IEnumerator LoadAssetBundleCoroutine()
    {
        // AssetBundleのURL
        var url = "";
#if UNITY_ANDROID
        url = "https://dl.dropboxusercontent.com/kingyo_asset";
#elif UNITY_IPHONE
        url = "https://dl.dropboxusercontent.com/kingyo_asset";
#else
        url = "https://dl.dropboxusercontent.com/kingyo_asset";
#endif

        // ダウンロード処理
        var www = WWW.LoadFromCacheOrDownload(url, 1);
        while(!www.isDone)
        {
            yield return null;
        }

        // エラー処理

        // AssetBundleをキャッシュする
        assetBundleCache = www.assetBundle;

        // リクエスト解放
        www.Dispose();
    }
    
    // AssetBundleからSpriteを取得
    public Sprite GetSpriteFromAssetBundle(string assetName)
    {
        try
        {
            // AssetBundleのキャッシュからスプライト読み込み
            return assetBundleCache.LoadAsset<Sprite>(string.Format("{0}.png", assetName));
        }
        catch(NullReferenceException e)
        {
            Debug.Log(e.ToString());
            return null;
        }
    }
}
