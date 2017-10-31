using System.Collections;
using UnityEngine;

// AssetBundleからAssetのロード
public class AssetBundleLoadAsset : AssetBundleLoadBase
{
    // AssetBundleの名前
    protected string _assetBundlePath;

    // AssetBundleにあるAssetの名前
    protected string _assetName;
    protected string _downloadingError;
    protected System.Type _type;

    // AssetBundleのリクエスト
    protected AssetBundleRequest _request = null;

    // AssetBundleの情報を設定
    public AssetBundleLoadAsset (string bundlePath, string assetName, System.Type type)
    {
        _assetBundlePath = bundlePath;
        _assetName = assetName;
        _type = type;
    }

    // Assetの取得
    public override T GetAsset<T>()
    {
        if(_request != null && _request.isDone)
        {
            return _request.asset as T;
        }
        else
        {
            return null;
        }
    }


    // AssetBundleダウンロード監視
    public override bool LoadRequest()
    {
        // すでにリクエストがある場合は無視する
        if(_request != null)
        {
            return false;
        }

        // アセットバンドルのダウンロード監視
        DownloadedAssetBundle downloadedBundle = AssetBundleManager.GetDownloadedAssetBundle(_assetBundlePath, out _downloadingError);

        // ダウンロード成功時
        if(downloadedBundle != null)
        {
            // ダウンロードしたアセットバンドルから、名前とタイプでアセットを非同期読み込み
            _request = downloadedBundle.assetBundle.LoadAssetAsync(_assetName, _type);
            return false;
        }
        else
        {
            return true;
        }
    }

    // 完了チェック
    public override bool IsDone()
    {
        if(_request == null && _downloadingError != null)
        {
            Debug.LogError("_downloadeingError: " + _downloadingError);
            return true;
        }

        return _request != null && _request.isDone;
    }
}
