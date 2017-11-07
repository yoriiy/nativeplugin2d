using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotUpLoad : MonoBehaviour {
#if UNITY_EDITOR
    private string ScreenShotURL = "file://C:/Users/Yoriy/Desktop/screenshot/screenShot.png";
#else
    private string ScreenShotURL = "http://github.com/yoriiy/nativeplugin2d/raw/master/Assets/Resources/image/png/screenShot.png";
#endif
    // Use this for initialization
    IEnumerator Start () {
        yield return new WaitForEndOfFrame();

        var width = Screen.width;
        var height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        var bytes = tex.EncodeToPNG();
        Destroy(tex);

        var form = new WWWForm();
        form.AddField("frameCount", Time.frameCount.ToString());
        form.AddBinaryData("fileUpload", bytes, "screenShot.png", "image/png");

        //var w = new WWW("file:///C:/Users/Yoriy/Desktop/nativeplugin2D/Assets/image/png/screenShot.png", form);
        var w = new WWW(ScreenShotURL,form);

         yield return w;

        if (w.error != null)
        {
            Debug.LogError(w.error);
        }
        else
        {
            Debug.Log("ScreenShotをアップロードしました。");

            Texture2D texture = Resources.Load("image/png/screenShot") as Texture2D;
            this.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
