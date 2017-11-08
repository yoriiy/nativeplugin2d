using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PHPButton : MonoBehaviour {

    public void OnClickButton()
    {
        StartCoroutine(CPHP());
    }
    private IEnumerator CPHP()
    {
        //送信
        string url = "http://hoguhogu.sakura.ne.jp/aa.php"; //送信するphp
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("count", 10);//カウントする数 +10
        WWW www = new WWW(url, wwwForm);
        yield return www;//送信

        //受信
        string url2 = "http://hoguhogu.sakura.ne.jp/ap.php";//受信する(表示している)php
        WWW www2 = new WWW(url2);
        yield return www2; //受信
        Debug.Log(www2.text); //表示

        GameObject count = GameObject.Find("Count").gameObject;
        count.GetComponent<Text>().text = www2.text;
        
        /*string url = "http://localhost:8000/unity_test.php";

        //WWWForm:WWWクラスを使用してwebサーバにポストするフォームデータを生成するヘルパークラス
        WWWForm wwwForm = new WWWForm();

        //AddFieldでfieldに値を格納                
        wwwForm.AddField("text", "テキストだよー");

        //WWWオブジェクトにURL,WWWFormをセットすることでPOST,GETを行える。
        WWW www = new WWW(url, wwwForm);

        //実行
        yield return www;

        Debug.Log(www.text); //表示*/
    }
}
