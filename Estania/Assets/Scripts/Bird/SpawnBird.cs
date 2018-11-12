using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBird : MonoBehaviour {

    public Transform [] spawnpoint;
    public GameObject[] bird;
    float SpawnInterval = 11f;
 

    // Use this for initialization
    void Start () {
        InvokeRepeating("SpawnBirds", 1f, SpawnInterval);
	}
	
	// Update is called once per frame
	void Update () {
	}

    void SpawnBirds()
    {
        int spawnIndex = Random.Range(0, spawnpoint.Length);
        int birdIndex = Random.Range(0, bird.Length);
 
        Instantiate(bird[birdIndex], spawnpoint[spawnIndex].position, Quaternion.identity);
    }
}
