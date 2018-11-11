using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour {

	public string sceneToLoad;//
	public GameObject spawnPoint; //spawn point we will land on when we change the scene
	public string spawnPointName;
	//GameState

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	/*void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			SceneManager.LoadScene("Town");
		}
	}
	*/
}
