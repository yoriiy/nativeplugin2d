using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParamMeter : MonoBehaviour {

    Vector3 position;

	// Use this for initialization
	void Start () {
        GameObject meter = GameObject.Find("frontmeter").gameObject;
        Image meterTex = meter.GetComponent<Image>();
        //position = meterTex.transform.localPosition;
        RectTransform meterRect = meter.GetComponent<RectTransform>();
        meterRect.anchorMax = new Vector2(0.5f,1.0f);
        //meterTex.transform.localPosition = new Vector3(-244.0f,0.0f,0.0f);
    }
	
	// Update is called once per frame
	void Update () {
        GameObject meter = GameObject.Find("frontmeter").gameObject;
        Image meterTex = meter.GetComponent<Image>();
        //position.x -= 1.01f;
        //meterTex.transform.localPosition = position;
    }
}
