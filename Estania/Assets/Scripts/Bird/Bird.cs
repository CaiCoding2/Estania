using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

    Vector2 camSize;
    private float birdSpeed = 1.5f;

    void Awake()
    {
        //get camera size
        camSize = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.aspect * Camera.main.orthographicSize);

    }
	// Update is called once per frame
	void Update () {

        transform.Translate(birdSpeed * Time.deltaTime, 0, 0);
		if(transform.position.x > camSize.x +  15)
        {
            Destroy(gameObject);
        }
	}
}
