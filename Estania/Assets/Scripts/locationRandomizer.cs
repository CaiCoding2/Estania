using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class locationRandomizer : MonoBehaviour {

   private float newXLocation;
    private float newYLocation;
	// Use this for initialization
	void Start () {
        newXLocation = gameObject.transform.position.x + Random.Range(-2f, 2f);
        newYLocation = gameObject.transform.position.y + Random.Range(-2f, 2f);
        gameObject.transform.position = new Vector3(newXLocation, newYLocation, gameObject.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
