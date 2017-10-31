using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetLoaderBase : MonoBehaviour {

    // ダウンロード予定のリスト
    // ファイルパスを設定
    public string[] downloadABList = { "" };

    // ロード済みゲームオブジェクトのリスト
    protected List<GameObject> _loadedObjectList;

    // AssetBundleロード
    protected virtual IEnumerator Load(string assetBundleName, string assetName)
    {
        yield return null;
    }

    // マニュフェストファイルのダウンロード
    protected IEnumerator DownloadManifest()
    {
        // ダウンロード開始
        AssetBundleLoadBase request = AssetBundleManager.DownloadManifest();

        if(request == null)
        {
            yield break;
        }
        // マニフェストファイルダウンロードの経過完了後コルーチン終了
        yield return StartCoroutine(this.CoroutineTimeOutCheck(request, 5f));
    }
}
