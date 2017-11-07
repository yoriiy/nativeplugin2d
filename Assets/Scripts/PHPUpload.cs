using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PHPUpload : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {

        var url = "http://github.com/yoriiy/nativeplugin2d/raw/master/Assets/test.php";

        var wwwForm = new WWWForm();

        wwwForm.AddField("key1", "xxx");

        var req = new WWW(url, wwwForm);

        yield return req;

        if (req.error != null)
        {
            Debug.LogError(req.error);
        }
        else
        {
            GameObject Text = GameObject.Find("Text").gameObject;

            Text.GetComponent<Text>().text = "Load Success!";
            Debug.Log("PHPファイルを更新しました。");
        }

        yield return req;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
