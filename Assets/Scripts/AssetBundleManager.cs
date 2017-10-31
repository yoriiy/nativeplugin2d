using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleManager : MonoBehaviour {
    // AssetBundleのマニフェストファイル
    private static AssetBundleManifest _assetBundleManifest = null;

    // ダウンロード済みリスト
    private static Dictionary<string, DownloadedAssetBundle> _downloadedAssetBundleDic = new Dictionary<string, DownloadedAssetBundle>();

    // ダウンロード実行中リスト
    // 中身があった場合ダウンロード処理開始
    private static Dictionary<string, WWW> _downloadingWWWDic = new Dictionary<string, WWW>();

    // ダウンロードエラーが発生した時のリスト
    private static Dictionary<string, string> _downloadingErrorDic = new Dictionary<string, string>();

    // 依存ファイルリスト
    private static Dictionary<string, string[]> _dependencies = new Dictionary<string, string[]>();

    // ロード経過チェック用リスト
    private static List<AssetBundleLoadBase> _inProgressList = new List<AssetBundleLoadBase>();

    // 自分自身のGameObject
    private static GameObject _myselfObject;

    // AssetBundleManifest
    public static AssetBundleManifest AssetBundleManifestObject
    {
        set { _assetBundleManifest = value; }
    }

    // AssetBundleManifestの初期化
    public static AssetBundleLoadManifest DownloadManifest()
    {
        // 自分自身のオブジェクト生成
        if (_myselfObject == null)
        {
            _myselfObject = new GameObject("AssetBundleManager", typeof(AssetBundleManager));
        }

        // マニフェストファイルのダウンロード
        string manifestAssetBundleName = AssetBundleUtility.GetPlatformFolder();
        _DownloadAssetBundle(manifestAssetBundleName, true);
        var loadManifest = new AssetBundleLoadManifest(manifestAssetBundleName, "AssetBundleManifest", typeof(AssetBundleManifest));
        _inProgressList.Add(loadManifest);
        return loadManifest;
    }

    // AssetBundleパスからAssetのロード
    public static AssetBundleLoadBase LoadAssetAsync(string assetBundlePath, string assetName, System.Type type)
    {
        AssetBundleLoadBase loadRequest = null;
        // ダウンロード開始予定リストに追加
        DownloadAssetBundle(assetBundlePath);
        // リクエスト取得
        loadRequest = new AssetBundleLoadAsset(assetBundlePath, assetName, type);
        // ロード経過チェック用リストにリクエスト追加
        _inProgressList.Add(loadRequest);

        // リクエスト処理を返す
        return loadRequest;
    }

    // ダウンロード済みAssetBundleの取得
    public static DownloadedAssetBundle GetDownloadedAssetBundle(string assetBundlePath, out string error)
    {
        // エラーリストに該当するAssetBundleがあった場合は終了
        if (_downloadingErrorDic.TryGetValue(assetBundlePath, out error))
        {
            Debug.LogError(assetBundlePath + " error:" + error);
            return null;
        }

        // ダウンロード済みリストに該当のAssetBundleがない場合終了
        DownloadedAssetBundle downloadedAB = null;
        if (_downloadedAssetBundleDic.TryGetValue(assetBundlePath, out downloadedAB) == false)
        {
            return null;
        }

        //=================================
        // ダウンロード済みリストを探す
        //=================================

        // assetBundleNameの依存ファイルチェック
        string[] dependencies = null;
        if (_dependencies.TryGetValue(assetBundlePath, out dependencies) == false)
        {
            // 依存ファイルがなければそのままloadedAssetBundleを返す
            return downloadedAB;
        }

        //=============================
        // 依存ファイルチェック
        //=============================

        // assetBundleNameの依存ファイルを摘出
        foreach (var dependency in dependencies)
        {
            // エラーリストに該当するAssetBundleの場合
            if (_downloadingErrorDic.TryGetValue(assetBundlePath, out error))
            {
                // downloadedAssetBundleを返し終了
                return downloadedAB;
            }

            // ロード済み依存ファイル　チェック
            DownloadedAssetBundle dependentBundle;
            _downloadedAssetBundleDic.TryGetValue(dependency, out dependentBundle);
            if (dependentBundle == null)
            {
                return null;
            }
        }

        return downloadedAB;
    }

    // AssetBundleのアンロード
    public static void UnloadAssetBundle(string assetBundleName)
    {
        // アセットバンドルのアンロード
        _UnloadAssetBundle(assetBundleName);
        // 依存ファイルも合った場合アンロード
        UnloadDependencies(assetBundleName);
    }

    // AssetBundleのダウンロード
    private static void DownloadAssetBundle(string assetBundleName)
    {
        // ダウンロード済みチェック
        bool isAlreadyProcessed = _DownloadAssetBundle(assetBundleName, false);

        // 初めてのダウンロードの場合は、依存ファイルをロード
        if (isAlreadyProcessed == false)
        {
            DownloadDependencies(assetBundleName);
        }
    }

    // ダウンロード処理
    // 真なら、ダウンロード済み、それ以外は初回ダウンロード
    private static bool _DownloadAssetBundle(string assetBundleName, bool isDownloadingABManifest)
    {
        /*
           テスト用キャッシュクリア
           if(Chaching.CleanChache())
           {
                Debug.Log("chache Clear");
           }
         */

        // 既にダウンロード済み
        DownloadedAssetBundle downloadedAB = null;
        _downloadedAssetBundleDic.TryGetValue(assetBundleName, out downloadedAB);
        if (downloadedAB != null)
        {
            // 既にダウンロード済みで、再度リクエストがあった場合、追加で参照アカウントを進める
            downloadedAB.referenceCount++;
            return true;
        }

        // ダウンロード実行中のリストをチェックする
        if (_downloadingWWWDic.ContainsKey(assetBundleName))
        {
            return true;
        }

        // ダウンロード実行中リストへ追加
        WWW downloadQue = null;
        // ここでAssetBundleのURLを設定する
        string url = AssetBundleUtility.GetDownloadPath() + assetBundleName;

        // マニフェストファイル or AssetBundle
        if (isDownloadingABManifest)
        {
            downloadQue = new WWW(url);
        }
        else
        {
            if (_assetBundleManifest == null)
            {
                downloadQue = WWW.LoadFromCacheOrDownload(url, 0);
            }
            else
            {
                // ダウンロードキューデータとして成功
                downloadQue = WWW.LoadFromCacheOrDownload(url, _assetBundleManifest.GetAssetBundleHash(assetBundleName), 0);
            }
        }

        // ダウンロード実行中リストに追加
        _downloadingWWWDic.Add(assetBundleName, downloadQue);

        // 初ダウンロード
        return false;
    }


    // 依存ファイルのロード
    private static void DownloadDependencies(string assetBundleName)
    {
        if (_assetBundleManifest == null)
        {
            Debug.LogError("AssetBundleManager.Initialize()でAssetBundleManifestを初期化してください。");
            return;
        }

        // マニフェストファイルから指定のAssetBundleの依存ファイルを取得
        string[] dependencies = _assetBundleManifest.GetAllDependencies(assetBundleName);
        if (dependencies.Length == 0)
        {
            return;
        }

        // Dictionaryに保存
        _dependencies.Add(assetBundleName, dependencies);
        foreach (var dependence in dependencies)
        {
            _DownloadAssetBundle(dependence, false);
        }
    }

    // 依存ファイルのアンロード
    private static void UnloadDependencies(string assetBundleName)
    {
        string[] dependencies = null;
        if (_dependencies.TryGetValue(assetBundleName, out dependencies) == false)
        {
            return;
        }
        foreach (var dependency in dependencies)
        {
            _UnloadAssetBundle(dependency);
        }

        _dependencies.Remove(assetBundleName);
        Debug.Log("Unload" + assetBundleName);
    }

    // AssetBundleのアンロード
    private static void _UnloadAssetBundle(string assetBundleName)
    {
        // ロード済みのAssetBundleチェック
        string error;
        DownloadedAssetBundle downloadedBundle = GetDownloadedAssetBundle(assetBundleName, out error);
        if (downloadedBundle == null)
        {
            return;
        }

        // AssetBundleの参照カウンタチェック
        if (--downloadedBundle.referenceCount == 0)
        {
            downloadedBundle.assetBundle.Unload(false);
            // ロード済みAssetBundleリストから該当分を削除する
            _downloadedAssetBundleDic.Remove(assetBundleName);
        }

    }
    void Update()
    {
        DownloadingCheck();
    }

    // ダウンロードリストへの追加
    private void DownloadingCheck()
    {
        var removeKeyList = new List<string>();

        // ダウンロード実行中リストからValue(Key)を取得
        foreach (var wwwValue in _downloadingWWWDic)
        {
            WWW downloadWWW = wwwValue.Value;

            // ダウンロードに失敗した場合は削除予定リストへ追加
            if (string.IsNullOrEmpty(downloadWWW.error) == false)
            {
                _downloadingErrorDic.Add(wwwValue.Key, downloadWWW.error);
                // 失敗したものをKeyで指定して削除予定リストへ追加
                removeKeyList.Add(wwwValue.Key);
                continue;
            }

            // ダウンロードに成功したら
            if (downloadWWW.isDone)
            {
                // ダウンロード済みリストへ保存
                _downloadedAssetBundleDic.Add(wwwValue.Key, new DownloadedAssetBundle(downloadWWW.assetBundle));
                // ダウンロード済みのものをKeyで指定して削除予定リストへ追加
                removeKeyList.Add(wwwValue.Key);
            }
        }

        // ダウンロード実行中リストから削除予定Keyで指定して削除
        foreach (var key in removeKeyList)
        {
            WWW download = _downloadingWWWDic[key];
            _downloadingWWWDic.Remove(key);
            download.Dispose();
        }

        // ロード経過リストをチェック
        for (int i = 0; i < _inProgressList.Count;)
        {
            // ロードリクエストが終了していたら
            if (_inProgressList[i].LoadRequest() == false)
            {
                _inProgressList.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
    }
}
