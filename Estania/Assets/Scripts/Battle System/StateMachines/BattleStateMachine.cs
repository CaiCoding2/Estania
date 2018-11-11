
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BattleStateMachine : MonoBehaviour
{
	public HeroStateMachine HSM2;
	public enum PerformAction
	{
		WAIT,
		TAKEACTION,
		PERFORMACTION,
		CHECKALIVE,
		WIN,
		LOSE
	}

	public PerformAction battleStates;

	public List<HandleTurns> PerformList = new List<HandleTurns>();
	public List<GameObject> HeroesInBattle = new List<GameObject>();
	public List<GameObject> EnemiesInBattle = new List<GameObject>();

	public enum HeroGUI
	{
		ACTIVATE, //new instance of handleturns
		WAITING, //waiting turn
		INPUT1, //basic attack
		INPUT2, //select opponent
		DONE,
	}

	public EnemyStateMachine ESM;

	public bool physicalAtk;
	public bool magicalAtk;
	public bool magical;
	public bool deathWaitOver;
	public bool attackButtonsCleared;
	public bool attackButtonsCreated;
	public bool magicSelect;
	public bool deathWaitOverDefeat;

	//ints

	public int actionBtnsIndex;
	public int previousActionBtnsIndex;
	public int EnemyBtnsIndex;
	public int previousEnemyBtnsIndex;
	public int magicBtnsIndex;
	public int previousMagicBtnsIndex;
	public int enemySelectCounter;
	public int magicSelectCounter;

	public HeroGUI Heroinput;

	public List<GameObject> HeroesToManage = new List<GameObject>();
	public HandleTurns HeroChoice;

	public GameObject enemyButton;
	public Transform Spacer;

	public GameObject dmgText;
	public GameObject AttackPanel;
	public GameObject EnemySelectPanel;
	public GameObject MagicPanel;
	public GameObject ComboPanel;
	public GameObject infoPanel;
	public Transform BattleCanvas;

	//attacks of heroes
	public Transform actionSpacer;
	public Transform magicSpacer;
	public GameObject actionButton;
	public GameObject magicButton;
	

	//text

	public GameObject infoText;
	public Text InfoText;

	private List<GameObject> atkBtns = new List<GameObject>();
	private List<GameObject> actionBtns = new List<GameObject>();
	private List<GameObject> magicBtns = new List<GameObject>();


	//enemy buttons
	private List<GameObject> enemyBtns = new List<GameObject>();
	// Use this for initialization

	//SPAWN POINTS
	public List<Transform> spawnPoints = new List<Transform>();

	void Awake()
	{
		for (int i = 0; i < GameManager.instance.enemyAmount; i++)
		{
			Debug.Log(GameManager.instance.enemyAmount);
			Debug.Log(GameManager.instance.enemiesToBattle.Count);
			GameObject NewEnemy = Instantiate(GameManager.instance.enemiesToBattle[i], spawnPoints[i].position, Quaternion.identity) as GameObject;
			NewEnemy.name = NewEnemy.GetComponent<EnemyStateMachine>().enemy.theName + "_" + (i + 1);
			NewEnemy.GetComponent<EnemyStateMachine>().enemy.theName = NewEnemy.name;
			EnemiesInBattle.Add(NewEnemy);
		}
	}
	void Start()
	{
		HSM2 = GameObject.Find("Hero 1").GetComponent<HeroStateMachine>();
		//ComboPanel.SetActive(false);
		infoPanel.SetActive(false);
		InfoText = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().infoText.gameObject.GetComponent<Text>();
		magicSelect = false;
		attackButtonsCleared = false;
		attackButtonsCreated = false;
		actionBtnsIndex = 0;
		EnemyBtnsIndex = 0;
		magicBtnsIndex = 0;
		previousMagicBtnsIndex = 0;
		magicSelectCounter = 0;

		deathWaitOver = false;
		battleStates = PerformAction.WAIT;
		//EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
		HeroesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
		Heroinput = HeroGUI.ACTIVATE;
		//EnemyButtons();

		AttackPanel.SetActive(false);//make attack panel not show
		EnemySelectPanel.SetActive(false);//make enemy select panel not show unless told to
		MagicPanel.SetActive(false);

	}

	// Update is called once per frame
	void Update()
	{
		
	
		if (battleStates == PerformAction.WIN)
			Debug.Log("WEVE WON");
		if (Input.GetKeyDown(KeyCode.B))
			Debug.Log("B is pressed");
		if ((deathWaitOverDefeat == true) && Input.GetKeyDown(KeyCode.A))
		{
            SceneManager.LoadScene("ExitScreen");
            //GameManager.instance.loadSceneAfterBattle();
            GameManager.instance.gameState = GameManager.GameStates.WORLD_STATE;
			GameManager.instance.enemiesToBattle.Clear();
		}
		if ((deathWaitOver == true) && Input.GetKeyDown(KeyCode.A))
			{
            SceneManager.LoadScene("ExitScreen");
            //GameManager.instance.loadSceneAfterBattle();
            GameManager.instance.gameState = GameManager.GameStates.WORLD_STATE;
			GameManager.instance.enemiesToBattle.Clear();}


			//StartCoroutine(waitForVictory());
			if (AttackPanel.gameObject.activeSelf == false)
		{
			actionBtnsIndex = 0;
			previousActionBtnsIndex = 0;
		}
		if (EnemySelectPanel.gameObject.activeSelf == false)
		{
			EnemyBtnsIndex = 0;
			previousEnemyBtnsIndex = 0;
		}
		StartCoroutine(selectingButtons());
		/*if (AttackPanel.gameObject.activeSelf == true)
		{
			selectActionButtons();
			highlightActionButton();
		}
		
		if (EnemySelectPanel.gameObject.activeSelf == true && AttackPanel.gameObject.activeSelf == false)
		{
			selectEnemyButtons();
			highlightEnemyButton();
		}*/

		//if (GameObject.Find("BattleCanvas").transform.Find("ActionPanel").gameObject.activeSelf == true)
		//Debug.Log("attack panel is active");


		switch (battleStates)
		{
			case (PerformAction.WAIT):
				if (PerformList.Count > 0)
				{
					battleStates = PerformAction.TAKEACTION;
				}
				break;
			case (PerformAction.TAKEACTION):
				GameObject performer = GameObject.Find(PerformList[0].Attacker);//catch information
																				//know if enemy or hero character
				if (PerformList[0].Type == "Enemy")
				{
					EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();//catch enemy statemachine
					for (int i = 0; i < HeroesInBattle.Count; i++)//check if currently dead hero is in battle list
					{
						if (PerformList[0].AttackersTarget == HeroesInBattle[i])
						{
							ESM.HeroToAttack = PerformList[0].AttackersTarget;
							ESM.currentState = EnemyStateMachine.TurnState.ACTION;
							break;
						}
						else
						{
							PerformList[0].AttackersTarget = HeroesInBattle[Random.Range(0, HeroesInBattle.Count)];//if hero is dead choose another
							ESM.HeroToAttack = PerformList[0].AttackersTarget;
							ESM.currentState = EnemyStateMachine.TurnState.ACTION;
						}
						//attackers target is not in heroesinbattle list
					}


					ESM.HeroToAttack = PerformList[0].AttackersTarget;
					ESM.currentState = EnemyStateMachine.TurnState.ACTION;
				}
				if (PerformList[0].Type == "Hero")
				{
					HeroStateMachine HSM = performer.GetComponent<HeroStateMachine>();
					HSM.EnemyToAttack = PerformList[0].AttackersTarget;
					HSM.currentState = HeroStateMachine.TurnState.ACTION; //hero performs an action at this point
																		  //Debug.Log("Hero is here to perform");
				}
				battleStates = PerformAction.PERFORMACTION;
				break;
			case (PerformAction.PERFORMACTION):
				//idle state
				break;
			case (PerformAction.CHECKALIVE):
				if (HeroesInBattle.Count < 1)
				{
					battleStates = PerformAction.LOSE;
				}//lose battle
				else if (EnemiesInBattle.Count < 1)
				{
					battleStates = PerformAction.WIN;//win battle
				}
				else
				{
					clearAttackPanel();
					actionBtnsIndex = 0;
					previousActionBtnsIndex = 0;
					magicBtnsIndex = 0;
					previousMagicBtnsIndex = 0;
					attackButtonsCleared = true;
					Heroinput = HeroGUI.ACTIVATE;//call function 
				}
				break;

			case (PerformAction.LOSE):
				{
					StartCoroutine(waitForDeathDefeat());

					Debug.Log("You lost the game");
				}
				break;

			case (PerformAction.WIN):
				{
					Debug.Log("You Win the Battle");
					for (int i = 0; i < HeroesInBattle.Count; i++)
					{
						HeroesInBattle[i].GetComponent<HeroStateMachine>().currentState = HeroStateMachine.TurnState.WAITING;
					}
					if (deathWaitOver != true);
					StartCoroutine(waitForDeath());

					if (deathWaitOver == true)
					{
						Debug.Log("WAIT FOR DEATH IS OVER");
						infoPanel.SetActive(true);
						InfoText.text = "Victory!";
						//StartCoroutine(waitForVictory());
					//	if (Input.GetKeyDown(KeyCode.B))
						//	{ GameManager.instance.loadSceneAfterBattle();
							//GameManager.instance.gameState = GameManager.GameStates.WORLD_STATE;
							//GameManager.instance.enemiesToBattle.Clear();
						//}
					}
				}
				break;
		}
		switch (Heroinput)
		{
			case (HeroGUI.ACTIVATE): //hero has a turn
				if (HeroesToManage.Count > 0)
				{
					HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(true);
					HeroChoice = new HandleTurns();
					AttackPanel.SetActive(true); //hero can see the attack menu
					Heroinput = HeroGUI.WAITING;
					//populate attack buttons
					CreateAttackButtons();
					attackButtonsCreated = true;
				}
				break;

			case (HeroGUI.WAITING):
				//idle
				break;

			case (HeroGUI.DONE):
				HeroInputDone();
				break;
		}

	}

	public void CollectActions(HandleTurns input)//collecting actions and giving access to the outside
	{
		PerformList.Add(input);
	}

	public void EnemyButtons()
	{
		//cleanup we add newbuton to the list
		foreach (GameObject enemyBtn in enemyBtns)
		{
			Destroy(enemyBtn);
		}
		enemyBtns.Clear();
		EnemyBtnsIndex = 0;
		previousEnemyBtnsIndex = 0;
		//create buttons
		foreach (GameObject enemy in EnemiesInBattle)
		{
			GameObject newButton = Instantiate(enemyButton) as GameObject;
			EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();//connection from button above to pass in info
			EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine>();
			Text buttonText = newButton.transform.Find("Text").gameObject.GetComponent<Text>();// getting button associated with ESM
			buttonText.text = cur_enemy.enemy.theName;

			button.EnemyPrefab = enemy;
			newButton.transform.SetParent(Spacer, false); //deals with overflow
			enemyBtns.Add(newButton);
		}
	}

	public void Input1()//attack button
	{
		physicalAtk = true;
		HeroChoice.Attacker = HeroesToManage[0].name;
		HeroChoice.AttackersGameObject = HeroesToManage[0];//saving the hero game object of heroes to manage in attackersgameobject.
		HeroChoice.Type = "Hero";
		//HeroChoice.chooseanAttack = HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attacks[0];//first attack is base attack
		AttackPanel.SetActive(false);
		EnemyButtons();
		EnemySelectPanel.SetActive(true); //we associated this with attack
		HeroChoice.chooseanAttack = HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attacks[0];//first attack is base attack
	}

	public void Input5()//attack button
	{
		HeroChoice.Attacker = HeroesToManage[0].name;
		HeroChoice.AttackersGameObject = HeroesToManage[0];//saving the hero game object of heroes to manage in attackersgameobject.
		HeroChoice.Type = "Hero";
		HeroChoice.chooseanAttack = HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attacks[0];//first attack is base attack
		AttackPanel.SetActive(false);
		EnemySelectPanel.SetActive(false); //we associated this with attack
	}

	public void Input2(GameObject chosenEnemy) //enemy selecton
	{
		magicSelect = false;
		HeroChoice.AttackersTarget = chosenEnemy;
		if (physicalAtk)
		{
			ComboPanel.SetActive(true);
			physicalAtk = false;
			Heroinput = HeroGUI.DONE;
		}
		else if (magical)
		{
			/*
				magicalAtk = true;
				magical = false;
				HSM2.hero.curMP -= HeroesToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks[magicBtnsIndex].attackCost;
				HSM2.stats.HeroMP.text = "MP: " + HSM2.hero.curMP;
				Heroinput = HeroGUI.DONE;
				*/
			if ((HSM2.hero.curMP - HSM2.hero.MagicAttacks[magicBtnsIndex].attackCost) >= 0)
			{
				magicalAtk = true;
				magical = false;
				HSM2.hero.curMP -= HeroesToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks[magicBtnsIndex].attackCost;
				HSM2.stats.HeroMP.text = "MP: " + HSM2.hero.curMP;
				Heroinput = HeroGUI.DONE;
			}

		}
	}

	public void Input6()
	{
		if (Input.GetKeyDown(KeyCode.W) == true)
		{ }
	}
	void HeroInputDone()
	{
		PerformList.Add(HeroChoice);
		clearAttackPanel();


		//clean the attackpanel
		//foreach (GameObject atkBtn in atkBtns)
		//{
		//Destroy(atkBtn);
		//}
		//atkBtns.Clear();


		HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(false);
		HeroesToManage.RemoveAt(0);
		Heroinput = HeroGUI.ACTIVATE;
	}

	void clearAttackPanel()
	{
		foreach (GameObject atkBtn in atkBtns)
		{
			Destroy(atkBtn);
		}
		foreach (GameObject actionBtn in actionBtns)
		{
			Destroy(actionBtn);
		}
		foreach (GameObject magicBtn in magicBtns)
		{
			Destroy(magicBtn);
		}
		actionBtns.Clear();
		atkBtns.Clear();
		magicBtns.Clear();
		EnemySelectPanel.SetActive(false);
		AttackPanel.SetActive(false);
		MagicPanel.SetActive(false);
	}

	//void clearMagicPanel()
	//	{ magicBtns.Clear(); }

	//create actionbuttons

	void CreateAttackButtons()
	{
		GameObject AttackButton = Instantiate(actionButton) as GameObject;//change text inside of button bottom
		Text AttackButtonText = AttackButton.transform.Find("Text").gameObject.GetComponent<Text>();
		AttackButtonText.text = "Attack";
		AttackButton.GetComponent<Button>().onClick.AddListener(() => Input1());//paste any function into onclick event. When we click that button it will perform input 1
		AttackButton.transform.SetParent(actionSpacer, false);
		atkBtns.Add(AttackButton);
		actionBtns.Add(AttackButton);
		Debug.Log("Action button number before error" + actionBtns.Count);
		//actionBtns[actionBtnsIndex].GetComponent<Image>().color = Color.yellow;

		GameObject MagicAttackButton = Instantiate(actionButton) as GameObject;//change text inside of button bottom
		Text MagicAttackButtonText = MagicAttackButton.transform.Find("Text").gameObject.GetComponent<Text>();
		MagicAttackButtonText.text = "Magic";
		MagicAttackButton.GetComponent<Button>().onClick.AddListener(() => Input3());
		//

		MagicAttackButton.transform.SetParent(actionSpacer, false);
		atkBtns.Add(MagicAttackButton);
		actionBtns.Add(MagicAttackButton);

		if (HeroesToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks.Count > 0)
		{
			foreach (BaseAttack magicAtk in HeroesToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks) //for each spell create a button
			{
				GameObject MagicButton = Instantiate(magicButton);
				Text MagicButtonText = MagicButton.transform.Find("Text").gameObject.GetComponent<Text>();
				MagicButtonText.text = magicAtk.attackName;
				AttackButton ATB = MagicButton.GetComponent<AttackButton>();
				ATB.magicAttackToPerform = magicAtk;
				MagicButton.transform.SetParent(magicSpacer, false); //place buttons into right spacer (this should be how you add items to an inventory)
				atkBtns.Add(MagicButton);
				magicBtns.Add(MagicButton);
				//make inventory by adding scripts to a hero and then populating the menu with those available in inventory
			}
		}
		else
		{
			MagicAttackButton.GetComponent<Button>().interactable = false;//false so we dont perform a magic atack
		}
	}

	void selectActionButtons() // allows player to select using keyboard / ps4 controller one of the four commands: attack, magic, charge, and item.
	{
		Debug.Log("Action Button Index = " + actionBtnsIndex);
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			previousActionBtnsIndex = actionBtnsIndex;
			actionBtnsIndex += 1;

			if (actionBtnsIndex > (actionBtns.Count - 1))
				actionBtnsIndex = 0;
			Debug.Log(actionBtnsIndex);
			Debug.Log("Action buttons amount = " + actionBtns.Count);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			previousActionBtnsIndex = actionBtnsIndex;
			actionBtnsIndex -= 1;
			if (actionBtnsIndex < 0)
				actionBtnsIndex = actionBtns.Count - 1;
			Debug.Log(actionBtnsIndex);
			Debug.Log("Action buttons amount = " + actionBtns.Count);
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			actionBtns[actionBtnsIndex].GetComponent<Button>().onClick.Invoke();
			//actionBtnsIndex = 0;
			//previousActionBtnsIndex = 0;
		}
	}

	void selectEnemyButtons() // allows player to select using keyboard / ps4 controller one of the four commands: attack, magic, charge, and item.
	{
		Debug.Log("Enemy Button Index = " + EnemyBtnsIndex);
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			previousEnemyBtnsIndex = EnemyBtnsIndex;
			EnemyBtnsIndex += 1;

			if (EnemyBtnsIndex > (enemyBtns.Count - 1))
				EnemyBtnsIndex = 0;
			Debug.Log(EnemyBtnsIndex);
			Debug.Log("Action buttons amount = " + enemyBtns.Count);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			previousEnemyBtnsIndex = EnemyBtnsIndex;
			EnemyBtnsIndex -= 1;
			if (EnemyBtnsIndex < 0)
				EnemyBtnsIndex = enemyBtns.Count - 1;
			Debug.Log(EnemyBtnsIndex);
			Debug.Log("Enemy buttons amount = " + enemyBtns.Count);
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			Debug.Log("A has been clicked!");
			enemyBtns[EnemyBtnsIndex].GetComponent<Button>().onClick.Invoke();
			//EnemyBtnsIndex = 0;
			//previousEnemyBtnsIndex = 0;
		}
	}

	void highlightActionButton()
	{
		actionBtns[previousActionBtnsIndex].GetComponent<Image>().color = Color.black;
		actionBtns[actionBtnsIndex].GetComponent<Image>().color = Color.yellow;
	}

	void highlightEnemyButton()
	{
		Debug.Log("Previous enemy button index" + previousEnemyBtnsIndex);
		Debug.Log("EnemyBtns Index" + EnemyBtnsIndex);
		enemyBtns[previousEnemyBtnsIndex].GetComponent<Image>().color = Color.black;
		enemyBtns[EnemyBtnsIndex].GetComponent<Image>().color = Color.yellow;

	}

	void selectMagicButtons() // allows player to select using keyboard / ps4 controller one of the four commands: attack, magic, charge, and item.
	{
		Debug.Log("Magic Button Index = " + magicBtnsIndex);
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			previousMagicBtnsIndex = magicBtnsIndex;
			magicBtnsIndex += 1;

			if (magicBtnsIndex > (magicBtns.Count - 1))
				magicBtnsIndex = 0;
			Debug.Log("MagicBtnsIndex " + magicBtnsIndex);
			Debug.Log("Magic buttons amount = " + magicBtns.Count);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			previousMagicBtnsIndex = magicBtnsIndex;
			magicBtnsIndex -= 1;
			if (magicBtnsIndex < 0)
				magicBtnsIndex = magicBtns.Count - 1;
			Debug.Log(magicBtnsIndex);
			Debug.Log("Magic buttons amount = " + magicBtns.Count);
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			Debug.Log("A has been clicked!");
			magicBtns[magicBtnsIndex].GetComponent<Button>().onClick.Invoke();
			infoPanel.SetActive(false);
			//EnemyBtnsIndex = 0;
			//previousEnemyBtnsIndex = 0;
		}
	}

	void highlightMagicButton()
	{
		magicBtns[previousMagicBtnsIndex].GetComponent<Image>().color = Color.black;
		magicBtns[magicBtnsIndex].GetComponent<Image>().color = Color.yellow;
		infoPanel.SetActive(true);
		InfoText.text = GameObject.Find("Hero 1").GetComponent<HeroStateMachine>().hero.MagicAttacks[magicBtnsIndex].attackDescription;
	}

	private IEnumerator selectingButtons()
	{

		if ((AttackPanel.gameObject.activeSelf == false) && (Input.GetKeyDown(KeyCode.S) && ((MagicPanel.gameObject.activeSelf == true) || ((EnemySelectPanel.gameObject.activeSelf == true) && (magicSelect == false)))))
			backToAttackPanel();
		if ((AttackPanel.gameObject.activeSelf == false) && (Input.GetKeyDown(KeyCode.S) && ((MagicPanel.gameObject.activeSelf == true) || ((EnemySelectPanel.gameObject.activeSelf == true) && (magicSelect == true)))))
			backToAttackPanelMagic();

		if (MagicPanel.gameObject.activeSelf == true)
			Debug.Log("MagicPanel");

		if (EnemySelectPanel.gameObject.activeSelf == false)
			enemySelectCounter = 0;
		if (MagicPanel.gameObject.activeSelf == false)
			magicSelectCounter = 0;
		if (AttackPanel.gameObject.activeSelf == true)
		{
			selectActionButtons();
			highlightActionButton();
		}

		if (MagicPanel.gameObject.activeSelf == true && AttackPanel.gameObject.activeSelf == false)
			highlightMagicButton();


		if (MagicPanel.gameObject.activeSelf == true && AttackPanel.gameObject.activeSelf == false && magicSelectCounter == 0)
		{
			magicSelectCounter += 1; //waits an update cycle before it will register an A click 
			yield break;

		}

		if (MagicPanel.gameObject.activeSelf == true && AttackPanel.gameObject.activeSelf == false && magicSelectCounter != 0)
			selectMagicButtons();

		//needs to click A to reach magic menu, but don't want to click A twice, so needs to have a cycle delay with a magic select counter

		//Debug.Log("REACHED RIGHT BEFORE ENEMY SELECT PANEL");

		if (EnemySelectPanel.gameObject.activeSelf == true && AttackPanel.gameObject.activeSelf == false && MagicPanel.gameObject.activeSelf == false)
			highlightEnemyButton();


		if (EnemySelectPanel.gameObject.activeSelf == true && AttackPanel.gameObject.activeSelf == false && MagicPanel.gameObject.activeSelf == false && enemySelectCounter == 0)
		{
			enemySelectCounter += 1; //waits an update cycle before it will register an A click 
			yield break;

		}

		if (EnemySelectPanel.gameObject.activeSelf == true && AttackPanel.gameObject.activeSelf == false && enemySelectCounter != 0 && MagicPanel.gameObject.activeSelf == false)
			selectEnemyButtons();
	}

	void clearEnemyBtns()
	{
		foreach (GameObject enemyBtn in enemyBtns)
		{
			Destroy(enemyBtn);
		}
		enemyBtns.Clear();
	}

	void backToAttackPanel()
	{
		clearAttackPanel();
		clearEnemyBtns();
		actionBtnsIndex = 0;
		EnemyBtnsIndex = 0;
		magicBtnsIndex = 0;
		previousActionBtnsIndex = 0;
		previousEnemyBtnsIndex = 0;
		previousMagicBtnsIndex = 0;
		magicSelect = false;
		AttackPanel.SetActive(true);
		CreateAttackButtons();
		physicalAtk = false;
		infoPanel.SetActive(false);
	}
	void backToAttackPanelMagic()
	{
		clearAttackPanel();
		clearEnemyBtns();
		actionBtnsIndex = 0;
		EnemyBtnsIndex = 0;
		magicBtnsIndex = 0;
		previousActionBtnsIndex = 0;
		previousEnemyBtnsIndex = 0;
		previousMagicBtnsIndex = 0;
		magicSelect = true;
		CreateAttackButtons();
		MagicPanel.SetActive(true);
		magical = false;
	

	}


	public void Input4(BaseAttack chooseaMagic)//choose a magic attack
	{
		magical = true;
		HeroChoice.Attacker = HeroesToManage[0].name;
		HeroChoice.AttackersGameObject = HeroesToManage[0];//saving the hero game object of heroes to manage in attackersgameobject.
		HeroChoice.Type = "Hero";

		HeroChoice.chooseanAttack = chooseaMagic;
		MagicPanel.SetActive(false);
		EnemySelectPanel.SetActive(true);
		EnemyButtons();
	}

	public void Input3()//switching to magic attacks
	{
		AttackPanel.SetActive(false);
		MagicPanel.SetActive(true);
		magicSelect = true;
	}

	public IEnumerator waitForDeathDefeat()
	{ yield return new WaitForSeconds(1.40f);
		deathWaitOverDefeat = true;
		if (deathWaitOverDefeat == true)
		{
			infoPanel.SetActive(true);
			InfoText.text = "Defeat!"; }
		//1. .65 about exact time of explosion 2. Need to have victory screen implemented to see how it looks exactly 
	}
		public IEnumerator waitForDeath()
	{
		yield return new WaitForSeconds(1.40f);//1. .65 about exact time of explosion 2. Need to have victory screen implemented to see how it looks exactly
		//GameManager.instance.loadSceneAfterBattle();
		//GameManager.instance.gameState = GameManager.GameStates.WORLD_STATE;
		//GameManager.instance.enemiesToBattle.Clear();
		deathWaitOver = true;
		if (deathWaitOver == true)
		{
			Debug.Log("WAIT FOR DEATH IS OVER");
			infoPanel.SetActive(true);
			InfoText.text = "Victory!";
			//while (!Input.GetKeyDown(KeyCode.A))
				//yield break;
		//	GameManager.instance.loadSceneAfterBattle();
			//GameManager.instance.gameState = GameManager.GameStates.WORLD_STATE;
			//GameManager.instance.enemiesToBattle.Clear();
			//StartCoroutine(waitForVictory());
			//GameManager.instance.loadSceneAfterBattle();
			//GameManager.instance.gameState = GameManager.GameStates.WORLD_STATE;
			//GameManager.instance.enemiesToBattle.Clear();
		}
	}


	private IEnumerator waitForVictory()
	{
		Debug.Log("Waitin for victory!");
		if (Input.GetKeyDown(KeyCode.A))
		{
			
			GameManager.instance.loadSceneAfterBattle();
			GameManager.instance.gameState = GameManager.GameStates.WORLD_STATE;
			GameManager.instance.enemiesToBattle.Clear();
		}
		else yield break;
	}
}

	/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour
{



	public enum PerformAction
	{
		WAIT,
		TAKEACTION,
		PERFORMANCE,
		CHECKALIVE,
		WIN,
		LOSE
	}
	public PerformAction battleStates;

	public List<HandleTurns> PerformList = new List<HandleTurns>();
	public List<GameObject> HeroesInBattle = new List<GameObject>();
	public List<GameObject> EnemyInCombat = new List<GameObject>();

	public enum HeroGUI
	{
		ACTIVATE,
		WAITING,
		ATTACK,
		SELECT,
		DONE
	}
	public HeroGUI HeroInput;
	public List<GameObject> HeroToControl = new List<GameObject>();
	private HandleTurns HeroChoice;

	public GameObject enemyButton;
	public Transform Spacer;

	public GameObject ActionPanel;
	public GameObject MagicPanel;
	public GameObject EnemySelectPanel;

	//Hero Action
	public Transform actionSpacer;
	public Transform magicSpacer;
	public GameObject actionButton;
	public GameObject magicButton;
	private List<GameObject> AtkButtons = new List<GameObject>();

	//Enemy Action
	private List<GameObject> EnemyBtns = new List<GameObject>();
	//spawn point
	public List<Transform> spawnPoint = new List<Transform>();
	

	void Start()
	{
		battleStates = PerformAction.WAIT;
		//EnemyInCombat.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
		HeroesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
		HeroInput = HeroGUI.ACTIVATE;

	
	}

	// Update is called once per frame
	void Update()
	{
		switch (battleStates)
		{
			case (PerformAction.WAIT):
				if (PerformList.Count > 0)
				{
					battleStates = PerformAction.TAKEACTION;
				}
				break;
			case (PerformAction.TAKEACTION):
				GameObject performer = GameObject.Find(PerformList[0].Attacker);
				if (PerformList[0].Type == "Enemy")
				{
					EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
					for (int i = 0; i < HeroesInBattle.Count; i++)
					{
						if (PerformList[0].AttackersTarget == HeroesInBattle[i])
						{
							ESM.HeroToAttack = PerformList[0].AttackersTarget;
							ESM.currentState = EnemyStateMachine.TurnState.ACTION;
							break;
						}
						else
						{
							PerformList[0].AttackersTarget = HeroesInBattle[Random.Range(0, HeroesInBattle.Count)];
							ESM.HeroToAttack = PerformList[0].AttackersTarget;
							ESM.currentState = EnemyStateMachine.TurnState.ACTION;
						}
					}
				}

				battleStates = PerformAction.PERFORMANCE;
				break;
			case (PerformAction.PERFORMANCE):
				break;

		}
	}
	

	public void CollectActions(HandleTurns input)
	{
		PerformList.Add(input);
	}

	
	//input 4
	
}
*/
 
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour
{

public enum PerformAction
{
	WAIT,
	TAKEACTION,
	PERFORMACTION
}

public PerformAction battleStates;

public List<HandleTurns> PerformList = new List<HandleTurns>();
public List<GameObject> HeroesInBattle = new List<GameObject>();
public List<GameObject> EnemiesInBattle = new List<GameObject>();

public enum HeroGUI
{
	ACTIVATE, //new instance of handleturns
	WAITING, //waiting turn
	INPUT1, //basic attack
	INPUT2, //select opponent
	DONE,
}

public HeroGUI Heroinput;

public List<GameObject> HeroesToManage = new List<GameObject>();
private HandleTurns HeroChoice;

public GameObject enemyButton;
public Transform Spacer;

public GameObject AttackPanel;
public GameObject EnemySelectPanel;
public GameObject MagicPanel;

//magic attack
public Transform actionSpacer;
public Transform magicSpacer;
public GameObject actionButton;
public GameObject magicButton;
private List<GameObject> atkBtns = new List<GameObject>();


// Use this for initialization
void Start()
{
	battleStates = PerformAction.WAIT;
	EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
	HeroesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
	Heroinput = HeroGUI.ACTIVATE;
	EnemyButtons();

	AttackPanel.SetActive(false);//make attack panel not show
	EnemySelectPanel.SetActive(false);//make enemy select panel not show unless told to
	MagicPanel.SetActive(false);

}

// Update is called once per frame
void Update()
{
	switch (battleStates)
	{
		case (PerformAction.WAIT):
			if (PerformList.Count > 0)
			{
				battleStates = PerformAction.TAKEACTION;
			}
			break;
		case (PerformAction.TAKEACTION):
			GameObject performer = GameObject.Find(PerformList[0].Attacker);//catch information
																			//know if enemy or hero character
			if (PerformList[0].Type == "Enemy")
			{
				EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();//catch enemy statemachine
				for (int i = 0; i < HeroesInBattle.Count; i++)//check if currently dead hero is in battle list
				{
					if (PerformList[0].AttackersTarget == HeroesInBattle[i])
					{
						ESM.HeroToAttack = PerformList[0].AttackersTarget;
						ESM.currentState = EnemyStateMachine.TurnState.ACTION;
						break;
					}
					else
					{
						PerformList[0].AttackersTarget = HeroesInBattle[Random.Range(0, HeroesInBattle.Count)];//if hero is dead choose another
						ESM.HeroToAttack = PerformList[0].AttackersTarget;
						ESM.currentState = EnemyStateMachine.TurnState.ACTION;
					}
					//attackers target is not in heroesinbattle list
				}


				ESM.HeroToAttack = PerformList[0].AttackersTarget;
				ESM.currentState = EnemyStateMachine.TurnState.ACTION;
			}
			if (PerformList[0].Type == "Hero")
			{
				HeroStateMachine HSM = performer.GetComponent<HeroStateMachine>();
				HSM.EnemyToAttack = PerformList[0].AttackersTarget;
				HSM.currentState = HeroStateMachine.TurnState.ACTION;
				//Debug.Log("Hero is here to perform");
			}
			battleStates = PerformAction.PERFORMACTION;
			break;
		case (PerformAction.PERFORMACTION):
			//idle state
			break;
	}

	switch (Heroinput)
	{
		case (HeroGUI.ACTIVATE):
			if (HeroesToManage.Count > 0)
			{
				HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(true);
				HeroChoice = new HandleTurns();
				AttackPanel.SetActive(true);
				Heroinput = HeroGUI.WAITING;
				//populate attack buttons
				CreateAttackButtons();
			}
			break;

		case (HeroGUI.WAITING):
			//idle
			break;

		case (HeroGUI.DONE):
			HeroInputDone();
			break;
	}
}

public void CollectActions(HandleTurns input)//collecting actions and giving access to the outside
{
	PerformList.Add(input);
}

void EnemyButtons()
{
	foreach (GameObject enemy in EnemiesInBattle)
	{
		GameObject newButton = Instantiate(enemyButton) as GameObject;
		EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();//connection from button above to pass in info
		EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine>();
		Text buttonText = newButton.transform.Find("Text").gameObject.GetComponent<Text>();// getting button associated with ESM
		buttonText.text = cur_enemy.enemy.theName;

		button.EnemyPrefab = enemy;
		newButton.transform.SetParent(Spacer, false); //deals with overflow

	}
}

public void Input1()//attack button
{
	HeroChoice.Attacker = HeroesToManage[0].name;
	HeroChoice.AttackersGameObject = HeroesToManage[0];//saving the hero game object of heroes to manage in attackersgameobject.
	HeroChoice.Type = "Hero";
	HeroChoice.chooseanAttack = HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attacks[0];//first attack is base attack
	AttackPanel.SetActive(false);
	EnemySelectPanel.SetActive(true); //we associated this with attack
}

public void Input2(GameObject chosenEnemy) //enemy selecton
{
	HeroChoice.AttackersTarget = chosenEnemy;
	Heroinput = HeroGUI.DONE;
}

void HeroInputDone()
{
	PerformList.Add(HeroChoice);
	EnemySelectPanel.SetActive(false);

	//clean the attackpanel
	foreach (GameObject atkBtn in atkBtns)
	{
		Destroy(atkBtn);
	}
	atkBtns.Clear();


	HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(false);
	HeroesToManage.RemoveAt(0);
	Heroinput = HeroGUI.ACTIVATE;
}

//create actionbuttons

void CreateAttackButtons()
{
	GameObject AttackButton = Instantiate(actionButton) as GameObject;//change text inside of button bottom
	Text AttackButtonText = AttackButton.transform.Find("Text").gameObject.GetComponent<Text>();
	AttackButtonText.text = "Attack";
	AttackButton.GetComponent<Button>().onClick.AddListener(() => Input1());//paste any function into onclick event. When we click that button it will perform input 1
	AttackButton.transform.SetParent(actionSpacer, false);
	atkBtns.Add(AttackButton);

	GameObject MagicAttackButton = Instantiate(actionButton) as GameObject;//change text inside of button bottom
	Text MagicAttackButtonText = MagicAttackButton.transform.Find("Text").gameObject.GetComponent<Text>();
	MagicAttackButtonText.text = "Magic";
	MagicAttackButton.GetComponent<Button>().onClick.AddListener(() => Input3());
	//

	MagicAttackButton.transform.SetParent(actionSpacer, false);
	atkBtns.Add(MagicAttackButton);

	if (HeroesToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks.Count > 0)
	{
		foreach (BaseAttack magicAtk in HeroesToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks) //for each spell create a button
		{
			GameObject MagicButton = Instantiate(magicButton);
			Text MagicButtonText = MagicButton.transform.Find("Text").gameObject.GetComponent<Text>();
			MagicButtonText.text = magicAtk.attackName;
			AttackButton ATB = MagicButton.GetComponent<AttackButton>();
			ATB.magicAttackToPerform = magicAtk;
			MagicButton.transform.SetParent(magicSpacer, false); //place buttons into right spacer (this should be how you add items to an inventory)
			atkBtns.Add(MagicButton);

			//make inventory by adding scripts to a hero and then populating the menu with those available in inventory
		}
	}
	else
	{
		MagicAttackButton.GetComponent<Button>().interactable = false;//false so we dont perform a magic atack
	}
}

public void Input4(BaseAttack chooseaMagic)//choose a magic attack
{
	HeroChoice.Attacker = HeroesToManage[0].name;
	HeroChoice.AttackersGameObject = HeroesToManage[0];//saving the hero game object of heroes to manage in attackersgameobject.
	HeroChoice.Type = "Hero";

	HeroChoice.chooseanAttack = chooseaMagic;
	MagicPanel.SetActive(false);
	EnemySelectPanel.SetActive(true);
}

public void Input3()//switching to magic attacks
{
	AttackPanel.SetActive(false);
	MagicPanel.SetActive(true);
}
}
*/

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour
{

	public enum PerformAction
	{
		WAIT,
		TAKEACTION,
		PERFORMACTION
	}

	public PerformAction battleStates;

	public List<HandleTurns> PerformList = new List<HandleTurns>();
	public List<GameObject> HeroesInBattle = new List<GameObject>();
	public List<GameObject> EnemiesInBattle = new List<GameObject>();

	public enum HeroGUI
	{
		ACTIVATE, //new instance of handleturns
		WAITING, //waiting turn
		INPUT1, //basic attack
		INPUT2, //select opponent
		DONE,
	}

	public HeroGUI Heroinput;

	public List<GameObject> HeroesToManage = new List<GameObject>();
	private HandleTurns HeroChoice;

	public GameObject enemyButton;
	public Transform Spacer;

	public GameObject AttackPanel;
	public GameObject EnemySelectPanel;
	public GameObject MagicPanel;

	//magic attack
	public Transform actionSpacer;
	public Transform magicSpacer;
	public GameObject actionButton;
	public GameObject magicButton;
	private List<GameObject> atkBtns = new List<GameObject>();


	// Use this for initialization
	void Start()
	{
		battleStates = PerformAction.WAIT;
		EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
		HeroesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
		Heroinput = HeroGUI.ACTIVATE;
		EnemyButtons();

		AttackPanel.SetActive(false);//make attack panel not show
		EnemySelectPanel.SetActive(false);//make enemy select panel not show unless told to
		MagicPanel.SetActive(false);

	}

	// Update is called once per frame
	void Update()
	{
		switch (battleStates)
		{
			case (PerformAction.WAIT):
				if (PerformList.Count > 0)
				{
					battleStates = PerformAction.TAKEACTION;
				}
				break;
			case (PerformAction.TAKEACTION):
				GameObject performer = GameObject.Find(PerformList[0].Attacker);//catch information
																				//know if enemy or hero character
				if (PerformList[0].Type == "Enemy")
				{
					EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();//catch enemy statemachine
					for (int i = 0; i < HeroesInBattle.Count; i++)//check if currently dead hero is in battle list
					{
						if (PerformList[0].AttackersTarget == HeroesInBattle[i])
						{
							ESM.HeroToAttack = PerformList[0].AttackersTarget;
							ESM.currentState = EnemyStateMachine.TurnState.ACTION;
							break;
						}
						else
						{
							PerformList[0].AttackersTarget = HeroesInBattle[Random.Range(0, HeroesInBattle.Count)];//if hero is dead choose another
							ESM.HeroToAttack = PerformList[0].AttackersTarget;
							ESM.currentState = EnemyStateMachine.TurnState.ACTION;
						}
						//attackers target is not in heroesinbattle list
					}


					ESM.HeroToAttack = PerformList[0].AttackersTarget;
					ESM.currentState = EnemyStateMachine.TurnState.ACTION;
				}
				if (PerformList[0].Type == "Hero")
				{
					HeroStateMachine HSM = performer.GetComponent<HeroStateMachine>();
					HSM.EnemyToAttack = PerformList[0].AttackersTarget;
					HSM.currentState = HeroStateMachine.TurnState.ACTION;
					//Debug.Log("Hero is here to perform");
				}
				battleStates = PerformAction.PERFORMACTION;
				break;
			case (PerformAction.PERFORMACTION):
				//idle state
				break;
		}

		switch (Heroinput)
		{
			case (HeroGUI.ACTIVATE):
				if (HeroesToManage.Count > 0)
				{
					HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(true);
					HeroChoice = new HandleTurns();
					AttackPanel.SetActive(true);
					Heroinput = HeroGUI.WAITING;
					//populate attack buttons
					CreateAttackButtons();
				}
				break;

			case (HeroGUI.WAITING):
				//idle
				break;

			case (HeroGUI.DONE):
				HeroInputDone();
				break;
		}
	}

	public void CollectActions(HandleTurns input)//collecting actions and giving access to the outside
	{
		PerformList.Add(input);
	}

	void EnemyButtons()
	{
		foreach (GameObject enemy in EnemiesInBattle)
		{
			GameObject newButton = Instantiate(enemyButton) as GameObject;
			EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();//connection from button above to pass in info
			EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine>();
			Text buttonText = newButton.transform.Find("Text").gameObject.GetComponent<Text>();// getting button associated with ESM
			buttonText.text = cur_enemy.enemy.theName;

			button.EnemyPrefab = enemy;
			newButton.transform.SetParent(Spacer, false); //deals with overflow

		}
	}

	public void Input1()//attack button
	{
		HeroChoice.Attacker = HeroesToManage[0].name;
		HeroChoice.AttackersGameObject = HeroesToManage[0];//saving the hero game object of heroes to manage in attackersgameobject.
		HeroChoice.Type = "Hero";
		HeroChoice.chooseanAttack = HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attacks[0];//first attack is base attack
		AttackPanel.SetActive(false);
		EnemySelectPanel.SetActive(true); //we associated this with attack
	}

	public void Input2(GameObject chosenEnemy) //enemy selecton
	{
		HeroChoice.AttackersTarget = chosenEnemy;
		Heroinput = HeroGUI.DONE;
	}

	void HeroInputDone()
	{
		PerformList.Add(HeroChoice);
		EnemySelectPanel.SetActive(false);

		//clean the attackpanel
		foreach (GameObject atkBtn in atkBtns)
		{
			Destroy(atkBtn);
		}
		atkBtns.Clear();


		HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(false);
		HeroesToManage.RemoveAt(0);
		Heroinput = HeroGUI.ACTIVATE;
	}

	//create actionbuttons

	void CreateAttackButtons()
	{
		GameObject AttackButton = Instantiate(actionButton) as GameObject;//change text inside of button bottom
		Text AttackButtonText = AttackButton.transform.Find("Text").gameObject.GetComponent<Text>();
		AttackButtonText.text = "Attack";
		AttackButton.GetComponent<Button>().onClick.AddListener(() => Input1());//paste any function into onclick event. When we click that button it will perform input 1
		AttackButton.transform.SetParent(actionSpacer, false);
		atkBtns.Add(AttackButton);

		GameObject MagicAttackButton = Instantiate(actionButton) as GameObject;//change text inside of button bottom
		Text MagicAttackButtonText = MagicAttackButton.transform.Find("Text").gameObject.GetComponent<Text>();
		MagicAttackButtonText.text = "Magic";
		MagicAttackButton.GetComponent<Button>().onClick.AddListener(() => Input3());
		//

		MagicAttackButton.transform.SetParent(actionSpacer, false);
		atkBtns.Add(MagicAttackButton);

		if (HeroesToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks.Count > 0)
		{
			foreach (BaseAttack magicAtk in HeroesToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks) //for each spell create a button
			{
				GameObject MagicButton = Instantiate(magicButton);
				Text MagicButtonText = MagicButton.transform.Find("Text").gameObject.GetComponent<Text>();
				MagicButtonText.text = magicAtk.attackName;
				AttackButton ATB = MagicButton.GetComponent<AttackButton>();
				ATB.magicAttackToPerform = magicAtk;
				MagicButton.transform.SetParent(magicSpacer, false); //place buttons into right spacer (this should be how you add items to an inventory)
				atkBtns.Add(MagicButton);

				//make inventory by adding scripts to a hero and then populating the menu with those available in inventory
			}
		}
		else
		{
			MagicAttackButton.GetComponent<Button>().interactable = false;//false so we dont perform a magic atack
		}
	}

	public void Input4(BaseAttack chooseaMagic)//choose a magic attack
	{
		HeroChoice.Attacker = HeroesToManage[0].name;
		HeroChoice.AttackersGameObject = HeroesToManage[0];//saving the hero game object of heroes to manage in attackersgameobject.
		HeroChoice.Type = "Hero";

		HeroChoice.chooseanAttack = chooseaMagic;
		MagicPanel.SetActive(false);
		EnemySelectPanel.SetActive(true);
	}

	public void Input3()//switching to magic attacks
	{
		AttackPanel.SetActive(false);
		MagicPanel.SetActive(true);
	}
}
*/
