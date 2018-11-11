using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MistMovement : MonoBehaviour {
    public Image mist;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (mist.transform.position.x < -5)
        {
            mist.transform.Translate(new Vector3(0.01f, 0.01f, 0));
        }
 
    }
}
