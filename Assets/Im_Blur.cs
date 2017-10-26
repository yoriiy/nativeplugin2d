using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Im_Blur : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var img = GetComponent<Image>();
        var tex = Texture_Blur.CreateBlurTexture(img.sprite.texture, 10.0f);
        img.sprite = Sprite.Create(tex, img.sprite.rect, new Vector2(0.5f, 0.5f), 100.0f);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
