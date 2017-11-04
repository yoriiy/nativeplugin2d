using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RankingPoint : MonoBehaviour {
    // ランキングjsonファイルパス
    private string RANKING_PATH = "Assets/Resources/Json/ranking.json";

    // Use this for initialization
    void Start () {
        // ランキングファイル読み込み
        string json = System.IO.File.ReadAllText(RANKING_PATH);
        RankingData loadedJsonData = null;

        Transform childranking = this.transform;    // ランキングノードを取得

        try
        {
            // ランキングjsonデータを配列データに変換
            loadedJsonData = JsonUtility.FromJson<RankingData>(json);
        }// データが正しくない場合はエラーを渡す
        catch (System.Exception i_exception)
        {
            Debug.LogError(i_exception);    // エラー表示
            loadedJsonData = null;  // データをnullクリア
        }

        // to do 仮として今のポイント降順にして入れている
        loadedJsonData.ranking[12].name = "田中さん";
        loadedJsonData.ranking[12].point = 12;

        loadedJsonData.ranking[14].name = "戸村さん";
        loadedJsonData.ranking[14].point = 12;

        // OrderByDescendingで降順でデータを並び替える
        // to do 今後ネットからデータを取得して並び替えてサーバーに保存する必要あり
        loadedJsonData.ranking = loadedJsonData.ranking.OrderByDescending(e => e.point).ToArray();

        int i = 0;
        // データが正しい場合ランキングのテキストに設定
        if (loadedJsonData != null)
        {
            // 表示するランキング分データ設定
            foreach (Transform child_base in childranking.transform)
            {
                Transform child_point = child_base.Find("Text");
                child_point.GetComponent<Text>().text = (i+1) + "　" + loadedJsonData.ranking[i].name + "　" + loadedJsonData.ranking[i].point.ToString() + "pt";
                i++;
            }
            
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
