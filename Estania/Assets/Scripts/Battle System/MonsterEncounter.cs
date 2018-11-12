using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEncounter : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "Player")
		{
			Debug.Log("Hi");
            //AudioManager.instance.PlaySound("Intro", transform.position, 1);
			GameManager.instance.gotAttacked = true;
			GameManager.instance.spawnedAttacked = true;
		}
	}
	// Use this for initialization
	void Start() {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
