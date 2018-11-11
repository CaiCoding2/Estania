using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemyencounterspawner : MonoBehaviour {
	public Scene m_Scene;
	public bool boss;
	public bool lavosdead;
	public string sceneName;
	public GameObject enemyToBeSpawned;
	public static enemyencounterspawner beastspawner;
	public int counter;
		
	GameObject spawnedEnemy;
	// Use this for initialization
	void Awake()
	{ }

	void Start()
	{
		//counter = 0;
		counter = GameManager.instance.counter;
		if (counter == 1)
		{
			boss = true;
			lavosdead = false;
		}
		if (GameObject.Find("BeastSpawner") != null && counter == 1)
		{
			Destroy(this.gameObject);
		}
		else
		{
			DontDestroyOnLoad(transform.gameObject);

		}

		lavosdead = false;
		lavosdead = GameManager.instance.lavosdead;
		//boss = true;
		//DontDestroyOnLoad(gameObject);
		m_Scene = SceneManager.GetActiveScene();
		sceneName = m_Scene.name;
		if (sceneName == "Town" && !GameManager.instance.spawnedAttacked)
			spawnedEnemy = Instantiate(enemyToBeSpawned, Vector3.zero, Quaternion.identity) as GameObject;
		//counter = 1;
		/*if (sceneName == "Field" && !GameManager.instance.spawnedAttacked)
		{
			spawnedEnemy = Instantiate(enemyToBeSpawned, Vector3.zero, Quaternion.identity) as GameObject;
			counter = 1;
			GameManager.instance.counter = counter;
		}
		else if (sceneName == "Field" && !lavosdead)
		{
			boss = true;
			GameManager.instance.boss = true;

			spawnedEnemy = Instantiate(enemyToBeSpawned, Vector3.zero, Quaternion.identity) as GameObject;
			lavosdead = true;
		}*/
		//if (sceneName == "Field" && !lavosdead)
		Debug.Log("Counter == " + counter);
		if (sceneName == "AfterSeerField" && !lavosdead && counter == 1)
		{
			boss = true;
			GameManager.instance.boss = true;

			spawnedEnemy = Instantiate(enemyToBeSpawned, Vector3.zero, Quaternion.identity) as GameObject;
			lavosdead = true;

		}
	}	
	// Update is called once per frame
	void Update () {
		
	}
}
