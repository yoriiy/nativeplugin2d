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
        string url = "http://github.com/yoriiy/nativeplugin2d/raw/master/Assets/aa.php"; //送信するphp
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("count", 10);//カウントする数 +10
        WWW www = new WWW(url, wwwForm);
        yield return www;//送信

        //受信
        string url2 = "http://github.com/yoriiy/nativeplugin2d/raw/master/Assets/ap.php";//受信する(表示している)php
        WWW www2 = new WWW(url2);
        yield return www2; //受信
        Debug.Log(www2.text); //表示

        GameObject count = GameObject.Find("Count").gameObject;
        count.GetComponent<Text>().text = www2.text;
    }
}
