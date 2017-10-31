using System.Collections;
using UnityEngine;

// ダウンロード済みのAssetBundle
public class DownloadedAssetBundle
{
    public AssetBundle assetBundle;
    public int referenceCount;

    public DownloadedAssetBundle(AssetBundle assetBundle)
    {
        this.assetBundle = assetBundle;
        referenceCount = 1;
    }
}
