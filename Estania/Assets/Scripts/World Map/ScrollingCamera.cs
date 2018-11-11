using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ScrollingCamera : MonoBehaviour
{
	float Timer;
	int i = 0;
	Vector3 targetPos, lastPos;
	// Use this for initialization
	void Start()
	{
		transform.DOMove(new Vector3(5f, 5f, -10f), 15);
		

	}

	// Update is called once per frame
	void Update()
	{
		Timer += Time.deltaTime;
		if ((Timer > 8))
			Debug.Log("HIYA!");
		if (transform.position == new Vector3 (5f, 5f, -10f)) 
		transform.DOMove(new Vector3(5f, 20f, -10f), 15);
		if (transform.position == new Vector3(5f, 20f, -10f))
			transform.DOMove(new Vector3(0f, 40f, -10f), 15);
		if (transform.position == new Vector3(0f, 40f, -10f))
			transform.DOMove(new Vector3(-20f, 65f, -10f), 15);
	}
}
