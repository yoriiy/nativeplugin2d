using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopAnim : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GameObject kingyo = GameObject.Find("Image");
        Animator anim = kingyo.GetComponent<Animator>();
        // アニメーション識別
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Kingyo"))
        {
            // アニメーションの再生時間分待ったら
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                anim.Play("Kingyo", 0, 0.0f);   // 次のアニメの再生
            }
        }
	}
}
