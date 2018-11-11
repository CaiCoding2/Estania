using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void onTriggerEnter(Collider other)
	{

		if (other.tag == "EnterArea")
		{
			CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
			GameManager.instance.nextPlayerPosition = other.gameObject.GetComponent<CollisionHandler>().spawnPoint.transform.position;
			GameManager.instance.sceneToLoad = col.sceneToLoad;
			GameManager.instance.loadNextScene();
		}

		if (other.tag == "LeaveArea")
		{
			CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
			GameManager.instance.nextPlayerPosition = other.gameObject.GetComponent<CollisionHandler>().spawnPoint.transform.position;
			GameManager.instance.sceneToLoad = col.sceneToLoad;
			GameManager.instance.loadNextScene();
		}
	}


}
	