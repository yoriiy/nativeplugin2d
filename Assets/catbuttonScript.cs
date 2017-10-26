using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catbuttonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void tap_catButton()
    {
        SceneFadeManager._fade_sequence = 2;
    }
}
