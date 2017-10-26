using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingyoBlue : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var spr = GetComponent<SpriteRenderer>();
        var tex = Texture_Blur.CreateBlurTexture(spr.sprite.texture, 2.0f);
        spr.sprite = Sprite.Create(tex, spr.sprite.rect, new Vector2(0.5f,0.5f), 100.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
