#if UNITY_EDITOR 
// エクスポータを設定するときはエディタを使わないので
// ビルド時は通らないようにエクスポータ設定時は通るように
// UNITY_EDITORでこのソースを囲む
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;


public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")] // UnityEditorのメニュータブに項目追加
    static void BuildAllAssetBundles()// 項目をクリックした時に処理
    {
        // AssetBundleファイルを保存するフォルダ
        string assetBundleDirectory = "Assets/AssetBundles";
        // フォルダが存在するか確認
        // Windows用
        if (!Directory.Exists(assetBundleDirectory))
        {
            // フォルダが存在しない場合はフォルダ作成
            Directory.CreateDirectory(assetBundleDirectory); 
        }
        // Android用
        if (!Directory.Exists(assetBundleDirectory+"/Android"))
        {
            // フォルダが存在しない場合はフォルダ作成
            Directory.CreateDirectory(assetBundleDirectory + "/Android");
        }
        // iOS用
        if (!Directory.Exists(assetBundleDirectory+"/iOS"))
        {
            // フォルダが存在しない場合はフォルダ作成
            Directory.CreateDirectory(assetBundleDirectory + "/iOS");
        }
        // AssetBundleファイルのビルドをしてファイルを作成しフォルダに保存
        // Windows用
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        // Android用
        BuildPipeline.BuildAssetBundles(assetBundleDirectory + "/Android", BuildAssetBundleOptions.None, BuildTarget.Android);
        // iOS用
        BuildPipeline.BuildAssetBundles(assetBundleDirectory + "/iOS", BuildAssetBundleOptions.None, BuildTarget.iOS);
    }

    // 複数アセットをAssetBundleにまとめる
    [MenuItem ("Assets/Set AssetBundle Name")]
    private static void SelectedAsset()
    {
        // 選択中のオブジェクトを取得
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        // 特定のアセットバンドルに追加していく
        foreach(var obj in selection)
        {
            SetAssetBundleName(obj);
        }
    }

    private static void SetAssetBundleName(Object obj)
    {
        // アセットをAssetBundleにインポート
        AssetImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(obj));
        // インポート先の名前設定
        importer.assetBundleName = obj.name;
    }
}
#endif