using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSet : MonoBehaviour {

    public static Boost _move;

	// Use this for initialization
	void Start () {
		if(Debug.isDebugBuild)
        {
            gameObject.SetActive(false);
        }
        if(!_move)
        {
            _move = new Boost();
        }  
        if(_move)
        {

        }  
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
