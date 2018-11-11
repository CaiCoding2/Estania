using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exittoamainmenu : MonoBehaviour
{
	public string leveltoLoad;
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			SceneManager.LoadScene(0);
		}
	}
}
