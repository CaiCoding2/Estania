using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyloads : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy(GameObject.Find("Player"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
