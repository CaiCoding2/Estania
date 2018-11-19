using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class swordscript : MonoBehaviour
{public int counter;
	public GameObject enemyToBeSpawned;
	public enemyencounterspawner EES;
    public Transform spawnpoint;
	GameObject spawnedEnemy;
	// Use this for initialization
	void Start()
	{
		EES = GameObject.Find("BeastSpawner").GetComponent<enemyencounterspawner>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	void OnTriggerStay2D(Collider2D other)
	
		{
		
		Debug.Log("Collid");

		//if (other.gameObject.name == "Player")
			if(Input.GetKeyDown(KeyCode.A))
		{
			Debug.Log("INSTANTIATING");
            AudioManager.instance.PlaySound("MonsterTransition", transform.position, 1);
			GameManager.instance.curRegions = 1;
				spawnedEnemy = Instantiate(enemyToBeSpawned, spawnpoint.position, Quaternion.identity) as GameObject;
				counter = 1;
				GameManager.instance.counter = counter;
			EES.counter = 1;
			}
		}
}
