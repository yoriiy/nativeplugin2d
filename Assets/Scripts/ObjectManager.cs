using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject pl in player)
        {
            Debug.Log(pl.name);
        }

        GameObject copy = Instantiate(player[0].gameObject) as GameObject;

        copy.name = "Dark_Hero";

        copy.transform.SetParent(gameObject.transform);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
