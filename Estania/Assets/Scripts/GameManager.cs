using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public enemyencounterspawner EES;
	public CameraController CC;
	public bool battleover;
	public int counter;
	public bool inbattle;
	public bool boss;
	public bool lavosdead;
	public Scene m_Scene;
	public string sceneName;
	public bool instantiated;
	//CLASS RANDOM MONSTER
	[System.Serializable]
	public class RegionData

	{
		public string BattleScene;
		public string regionName;
		public int maxAmountEnemies = 4;
		public List<GameObject> possibleEnemies = new List<GameObject>();

		
	}

	public bool MariamHero;
	public int ap;
	public int curRegions;

	public List<RegionData> Regions = new List<RegionData>();

	public PlayerController PC;
	public static GameManager instance; //so game manager is permanent
	
	//Player
	// Use this for initialization
	public GameObject Player;
	//POSITIONS

	public Vector3 nextPlayerPosition; //spawn point position

	public Vector3 lastPlayerPosition;//BATTLE

	//SCENES

	public string sceneToLoad;
	public string lastScene;//BATTLE

	//BOOLS

	public bool isWalking = false;
	public bool canGetEncountered = false;
	public bool gotAttacked = false;
	public bool spawnedAttacked;

	//ENUM
	public enum GameStates
	{
		WORLD_STATE,
		TOWN_STATE,
		BATTLE_STATE,
		IDLE
	}

	//BATTLE

	public int enemyAmount;
	public List<GameObject> enemiesToBattle = new List<GameObject>();

	public GameStates gameState;

	void Awake() //awake called whenever a scene is called
	{

	
			
		if (spawnedAttacked) 
		Destroy(GameObject.Find("DialogueBeast"));
		Debug.Log("AWAKE!");
		m_Scene = SceneManager.GetActiveScene();
		sceneName = m_Scene.name;
		Debug.Log(sceneName);
		if (sceneName == "MenBattleScene")
		{ inbattle = true;
			Debug.Log("inbattle");
		}
		MariamHero = false;
		if (lastScene == "Field")
			MariamHero = true;
		//change when you have to remove the town to BattleSystem
		instantiated = false;
		ap = 3;
		//check if this instance is already existing how do we react to another game manager instance
		if (instance == null)
		{
			//if nt set the instane to this
			instance = this;
		}
		//if it exists but it is not this instance (it's another instance that we created and dragged into everything we need)
		else if (instance != this)
		{
			Destroy(gameObject);//destroy it
		}

		//set this to not be destroyable

		DontDestroyOnLoad(gameObject);
		if (!GameObject.Find("Player") && sceneName != "Finale")
	//	{
		//	GameObject Hero = Instantiate(Player, nextPlayerPosition, Quaternion.identity) as GameObject;
			//Hero.name = "Player";
	//	}
		//else
		{
			Debug.Log("Instantiating!");
			GameObject Hero = Instantiate(Player, Vector3.zero, Quaternion.identity) as GameObject;
			Hero.name = "Player";
			instantiated = true;
		}
	

	}


	public void loadNextScene()
	{
		CC = GameObject.Find("Main Camera").GetComponent<CameraController>();
		CC.inbattle = true;
		PC = GameObject.Find("Player").GetComponent<PlayerController>();
		PC.inbattle = true;
		if (sceneToLoad == "MenBattleScene")
			inbattle = true;
		SceneManager.LoadScene(sceneToLoad);
	}

	public void loadSceneAfterBattle()
	{
		battleover = true;
		if (lastScene == "RodrikPreContender")
			SceneManager.LoadScene("TownScene");
		else if (lavosdead == true)
			SceneManager.LoadScene("Ending Cutscene");
		else
		SceneManager.LoadScene(lastScene);
		
	}

	void RandomEncounter()
	{
		if (isWalking && canGetEncountered)
		{
			if (Random.Range(0, 1000) < 10)
			{
				Debug.Log("I got attacked");
				gotAttacked = true;
			}
		}
	}
	void Start() {
		m_Scene = SceneManager.GetActiveScene();
		sceneName = m_Scene.name;
		if (counter == 1)
			Destroy(GameObject.Find("swordinstone"));
		EES = GameObject.Find("BeastSpawner").GetComponent<enemyencounterspawner>();
		//if (counter == 1 && SceneManager.GetActiveScene().name != "FieldLavosSpawn" && SceneManager.GetActiveScene().name != "Field")
		if (counter == 1 && SceneManager.GetActiveScene().name != "FieldLavosSpawn" && SceneManager.GetActiveScene().name != "AfterSeerField")
		{
			EES.boss = true;
			EES.lavosdead = true;
			lavosdead = true;
		}
		if (boss)
			curRegions = 2;
		if (lastScene == "Field")
			MariamHero = true;
		if (spawnedAttacked)
			Destroy(GameObject.Find("DialogueBeast"));
	}

	// Update is called once per frame
	void Update () {
		m_Scene = SceneManager.GetActiveScene();
		sceneName = m_Scene.name;
		if (!GameObject.Find("Player") && !GameObject.Find("Hero 1") && sceneName != "Finale" && sceneName != "Sleeping Hart")
		//	{
		//	GameObject Hero = Instantiate(Player, nextPlayerPosition, Quaternion.identity) as GameObject;
		//Hero.name = "Player";
		//	}
		//else
		{
			Debug.Log("Instantiating!");
			GameObject Hero = Instantiate(Player, Vector3.zero, Quaternion.identity) as GameObject;
			Hero.name = "Player";
			instantiated = true;
		}
		if (counter == 1)
			Destroy(GameObject.Find("swordinstone"));
		//if (counter == 1 && SceneManager.GetActiveScene().name != "FieldLavosSpawn" && SceneManager.GetActiveScene().name != "Field")
		if (counter == 1 && SceneManager.GetActiveScene().name != "FieldLavosSpawn" && SceneManager.GetActiveScene().name != "AfterSeerField")
		{ EES.boss = true;
			EES.lavosdead = true;
			lavosdead = true;
		}
		if (boss)
			curRegions = 2; //was 1
		//if (lastScene == "Field")
		if (lastScene == "AfterSeerField")
			MariamHero = true;
		if (spawnedAttacked)
			Destroy(GameObject.Find("DialogueBeast"));
		switch (gameState)
		{
			case (GameStates.WORLD_STATE):
				if (isWalking)
				{
					RandomEncounter();
				}
				if (gotAttacked)
				{
					gameState = GameStates.BATTLE_STATE;
				}
				break;
			case (GameStates.TOWN_STATE):
				break;
			case (GameStates.BATTLE_STATE):
				//LOAD BATTLE SCENE
				StartBattle();
					gameState = GameStates.IDLE;
				//GO TO IDLE
				break;
			case (GameStates.IDLE):
				break;
		}

		
	}void StartBattle()
		{
		//AMOUNT OF ENEMIES
		enemyAmount = Random.Range(1, Regions[curRegions].maxAmountEnemies + 1);
		//which enemies are we going to send into battle
		Debug.Log(GameManager.instance.enemiesToBattle.Count);
		for (int i = 0; i < enemyAmount ; i++)
		{
			Debug.Log(i);
			enemiesToBattle.Add(Regions[curRegions].possibleEnemies[Random.Range(0, Regions[curRegions].possibleEnemies.Count)]);//take enemies from particular region and store into a list which has all enemies in a region
			Debug.Log(GameManager.instance.enemiesToBattle.Count);
		}
		//Player
		if (sceneName != "RodrikPreContender")
			lastPlayerPosition = GameObject.Find("Player").gameObject.transform.position;
		else lastPlayerPosition = Vector3.zero;
		nextPlayerPosition = lastPlayerPosition;
		lastScene = SceneManager.GetActiveScene().name;
		//LOAD LEVEL
		SceneManager.LoadScene(Regions[curRegions].BattleScene);
		//RESET HERO
		isWalking = false;
		gotAttacked = false;
		canGetEncountered = false;
	}
}
