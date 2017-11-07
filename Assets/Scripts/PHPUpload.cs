using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Debug.Log("PHPファイルを更新しました。");
        }

        yield return req;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
