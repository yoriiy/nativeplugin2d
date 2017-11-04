using UnityEngine;
using UnityEditor;

public class JsonEditorWindow : ScriptableWizard {

    [SerializeField]
    private RankingData m_rankData = null;  // ランキングデータ

    [SerializeField]
    private JsonScriptableObject m_jsonScriptableObject = null; // 一時保存jsonデータ

    // セーブファイルパス
    private static readonly string SAVE_ASSET_PATH = "Assets/Editor/JsonScriptableObject.asset";

    // ランキングエディタのウィンドウ表示
    [MenuItem("Tools/RankingEditor")]
    public static void Open()
    {
        // ウィンドウ作成(Save Loadボタンを追加)
        var window = DisplayWizard<JsonEditorWindow>("RankingDataEditor", "Save", "Load");

        //============ 一時保存データからデータを取得する ===========//
        var saveAsset = AssetDatabase.LoadAssetAtPath<JsonScriptableObject>(SAVE_ASSET_PATH);
        
        // ファイルがなければ作成する
        if( saveAsset == null)
        {
            // ファイルを作成する
            // 作成したときはウィンドウの設定データが空になるので注意
            saveAsset = CreateInstance<JsonScriptableObject>();
            AssetDatabase.CreateAsset(saveAsset, SAVE_ASSET_PATH);  // ファイル作成
            AssetDatabase.SaveAssets(); // ファイル保存
            AssetDatabase.Refresh();    // ファイル更新
        }
        //============================================================//

        // ウィンドウの方のデータに反映
        window.m_jsonScriptableObject = saveAsset;
        window.m_rankData = saveAsset.Json;
    }

    // Saveボタンの処理
    private void OnWizardCreate()
    {   
        // jsonデータをstinrg型に変換
        string json = JsonUtility.ToJson(m_rankData);
        json = JsonSmartPrint(json);    // 改行などで中身を適切に変更
        // 保存ポップアップウィンドウから保存先を選ばせて保存先を取得する
        string path = EditorUtility.SaveFilePanel("ランキングデータを保存", "", "ranking", "json");
        // 変換済みのデータを保存先に書き込む
        System.IO.File.WriteAllText(path, json);
        // アセット内を更新する
        AssetDatabase.Refresh();
    }

    // ウィンドウの状態が更新された時の処理
    private void OnWizardUpdate()
    {
        if(m_jsonScriptableObject != null)
        {
            m_jsonScriptableObject.Json = m_rankData;
            // Unityを閉じた時に一時データを保存する
            EditorUtility.SetDirty(m_jsonScriptableObject);
        }
    }

    // ロードボタン(OtherButton)の処理
    private void OnWizardOtherButton()
    {
        // ロードポップアップウインドウから読み込み先を選ばせて読み込み先を取得する
        string path = EditorUtility.OpenFilePanel("ランキングデータを開く","", "json");
        // ファイルを読み込みしてjsonデータで取得
        string json = System.IO.File.ReadAllText(path);

        RankingData loadedJsonData = null;

        try
        {
            // jsonUtilityでjson文字列をデータ配列に変換
            loadedJsonData = JsonUtility.FromJson<RankingData>( json );
        }// データが正しくない場合はエラーを渡す
        catch( System.Exception i_exception)
        {
            Debug.LogError(i_exception);    // エラー表示
            loadedJsonData = null;  // データをnullクリア
        }

        // データが正しい場合ウィンドウの値に設定
        if( loadedJsonData != null)
        {
            m_rankData = loadedJsonData;
            OnWizardUpdate();   // ウィンドウの更新
        }
    }

    // 改行とスペースでjsonデータを見やすく変換
    private string JsonSmartPrint( string i_json)
    {
        // jsonデータの中身が空の場合は何もない状態を返す
        if( string.IsNullOrEmpty(i_json))
        {
            return string.Empty;
        }

        // 文字列変換で改行文字と"\t"タブを""に変更
        i_json = i_json.Replace(System.Environment.NewLine, "").Replace("\t", "");

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        bool quote = false;
        bool ignore = false;
        int offset = 0;
        int indentLength = 3;

        // 文字位置文字ずつを処理
        foreach(char ch in i_json)
        {
            switch(ch)
            {
                case '"':   // 文字列識別のときはそのまま文字として処理
                    if(!ignore)
                    {
                        quote = !quote;
                    }
                    break;
                case '\'':  // 文字列識別(")の最中に(')が合ったら文字列識別の終わり
                    if(quote)
                    {
                        ignore = !ignore;
                    }
                    break;
            }
            // 文字列識別時はそのまま文字を代入
            if(quote)
            {
                sb.Append(ch);
            }
            else
            {
                switch(ch)
                {
                    // 処理区切りの{と[があった場合
                    // あとに改行を置く
                    case '{':
                    case '[':
                        sb.Append(ch);
                        sb.Append(System.Environment.NewLine);
                        // タブ分のスペースで位置調整
                        // offsetはタブの数+1
                        sb.Append(new string(' ', ++offset * indentLength));
                        break;
                    // 処理区切り終了の}と]があった場合
                    // 前に改行を置く
                    case '}':
                    case ']':
                        sb.Append(System.Environment.NewLine);
                        // タブ分のスペースで位置調整
                        // offsetはタブの数-1
                        sb.Append(new string(' ', --offset * indentLength));
                        sb.Append(ch);
                        break;
                    // ,の場合は後ろに改行追加
                    case ',':
                        sb.Append(ch);
                        sb.Append(System.Environment.NewLine);
                        // タブ分のスペースで位置調整
                        // offsetはタブの数
                        sb.Append(new string(' ', offset * indentLength));
                        break;
                    // :の場合は後ろにスペース追加
                    case ':':
                        sb.Append(ch);
                        sb.Append(' ');
                        break;
                    default:
                        // スペース以外はそのまま追加
                        if(ch != ' ')
                        {
                            sb.Append(ch);
                        }
                        break;
                }
            }
        }
        return sb.ToString().Trim();    // 無駄なスペースがある場合は削除して返す  
    }
}
