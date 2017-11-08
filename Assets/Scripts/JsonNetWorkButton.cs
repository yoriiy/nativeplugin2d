using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonNetWorkButton : MonoBehaviour {

	// Use this for initialization
	public void OnClickButton () {
        StartCoroutine(JsonNetButton());
	}
	
	// Update is called once per frame
	private IEnumerator JsonNetButton() {
        // 送信
        string url = "http://hoguhogu.sakura.ne.jp/ranking.php";
        WWW www = new WWW(url);
        yield return www; //受信
        Debug.Log(www.text); //表示
    }
}
