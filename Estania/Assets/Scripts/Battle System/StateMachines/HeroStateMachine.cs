//adding to flamejavelin and backstab an if mariamhero
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour
{
	public List<bool> isChainList = new List<bool>();
	public Transform BattleCanvas;
	public GameObject hitPanel;
	public Transform hitSpacer1;
	public Transform hitSpacer2;
	public GameObject chaintext;
	private int chainCount;
	public List<GameObject> firstNumberList = new List<GameObject>();
	public List<GameObject> secondNumberList = new List<GameObject>();
	public int firstNumber;
	public int secondNumber;
	public GameObject one;
	public GameObject two;
	public GameObject three;
	public GameObject four;
	public GameObject five;
	public GameObject six;
	public GameObject seven;
	public GameObject eight;
	public GameObject nine;
	public GameObject zero;
	//AudioSource audioData;
	//AudioSource audioData2;

	public GameObject flameJavelinAudio;
	public GameObject backStabAudio;
	public enemyencounterspawner EES;
	public bool isDeathBlow;
	public List<GameObject> dmgBtns = new List<GameObject>();
	public List<string> comboTracker = new List<string>();
	public List<string> flameJavelinTracker = new List<string>(new string[] { "Triangle", "Circle" });
	public List<string> backStabTracker = new List<string>(new string[] { "Square", "Circle" });
	public List<bool> flameJavelinBools = new List<bool>(new bool[] { false, false });
	public List<bool> backStabBools = new List<bool>(new bool[] { false, false });
	int k;
	bool isFlameJavelin;
	bool isBackStab;
	public int flameJavelinIndex;
	public int comboIndex;
	public int newComboIndex;
	public int newComboIndexCheck;
	public Transform deathAnim;
	public bool deathAnimTimer;
	public Rigidbody2D rigidbod;
	public GameObject rigidbody;
	public Text DMGTEXT;
	public GameObject squareButton;
	public bool earlyReturn;
	public bool battleAnim;
	public bool running;
	public int j;
	public bool dontattackme;
	public bool atPosition;
	public BaseHero hero;
	private BattleStateMachine BSM;
	private BattleStateMachine BSM2;
	public GameObject BSMa;
	public Animation stickcleave;
	private GameManager GMan;
	public GameObject GMana;
	public bool isCombo;
	public EnemyStateMachine ESM;
	public Transform ListedComboSpacer;
	public GameObject triangleButton;
	public GameObject circleButton;
	private List<GameObject> comboBtns = new List<GameObject>();
	public int comboNumber;
	public bool AHit;
	public bool WHit;
	public bool DHit;
	public string attackName;
	public float damage;

	public List<GameObject> chainBtns = new List<GameObject>();

	public Text CHAINTEXT;

	public enum TurnState
	{
		PROCESSING, //bar fills
		ADDTOLIST, //add hero to a list
		WAITING, //idle state
		SELECTING, //player selects action which we create later on
		ACTION, //can do an action
		DEAD
	}

	public TurnState currentState;
	//for the progress bar
	public bool attacking;

	public float cur_cooldown = 0f;
	private float max_cooldown = 5f;
	public Image ProgressBar;
	public GameObject Selector;

	public GameObject EnemyToAttack;
	public Animator playerAnimator;
	public bool actionStarted = false;
	private Vector3 startposition;
	private float animSpeed = 10f;
	private bool alive = true;
	//hero Panel
	public HeroPanelStats stats;
	public GameObject HeroPanel;
	private Transform HeroPanelSpacer;
	public GameObject apText;
	int flameJavelinCount;

	Text AP_Text;
	// Use this for initialization
	void Start()
	{
		isChainList.Add(false);
		//isChainList.Add(false);
		//isChainList.Add(false);
		chainCount = 0;
		
		hitPanel.SetActive(false);
		isFlameJavelin = false;
		isBackStab = false;
		k = 0;
		newComboIndex = 0;
		newComboIndexCheck = 0;
		isDeathBlow = false;
		flameJavelinCount = 2;
		/*
		newComboIndex = comboIndex;
		for (int i = 0; i <= flameJavelinCount; i++)
		{
			if (i == flameJavelinIndex && isCombo == false) //if we have no combo and we're went through a round without finding the last combo to be a deathblow execute
			{ flameJavelinIndex = 0;
				foreach (bool itsabool in flameJavelinBools)
					itsabool = false;
				newComboIndex = 0;
				newComboIndexCheck = 
			}
			for (int j = newComboIndex; j <= newComboIndexCheck; j++)
			{	
				if (flameJavelinTracker[i] == comboTracker[j])
					{ comboIsTrue == true;
					flameJavelinBools[flameJavelinIndex] == true;
					flameJavelinIndex++;
					newComboIndex = j + 1;
					newComboIndexCheck = j + 2; 
					if (comboIsTrue && ((j + 1) == comboIndex))// if the 
					break;
				}
				isCombo = false;
				//if ((j + 1) == comboIndex)//if at the last possible combo 
				//{ comboIsTrue == false; }
			}
		}
	for (int i = 0; i <= flameJavelinCount; i++)
		{if (flameJavelinTracker[i] == false)
				break;
			else if (((i + 1) == flameJavelineCount) && flameJavelinTracker[flameJavelinCount - 1] == true) //if at last index in the tracker for flame javelin and the last index has a value of true 
				;//execute flame javelin
		}
	*/




		//flame javelin will require a specific animation and attack sequence that will only be run if the bool is true.
		//need to start combotracker over after the count of attacks has been either passed without the finisher or if the comboIsTrue breaks
		//use list of booleans and reset list to false if combo breaks
		//once the there is the first match, the following matches have to check ONLY the first try, and if the first try doesnt work you have to reset.
		//reset combo tracker after execution
		// AWWD     or AWWAD  or (it's going to go through entire list looking for A. If it finds A, it needs to continue to search for another A if it fails the first time. 

		//if it looks through all those in the combo. It goes to A and says, ok, do a loop to see if A ='s the first part of the deathblow, then check if the combo after A is the in the deathblow, etc.

		// for all combos
		// for all deathblows
		// check if combo == deathblow. If not, break; otherwise, continue. if all deathblow items are reached and we start and we start at the next combo, check to see if deathblow bools are all true, and if so break and
		//execute th combbo
		//if flamejavelin use a separate function for attack delay and execute.

		//audioData = GameObject.Find("FlameJavelinAudio").GetComponent<AudioSource>();
		//audioData2 = GameObject.Find("BackStabAudio").GetComponent<AudioSource>();
		EES = GameObject.Find("BeastSpawner").GetComponent<enemyencounterspawner>();
		EES = GameObject.Find("BeastSpawner").GetComponent<enemyencounterspawner>();
		deathAnimTimer = false;
		AHit = false;
		WHit = false;
		DHit = false;
		comboNumber = 0;
		//ap_update();
		//GameObject ApText = Instantiate(apText) as GameObject;//change text inside of button bottom
		AP_Text = GameObject.Find("Hero 1").GetComponent<HeroStateMachine>().apText.gameObject.GetComponent<Text>();
		//AP_Text = ApText.transform.Find("Text").gameObject.GetComponent<Text>();

		earlyReturn = false;
		battleAnim = false;
		running = false;
		isCombo = false;
		GMana = GameObject.Find("GameManager");
		BSMa = GameObject.Find("BattleManager");
		playerAnimator = GameObject.Find("Hero 1").GetComponent<Animator>();
		GMan = GMana.GetComponent<GameManager>();
		BSM2 = BSMa.GetComponent<BattleStateMachine>();
		HeroPanelSpacer = GameObject.Find("BattleCanvas").transform.Find("HeroPanel").transform.Find("HeroPanelSpacer");
		CreateHeroPanel();
		//create panel, fill in info 


		//find spacer
		startposition = transform.position;
		cur_cooldown = Random.Range(0, 2.5f);
		Selector.SetActive(false);
		BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>(); //connects enemystatemachine to battlestatemachine
		currentState = TurnState.PROCESSING;
		j = 0;
	}

	// Update is called once per frame
	void Update() //if enemy is dead automatically cancel and run back
	{

		StartCoroutine(waitingForCompletion());
		if (!isCombo)
		{
			ap_update();
		}


		if ((Input.GetKeyDown(KeyCode.S) == true) && attacking != true && isCombo && !running && !battleAnim && !earlyReturn)
		{

			dontattackme = true;
			Debug.Log("Perform list");
			//BSM.PerformList.RemoveAt(0);
			isCombo = false;
			StartCoroutine(ReturnToSpot());


			GMan.ap += 3;
			ap_update();
			BSM.ComboPanel.SetActive(false);

			//reset BSM -> WAIT
			if (!dontattackme)
			{


				Debug.Log("Removing performlist!");

			}
			dontattackme = true;

			//reset BSM -> WAIT
		}
		/*if ((Input.GetKeyDown(KeyCode.S) == true) && isCombo && !attacking && atPosition)	
		{
			Debug.Log("Perform list");
			//BSM.PerformList.RemoveAt(0);
			isCombo = false;
			StartCoroutine(ReturnToSpot());
			BSM.PerformList.RemoveAt(0);

			GMan.ap = 3;
			BSM.ComboPanel.SetActive(false);
			//reset BSM -> WAIT
			if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
			{
				BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
				cur_cooldown = 0f;
				currentState = TurnState.PROCESSING;

			}
			//reset the battle state machine and set monster to wait
			Debug.Log("Perform Action Wait");
			BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
			//end coroutine
			Debug.Log("Action false");
			actionStarted = false;
			//reset this enemy state
			Debug.Log("Current cooldown = 0");
			cur_cooldown = 0f;
			Debug.Log("Turnstate.PROCESSING");
			currentState = TurnState.PROCESSING;
		}
		*/
		else if (Input.GetKeyDown(KeyCode.W) == true && attacking != true && isCombo && !running && !battleAnim && !earlyReturn)
		{

			comboTracker.Add("Triangle");

			for (int i = 0; i < comboTracker.Count -1; i++)
				if (comboTracker[i] == "Triangle" && comboTracker[i + 1] == "Triangle")
			{
				comboTracker.Remove(comboTracker[0]);
				chainCount = 0;
					break;
			}
			WHit = true;
			comboNumber += 1;
			hitFunction();
			Debug.Log("Subtract ap");
			GMan.ap -= 1;
			ap_update();
			Debug.Log(GMan.ap);
			GameObject comboButton = Instantiate(triangleButton) as GameObject;
			comboBtns.Add(comboButton);
			comboButton.transform.SetParent(ListedComboSpacer, false); //deals with overflow

			Debug.Log("animate cleave");
			if (!GMan.MariamHero)
				playerAnimator.Play("FightManRight_Punch");
			else
				playerAnimator.Play("Mariam_Attack_0");

			//playerAnimator.Play("stickcleave");
			attacking = true;
			BSM.HeroChoice.chooseanAttack = GameObject.Find("Hero 1").GetComponent<HeroStateMachine>().hero.attacks[0];
			StartCoroutine(AttackDelay());
			Debug.Log("attacking is true");

			Debug.Log("action started is true");
			//actionStarted = true;
			//yield return new WaitForSeconds(2f);
			Debug.Log("choose attack");
			//BSM.HeroChoice.chooseanAttack = BSM.HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attacks[0]; // was 1

			Debug.Log("damage");
			//	doDamage();

			attacking = false;
			Debug.Log("stickidle");

			//playerAnimator.Play("stickrunning");
		}

		else if (Input.GetKeyDown(KeyCode.A) == true && attacking != true && isCombo && !running && !battleAnim && !earlyReturn)
		{
			comboTracker.Add("Square");
			AHit = true;
			comboNumber += 1;
			hitFunction();
			Debug.Log("Subtract ap");
			GMan.ap -= 1;
			ap_update();
			Debug.Log(GMan.ap);
			GameObject comboButton = Instantiate(squareButton) as GameObject;
			comboBtns.Add(comboButton);
			comboButton.transform.SetParent(ListedComboSpacer, false); //deals with overflow

			Debug.Log("animate cleave");

			if (!GMan.MariamHero)
				playerAnimator.Play("FightManRight_Punch_1");
			else
				playerAnimator.Play("Mariam_Attack_1");

			attacking = true;
			BSM.HeroChoice.chooseanAttack = GameObject.Find("Hero 1").GetComponent<HeroStateMachine>().hero.attacks[0];
			StartCoroutine(AttackDelay());
			Debug.Log("attacking is true");

			Debug.Log("action started is true");
			//actionStarted = true;
			//yield return new WaitForSeconds(2f);
			Debug.Log("choose attack");
			//BSM.HeroChoice.chooseanAttack = BSM.HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attacks[0]; // was 1

			Debug.Log("damage");
			//	doDamage();

			attacking = false;
			Debug.Log("stickidle");

			//playerAnimator.Play("stickrunning");
		}

		else if (Input.GetKeyDown(KeyCode.D) == true && attacking != true && isCombo && !running && !battleAnim && !earlyReturn)
		{

			
			attacking = true;
			comboTracker.Add("Circle");
			DHit = true;
			comboNumber += 1;
			hitFunction();
			Debug.Log("Subtract ap");
			GMan.ap -= 1;
			ap_update();
			Debug.Log(GMan.ap);
			GameObject comboButton = Instantiate(circleButton) as GameObject;
			comboButton.transform.SetParent(ListedComboSpacer, false); //deals with overflow
			comboBtns.Add(comboButton);
			if (GMan.MariamHero)
			{ for (int i = 0; i < comboTracker.Count && !isFlameJavelin; i++)
				{ Debug.Log("checking combo" + comboTracker[i]);
					for (int j = 0; j <= flameJavelinTracker.Count; j++)
					{ Debug.Log("checking deathblow" + flameJavelinTracker[j]);
						if (flameJavelinBools[j] == false)
							break;
						else if (((j + 1) == flameJavelinTracker.Count) && flameJavelinBools[j] == true) //if at last index in the tracker for flame javelin and the last index has a value of true 
							;//execute flame javelin
					}
					for (int j = 0, k = i; k < comboTracker.Count && j < flameJavelinTracker.Count; j++, k++)
					{
						Debug.Log("flamejavelintracker count = " + flameJavelinTracker.Count);
						Debug.Log("j = " + j + "k = " + k);
						if (comboTracker[k] == flameJavelinTracker[j])
						{
							Debug.Log(comboTracker[k] + " = " + flameJavelinTracker[j]);
							flameJavelinBools[j] = true;
							Debug.Log("truth for flame javelin!");
							if (((j + 1) == flameJavelinTracker.Count) && flameJavelinBools[j] == true)
							{
								Debug.Log("FLAME JAVELIN!");
								isFlameJavelin = true;
								isChainList[0] = true;
							}
						}
						else
						{
							flameJavelinBools[j] = false;
							//Debug.Log("FLAMEING BLOOLS IS FALSE!");
							isChainList[0] = false;
							if (flameJavelinBools[0] == true)
								break;
							else
								chainCount = 0;
							break;
						}
					} }
			}
			k = 0;
			for (int i = 0; i < flameJavelinTracker.Count; i++)
				flameJavelinBools[i] = false;
			//check all combos to see if you're on a combo path; if not on a combo path, reset the combo list;


			if (isFlameJavelin)
			{
				attacking = true;
				int l = comboTracker.Count;
				for (int i = 0; i < l; i++)
					comboTracker.Remove(comboTracker[0]);
				//.Play("FlameJave");
				StartCoroutine(AttackDelayFlameJavelin());
				Debug.Log("Want to use flame javelin!");
				for (int i = 0; i < 1; i++) 
				{ if (isChainList[i] == true)
					{
						if (i == 0)
							StartCoroutine(chainFunction());
									

					}
					else break;
				}
			}//execute flameJavelin
			Debug.Log("animate cleave");

			if (GMan.MariamHero)
			{ for (int i = 0; i < comboTracker.Count && !isBackStab; i++)
				{
					Debug.Log("checking combo" + comboTracker[i]);
					for (int j = 0; j <= backStabTracker.Count; j++)
					{
						Debug.Log("checking deathblow" + flameJavelinTracker[j]);
						if (backStabBools[j] == false)
							break;
						else if (((j + 1) == backStabTracker.Count) && backStabBools[j] == true) //if at last index in the tracker for flame javelin and the last index has a value of true 
							;//execute flame javelin
					}
					for (int j = 0, k = i; k < comboTracker.Count && j < backStabTracker.Count; j++, k++)
					{
						Debug.Log("flamejavelintracker count = " + flameJavelinTracker.Count);
						Debug.Log("j = " + j + "k = " + k);
						if (comboTracker[k] == backStabTracker[j])
						{
							Debug.Log(comboTracker[k] + " = " + flameJavelinTracker[j]);
							backStabBools[j] = true;
							Debug.Log("truth for flame javelin!");
							if (((j + 1) == backStabTracker.Count) && backStabBools[j] == true)
							{
								Debug.Log("FLAME JAVELIN!");
								isBackStab = true;
								isChainList[0] = true;
							}
						}
						else
						{
							backStabBools[j] = false;
							//Debug.Log("FLAMEING BLOOLS IS FALSE!");
							isChainList[0] = false;
							if (backStabBools[0] == true)
								break;
							else
								chainCount = 0;
							break;
						}
					}
				}
			}
			k = 0;
			for (int i = 0; i < backStabTracker.Count; i++)
				backStabBools[i] = false;
			//check all combos to see if you're on a combo path; if not on a combo path, reset the combo list;


			if (isBackStab)
			{
				attacking = true;
				int l = comboTracker.Count;
				for (int i = 0; i < l; i++)
					comboTracker.Remove(comboTracker[0]);
				//.Play("FlameJave");
				StartCoroutine(AttackDelayBackStab());
				Debug.Log("Want to use flame javelin!");
				for (int i = 0; i < 1; i++)
				{
					if (isChainList[i] == true)
					{
						if (i == 0)
							StartCoroutine(chainFunction());


					}
					else break;
				}
			}//execute flameJavelin


			//playerAnimator.Play(stickfigurecleave);
			if (!isFlameJavelin && !isBackStab)
			{
				if (!GMan.MariamHero)
					playerAnimator.Play("FightManRight_Punch_2");
				else
					playerAnimator.Play("Mariam_Attack_2");
				attacking = true;
				BSM.HeroChoice.chooseanAttack = GameObject.Find("Hero 1").GetComponent<HeroStateMachine>().hero.attacks[0];
				StartCoroutine(AttackDelay());
				Debug.Log("attacking is true");

				Debug.Log("action started is true");
				//actionStarted = true;
				//yield return new WaitForSeconds(2f);
				Debug.Log("choose attack");
				//BSM.HeroChoice.chooseanAttack = BSM.HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attacks[0]; // was 1

				Debug.Log("damage");
				//	doDamage();
			}
			attacking = false;
			isFlameJavelin = false;
			isBackStab = false;
			Debug.Log("stickidle");

			//playerAnimator.Play("stickrunning");
		}


		if (Input.GetKeyDown(KeyCode.S) == true)
			Debug.Log("HOO!");
		//Debug.Log(currentState);

		switch (currentState)
		{

			case (TurnState.PROCESSING):
				UpdateProgressBar();
				break;

			case (TurnState.ADDTOLIST):
				BSM.HeroesToManage.Add(this.gameObject);
				currentState = TurnState.WAITING;
				break;

			case (TurnState.WAITING):
				//idle
				break;

			case (TurnState.SELECTING):

				break;

			case (TurnState.ACTION):
				StartCoroutine(TimeForAction());
				break;

			case (TurnState.DEAD):
				if (!alive)
				{
					return;
				}
				else
				{
					this.gameObject.tag = "DeadHero";
					//change tag

					//change attackable by enemy 

					BSM.HeroesInBattle.Remove(this.gameObject);

					//not able to manage hero anymore

					BSM.HeroesToManage.Remove(this.gameObject);

					//deactive the selector if it's on

					Selector.SetActive(false);

					//reset gui (get rid of attack / select panel)

					BSM.AttackPanel.SetActive(false);
					BSM.EnemySelectPanel.SetActive(false);
					//remove item from performlist

					if (BSM.HeroesInBattle.Count > 0)
					{
						for (int i = 0; i < BSM.PerformList.Count; i++)
						{
							if (i != 0)
							{
								if (BSM.PerformList[i].AttackersGameObject == this.gameObject) //if we are current attacker
								{
									BSM.PerformList.Remove(BSM.PerformList[i]);

								}

								else if (BSM.PerformList[i].AttackersTarget == this.gameObject)
								{
									BSM.PerformList[i].AttackersTarget = BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)];
								}
							}
						}
					}
					//change color / play animation

					this.gameObject.GetComponent<SpriteRenderer>().material.color = new Color32(105, 105, 105, 255);

					BSM.battleStates = BattleStateMachine.PerformAction.CHECKALIVE; //if other hero is alive.
					alive = false;

					//reset the heroinput


					alive = false;
				}
				break;


		}


	}

	void UpdateProgressBar()
	{

		cur_cooldown = cur_cooldown + Time.deltaTime;
		float calc_cooldown = cur_cooldown / max_cooldown;
		ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calc_cooldown, 0, 1), ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
		if (cur_cooldown >= max_cooldown)
		{
			currentState = TurnState.ADDTOLIST;
		}
	}

	private IEnumerator AttackDelay()
	{
		actionStarted = true;

		actionStarted = true;
		attacking = true;
		if (attacking)
			Debug.Log("ATTACKING!");
		Debug.Log("AttackDelay");
		//stickcleave = GameObject.Find("Hero 1").GetComponent<Animation>().name;
		RuntimeAnimatorController ac = playerAnimator.runtimeAnimatorController;
		if (!GMan.MariamHero)
		{
            if (WHit == true)
            {
                attackName = "FightManRight_Punch";
                AudioManager.instance.PlaySound("punch2", transform.position, 1);
            }
				
			else if (AHit == true)
            {
                attackName = "FightManRight_Punch_1";
                AudioManager.instance.PlaySound("punch", transform.position, 1);
            }
				
			else if (DHit == true)
            {
                attackName = "FightManRight_Punch_2";
                AudioManager.instance.PlaySound("punch1", transform.position, 1);
            }
				
		}
		else
		{
            if (WHit == true)
            {
                attackName = "Mariam_Attack_0";
                AudioManager.instance.PlaySound("slash1", transform.position, 1);
            }

            else if (AHit == true)
            {
                attackName = "Mariam_Attack_1";
                AudioManager.instance.PlaySound("slash2", transform.position, 1);
            }


            else if (DHit == true)
            {
                attackName = "Mariam_Attack_2";
                AudioManager.instance.PlaySound("slash3", transform.position, 1);
            }
                
		}
		for (int i = 0; i < ac.animationClips.Length; i++)
		{
			Debug.Log(ac.animationClips[i].name);
			if (ac.animationClips[i].name == attackName)
			{
				AHit = false;
				WHit = false;
				DHit = false;
				Debug.Log("stickcleave timetowait");
				battleAnim = true;
				yield return new WaitForSeconds(ac.animationClips[i].length);
				doDamage(); //switched places with button command in update
				if (BSM.PerformList[0].Type == "Hero")
				{
					//EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();//catch enemy statemachine
					for (int k = 0; k < BSM.EnemiesInBattle.Count; k++)//check if currently dead hero is in battle list
					{
						Debug.Log("Current number of enemies in battle: " + k + 1);
						ESM = GameObject.Find(BSM.PerformList[0].AttackersTarget.name).GetComponent<EnemyStateMachine>();
						//EnemyStateMachine tempEnemy = GameObject.Find(BSM.PerformList[0].AttackersTarget.name).GetComponent<EnemyStateMachine>().currentState;

						//if (tempEnemy.TurnState != DEAD)
						if (GameObject.Find(BSM.PerformList[0].AttackersTarget.name).GetComponent<EnemyStateMachine>().currentState != EnemyStateMachine.TurnState.DEAD)
						//if (BSM.PerformList[0].AttackersTarget == BSM.EnemiesInBattle[k])
						{
							Debug.Log("Attacked same person!");
							EnemyToAttack = BSM.PerformList[0].AttackersTarget;
							currentState = TurnState.ACTION;
							break;
						}
						else
						{
							Debug.Log("Early return");
							earlyReturn = true;
							StartCoroutine(ReturnToSpot());
							GMan.ap += 3;
							ap_update();
							BSM.ComboPanel.SetActive(false);
							{
								battleAnim = false;
								earlyReturn = false;
								yield break; };
							//reset BSM -> WAIT
							if (!dontattackme)
							{


								Debug.Log("Removing performlist!");

							}
							dontattackme = true;

						}
					}
				}

				attacking = false;
				if (!attacking)
					Debug.Log("NOT ATTACKING!");
				if (!GMan.MariamHero)
					playerAnimator.Play("FightManRight_Idle");
				else
					playerAnimator.Play("Mariam_Idle");
				//playerAnimator.Play("stickfigureidle");
			}


		}
		battleAnim = false;
		//if (actionStarted && GMan.ap != 0)
		//{
		//	yield break;
		//	}

		if (actionStarted && earlyReturn)
		{ earlyReturn = false;
			yield break; }



		if (GMan.ap == 0 || (Input.GetKeyDown(KeyCode.S) == true) && isCombo && !attacking && !earlyReturn)
		{
			dontattackme = true;
			Debug.Log("Perform list");
			//BSM.PerformList.RemoveAt(0);
			isCombo = false;
			StartCoroutine(ReturnToSpot());


			GMan.ap += 3;
			ap_update();
			BSM.ComboPanel.SetActive(false);
			{ yield break; };
			//reset BSM -> WAIT
			if (!dontattackme)
			{


				Debug.Log("Removing performlist!");

			}
			dontattackme = true;

			//reset BSM -> WAIT

		}
		earlyReturn = false;
		if (actionStarted)
		{ yield break; }
	}

	private IEnumerator waitingForCompletion()

	{

		yield return null; }
	private IEnumerator TimeForAction()
	{
		if (BSM.magicalAtk)
		{
			//BSM.magicalAtk = false;
			attacking = true;
			if (actionStarted)
			{
				yield break;
			}
			actionStarted = true;

			//animate enemy near the hero to attack

			// run to enemy

			Vector3 magicStart = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);

			// EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z
			Vector3 magicEnd = new Vector3(this.gameObject.transform.position.x - 1.5f, this.gameObject.transform.position.y, this.gameObject.transform.position.z);//increasing target's x position by 1.5f units to give space to attack

			//playerAnimator.Play("stickrunleft");
			//running = true;
			//MoveTowardsEnemy(heroPosition);
			//RuntimeAnimatorController ac = playerAnimator.runtimeAnimatorController;
			while (MoveTowardsEnemy(magicEnd))
			{
                AudioManager.instance.PlaySound("enchant", transform.position, 1);
                playerAnimator.Play("Mariam_Move_Left");
				//playerAnimator.Play("stickrunleft");
				//running = true;
				yield return null;
			}
			playerAnimator.Play("Mariam_CastSpell");
			RuntimeAnimatorController ac = playerAnimator.runtimeAnimatorController;

			for (int i = 0; i < ac.animationClips.Length; i++)
			{
				Debug.Log(ac.animationClips[i].name);
				if (ac.animationClips[i].name == "Mariam_CastSpell")
				{

					//battleAnim = true;
					yield return new WaitForSeconds(ac.animationClips[i].length);
				}
			}
			//StartCoroutine(ReturnToSpot());
			for (int i = 0; i < ac.animationClips.Length; i++)
			{
				Debug.Log(ac.animationClips[i].name);
				if (ac.animationClips[i].name == "Mariam_CastSpell")
				{

					//battleAnim = true;
					yield return new WaitForSeconds(ac.animationClips[i].length);
				}
			}

            AudioManager.instance.PlaySound("Fire", transform.position, 1);
            doDamage();
			StartCoroutine(ReturnToSpot());
			//running = false;

			//atPosition = true;


			Vector3 firstPosition = startposition;

			//if (!GMan.MariamHero)
			//playerAnimator.Play("FightManRight_Idle");
			//else
			playerAnimator.Play("Mariam_Idle");

		}

		else
		{
			isCombo = true;
			if (actionStarted)
			{
				yield break;
			}
			actionStarted = true;

			//animate enemy near the hero to attack

			// run to enemy


			Vector3 heroPosition = new Vector3(EnemyToAttack.transform.position.x + 1.5f, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z);//increasing target's x position by 1.5f units to give space to attack
			if (GMan.MariamHero && !EES.boss)
				heroPosition = new Vector3(EnemyToAttack.transform.position.x + 2.75f, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z);
			else if (EES.boss)
				heroPosition = new Vector3(EnemyToAttack.transform.position.x + 6.5f, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z);
			//playerAnimator.Play("stickrunleft");
			//running = true;
			//MoveTowardsEnemy(heroPosition);
			//RuntimeAnimatorController ac = playerAnimator.runtimeAnimatorController;
			while (MoveTowardsEnemy(heroPosition))
			{
				if (!GMan.MariamHero)
					playerAnimator.Play("FightManRight_Forward");
				else
					playerAnimator.Play("Mariam_MoveToAttack");
				//playerAnimator.Play("stickrunleft");
				running = true;
				yield return null;
			}
			running = false;
			/*for (int i = 0; i < ac.animationClips.Length; i++)
			{
				Debug.Log(ac.animationClips[i].name);
				if (ac.animationClips[i].name == "stickrunleft")
				{
					Debug.Log("stickrunleft timetowait");
					yield return new WaitForSeconds(10f);// see if we can do an action while during wait for seconds
					running = false;
					playerAnimator.Play("stickfigureidle");
				}
			}*/


			/*while (MoveTowardsEnemy(heroPosition))
			{
				playerAnimator.Play("stickrunleft");
				yield return null;
			}*/
			atPosition = true;
			//	playerAnimator.Play("stickidle");//animate idle
			if (GMan.ap == 3)
				Debug.Log("HIYA!");
			//do { yield return new WaitForSeconds(2f); }
			//while (Input.GetKeyDown(KeyCode.S) != true);
			Debug.Log("Holy Smokes!");
			//while (Input.GetKeyDown(KeyCode.S) != true);

			/*
			while (GMan.ap != 0 || (Input.GetKeyDown(KeyCode.S) == true))
			{
				if (Input.GetKeyDown(KeyCode.W) == true && attacking != true)
				{
					GMan.ap -= 2;
					playerAnimator.Play("stickcleave");
					attacking = true;
					actionStarted = true;
					yield return new WaitForSeconds(2f);
					BSM.HeroChoice.chooseanAttack = BSM.HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attacks[0]; // was 0
					doDamage();
					attacking = false;
					playerAnimator.Play("stickidle");//animate idle

				}

				else if (Input.GetKeyDown(KeyCode.A) == true && attacking != true)
				{
					GMan.ap -= 1;
					playerAnimator.Play("stickcleave");
					attacking = true;
					actionStarted = true;
					yield return new WaitForSeconds(2f);
					BSM.HeroChoice.chooseanAttack = BSM.HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attacks[0]; // was 1
					doDamage();
					attacking = false;
					playerAnimator.Play("stickidle");//animate idle

				}

				else if (Input.GetKeyDown(KeyCode.D) == true && attacking != true)
				{
					GMan.ap -= 1;
					playerAnimator.Play("stickcleave");
					attacking = true;
					actionStarted = true;
					yield return new WaitForSeconds(2f);
					BSM.HeroChoice.chooseanAttack = BSM.HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attacks[0]; //was 2
					doDamage();
					attacking = false;
					playerAnimator.Play("stickidle");//animate idle

				}
			}*/
			//do while loop to await command and wait for ap to either = 0 or x to be hit as cancel

			//if attack and attacking != true, play animation associated with attack; attacking = true
			//after certain length of animation attacking = false
			//animate idle when retrned

			//animate run back to spot

			//actionStarted = true;
			//yield return new WaitForSeconds(0.5f);
			//doDamage();

			Vector3 firstPosition = startposition;
			//playerAnimator.Play("stickrunning");//animate run right
			//while (MoveTowardsStart(firstPosition)) { yield return null; }
			//remove this performer from the list in BSM because you don't want him to do action twice
			Debug.Log("Animating idle");
			//playerAnimator.enabled = false;//animateidle
			//playerAnimator.enabled = true;//animateidle
			if (!GMan.MariamHero)
				playerAnimator.Play("FightManRight_Idle");
			else
				playerAnimator.Play("Mariam_Idle");
			//playerAnimator.Play("stickfigureidle");//animateidle

			//BSM.PerformList.RemoveAt(0);
			//reset BSM -> WAIT
			/*if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
			{
				BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
				cur_cooldown = 0f;
				currentState = TurnState.PROCESSING;

			}
			//reset the battle state machine and set monster to wait
			BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
			//end coroutine
			actionStarted = false;
			//reset this enemy state
			cur_cooldown = 0f;
			currentState = TurnState.PROCESSING;*/
		}
	}



	private IEnumerator AttackDelayFlameJavelin()
	{
		battleAnim = true;
		Debug.Log("USING FLAME JAVELIN");
		BSM.infoPanel.SetActive(true);
		BSM.InfoText.text = "Flame Javelin";
        AudioManager.instance.PlaySound("FlameJavelin1", transform.position, 1);
        yield return new WaitForSeconds(0.40f); //was at 1.5, was at .75, was at .40
		StartCoroutine(waitForInfo());
		//BSM.infoPanel.SetActive(false);
		Debug.Log("animate cleave");
		playerAnimator.Play("FlameJave");

		//playerAnimator.Play(stickfigurecleave);
		attacking = true;
		BSM.HeroChoice.chooseanAttack = GameObject.Find("Hero 1").GetComponent<HeroStateMachine>().hero.attacks[0];


		actionStarted = true;

		actionStarted = true;
		attacking = true;
		if (attacking)
			Debug.Log("ATTACKING!");
		Debug.Log("AttackDelay");
		//stickcleave = GameObject.Find("Hero 1").GetComponent<Animation>().name;
		RuntimeAnimatorController ac = playerAnimator.runtimeAnimatorController;
		for (int i = 0; i < ac.animationClips.Length; i++)
		{
			Debug.Log(ac.animationClips[i].name);
			if (ac.animationClips[i].name == "FlameJave")
			{
				Debug.Log("stickcleave timetowait");
				battleAnim = true;
				yield return new WaitForSeconds(ac.animationClips[i].length);
				doDamage(); //switched places with button command in update
				if (BSM.PerformList[0].Type == "Hero")
				{
					//EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();//catch enemy statemachine
					for (int k = 0; k < BSM.EnemiesInBattle.Count; k++)//check if currently dead hero is in battle list
					{
						Debug.Log("Current number of enemies in battle: " + k + 1);
						ESM = GameObject.Find(BSM.PerformList[0].AttackersTarget.name).GetComponent<EnemyStateMachine>();
						//EnemyStateMachine tempEnemy = GameObject.Find(BSM.PerformList[0].AttackersTarget.name).GetComponent<EnemyStateMachine>().currentState;

						//if (tempEnemy.TurnState != DEAD)
						if (GameObject.Find(BSM.PerformList[0].AttackersTarget.name).GetComponent<EnemyStateMachine>().currentState != EnemyStateMachine.TurnState.DEAD)
						//if (BSM.PerformList[0].AttackersTarget == BSM.EnemiesInBattle[k])
						{
							Debug.Log("Attacked same person!");
							EnemyToAttack = BSM.PerformList[0].AttackersTarget;
							currentState = TurnState.ACTION;
							break;
						}
						else
						{
							Debug.Log("Early return");
							earlyReturn = true;
							StartCoroutine(ReturnToSpot());
							GMan.ap += 3;
							ap_update();
							BSM.ComboPanel.SetActive(false);
							{
								battleAnim = false;
								earlyReturn = false;
								yield break;
							};
							//reset BSM -> WAIT
							if (!dontattackme)
							{


								Debug.Log("Removing performlist!");

							}
							dontattackme = true;

						}
					}
				}

				attacking = false;
				if (!attacking)
					Debug.Log("NOT ATTACKING!");
				if (!GMan.MariamHero)
					playerAnimator.Play("FightManRight_Idle");
				else
					playerAnimator.Play("Mariam_Idle");
				//playerAnimator.Play("stickfigureidle");
			}


		}
		battleAnim = false;
		//if (actionStarted && GMan.ap != 0)
		//{
		//	yield break;
		//	}

		if (actionStarted && earlyReturn)
		{ earlyReturn = false;
			yield break;
		}



		if (GMan.ap == 0 || (Input.GetKeyDown(KeyCode.S) == true) && isCombo && !attacking && !earlyReturn)
		{
			dontattackme = true;
			Debug.Log("Perform list");
			//BSM.PerformList.RemoveAt(0);
			isCombo = false;
			StartCoroutine(ReturnToSpot());


			GMan.ap += 3;
			ap_update();
			BSM.ComboPanel.SetActive(false);
			{ yield break;
			};
			//reset BSM -> WAIT
			if (!dontattackme)
			{


				Debug.Log("Removing performlist!");

			}
			dontattackme = true;

			//reset BSM -> WAIT

		}
		earlyReturn = false;
		if (actionStarted)
		{ yield break;
		}
	}



	//backstab

	private IEnumerator AttackDelayBackStab()
	{
		battleAnim = true;
		Debug.Log("USING FLAME JAVELIN");
		BSM.infoPanel.SetActive(true);
		BSM.InfoText.text = "Backstab";
        AudioManager.instance.PlaySound("Backstab", transform.position, 1);
        yield return new WaitForSeconds(0.40f); //was at 1.5, was at .75, was at .40
		StartCoroutine(waitForInfo());
		//BSM.infoPanel.SetActive(false);
		Debug.Log("animate cleave");
		playerAnimator.Play("backst");

		//playerAnimator.Play(stickfigurecleave);
		attacking = true;
		BSM.HeroChoice.chooseanAttack = GameObject.Find("Hero 1").GetComponent<HeroStateMachine>().hero.attacks[0];


		actionStarted = true;

		actionStarted = true;
		attacking = true;
		if (attacking)
			Debug.Log("ATTACKING!");
		Debug.Log("AttackDelay");
		//stickcleave = GameObject.Find("Hero 1").GetComponent<Animation>().name;
		RuntimeAnimatorController ac = playerAnimator.runtimeAnimatorController;
		for (int i = 0; i < ac.animationClips.Length; i++)
		{
			Debug.Log(ac.animationClips[i].name);
			if (ac.animationClips[i].name == "backst")
			{
				Debug.Log("stickcleave timetowait");
				battleAnim = true;
				yield return new WaitForSeconds(ac.animationClips[i].length);
				doDamage(); //switched places with button command in update
				if (BSM.PerformList[0].Type == "Hero")
				{
					//EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();//catch enemy statemachine
					for (int k = 0; k < BSM.EnemiesInBattle.Count; k++)//check if currently dead hero is in battle list
					{
						Debug.Log("Current number of enemies in battle: " + k + 1);
						ESM = GameObject.Find(BSM.PerformList[0].AttackersTarget.name).GetComponent<EnemyStateMachine>();
						//EnemyStateMachine tempEnemy = GameObject.Find(BSM.PerformList[0].AttackersTarget.name).GetComponent<EnemyStateMachine>().currentState;

						//if (tempEnemy.TurnState != DEAD)
						if (GameObject.Find(BSM.PerformList[0].AttackersTarget.name).GetComponent<EnemyStateMachine>().currentState != EnemyStateMachine.TurnState.DEAD)
						//if (BSM.PerformList[0].AttackersTarget == BSM.EnemiesInBattle[k])
						{
							Debug.Log("Attacked same person!");
							EnemyToAttack = BSM.PerformList[0].AttackersTarget;
							currentState = TurnState.ACTION;
							break;
						}
						else
						{
							Debug.Log("Early return");
							earlyReturn = true;
							StartCoroutine(ReturnToSpot());
							GMan.ap += 3;
							ap_update();
							BSM.ComboPanel.SetActive(false);
							{
								battleAnim = false;
								earlyReturn = false;
								yield break;
							};
							//reset BSM -> WAIT
							if (!dontattackme)
							{


								Debug.Log("Removing performlist!");

							}
							dontattackme = true;

						}
					}
				}

				attacking = false;
				if (!attacking)
					Debug.Log("NOT ATTACKING!");
				if (!GMan.MariamHero)
					playerAnimator.Play("FightManRight_Idle");
				else
					playerAnimator.Play("Mariam_Idle");
				//playerAnimator.Play("stickfigureidle");
			}


		}
		battleAnim = false;
		//if (actionStarted && GMan.ap != 0)
		//{
		//	yield break;
		//	}

		if (actionStarted && earlyReturn)
		{
			earlyReturn = false;
			yield break;
		}



		if (GMan.ap == 0 || (Input.GetKeyDown(KeyCode.S) == true) && isCombo && !attacking && !earlyReturn)
		{
			dontattackme = true;
			Debug.Log("Perform list");
			//BSM.PerformList.RemoveAt(0);
			isCombo = false;
			StartCoroutine(ReturnToSpot());


			GMan.ap += 3;
			ap_update();
			BSM.ComboPanel.SetActive(false);
			{
				yield break;
			};
			//reset BSM -> WAIT
			if (!dontattackme)
			{


				Debug.Log("Removing performlist!");

			}
			dontattackme = true;

			//reset BSM -> WAIT

		}
		earlyReturn = false;
		if (actionStarted)
		{
			yield break;
		}
	}

	//end backstab
	private IEnumerator waitForInfo()
	{
		yield return new WaitForSeconds(.18f);
		BSM.infoPanel.SetActive(false);
	}





	private IEnumerator ReturnToSpot()
	{
		chainCount = 0;
		hitPanel.SetActive(false);
		foreach (GameObject box in firstNumberList)
		{
			Destroy(box);
		}
		foreach (GameObject box in secondNumberList)
		{
			Destroy(box);
		}
		firstNumber = 0;
		secondNumber = 0;
		firstNumberList.Clear();
		secondNumberList.Clear();

		int l = comboTracker.Count;
		for (int i = 0; i < l; i++)
			comboTracker.Remove(comboTracker[0]);

		for (int i = 0; i < flameJavelinTracker.Count; i++)
			flameJavelinBools[i] = false;
		comboNumber = 0;
		foreach (GameObject btn in comboBtns)
		{
			Destroy(btn);
		}

		comboBtns.Clear();
		if (dontattackme = false)
		{ yield break; }


		atPosition = false;
		if (!GMan.MariamHero)
			playerAnimator.Play("FightingManBackward");
		else
			playerAnimator.Play("Mariam_Move_Right");
		//playerAnimator.Play("stickrunning");//animate run right
		while (MoveTowardsStart(startposition))
		{
			running = true;
			yield return null; }
		isCombo = false;
		running = false;


		BSM.PerformList.RemoveAt(0);
		if (!GMan.MariamHero)
			playerAnimator.Play("FightManRight_Idle");
		else
			playerAnimator.Play("Mariam_Idle");
		//playerAnimator.Play("stickfigureidle");

		dontattackme = false;

		RuntimeAnimatorController ac = playerAnimator.runtimeAnimatorController;

		if (BSM.magicalAtk)
			for (int i = 0; i < ac.animationClips.Length; i++)
			{
				Debug.Log(ac.animationClips[i].name);
				if (ac.animationClips[i].name == "Mariam_CastSpell")
				{

					//battleAnim = true;
					yield return new WaitForSeconds(ac.animationClips[i].length);
				}
			}

		if (!BSM.magicalAtk)
		{
			if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
			{
				BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
				cur_cooldown = 0f;
				currentState = TurnState.PROCESSING;

			}
			//reset the battle state machine and set monster to wait

			//BSM.battleStates = BattleStateMachine.PerformAction.WAIT;

			//end coroutine
			actionStarted = false;
			//reset this enemy state
			//cur_cooldown = 0f;
			//currentState = TurnState.PROCESSING;
			//BSM.PerformList.RemoveAt(0);
		}
		//BSM.magicalAtk = false;

	}
	private bool MoveTowardsEnemy(Vector3 target)
	{
		return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
	}
	private bool MoveTowardsStart(Vector3 target)
	{
		return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
	}

	void hitFunction()
	{
		hitPanel.SetActive(true);
		if (comboNumber > 9)
		{ firstNumber = comboNumber / 10; }
		secondNumber = comboNumber % 10;
		/*if (firstNumber == 0)
		{
			//if (GameObject.Find("zeroFirst") != null)
			//Destroy(firstNumberBox);
			foreach (GameObject box in firstNumberList)
			{
				Destroy(box);
			}

			firstNumberList.Clear();
			GameObject firstNumberBox = Instantiate(zero) as GameObject;
			firstNumberList.Add(firstNumberBox);
			firstNumberBox.transform.SetParent(hitSpacer1, false); //deals with overflow}

		}*/
		if (firstNumber == 1)
		{
			//if (GameObject.Find("oneFirst") != null)
			//Destroy(firstNumberBox);
			foreach (GameObject box in firstNumberList)
			{
				Destroy(box);
			}
			firstNumberList.Clear();
			GameObject firstNumberBox = Instantiate(one) as GameObject;
			firstNumberList.Add(firstNumberBox);
			firstNumberBox.transform.SetParent(hitSpacer1, false); //deals with overflow}
		}
		else if (firstNumber == 2)
		{
			//if (GameObject.Find("twoFirst") != null)
			//Destroy(firstNumberBox);
			foreach (GameObject box in firstNumberList)
			{
				Destroy(box);
			}
			firstNumberList.Clear();
			GameObject firstNumberBox = Instantiate(two) as GameObject;
			firstNumberList.Add(firstNumberBox);
			firstNumberBox.transform.SetParent(hitSpacer1, false); //deals with overflow}
		}
		else if (firstNumber == 3)
		{
			//if (GameObject.Find("threeFirst") != null)
			//Destroy(firstNumberBox);
			foreach (GameObject box in firstNumberList)
			{
				Destroy(box);
			}
			firstNumberList.Clear();
			GameObject firstNumberBox = Instantiate(three) as GameObject;
			firstNumberList.Add(firstNumberBox);
			firstNumberBox.transform.SetParent(hitSpacer1, false); //deals with overflow}
		}
		else if (firstNumber == 4)
		{
			//if (GameObject.Find("fourFirst") != null)
			//Destroy(firstNumberBox);
			foreach (GameObject box in firstNumberList)
			{
				Destroy(box);
			}
			firstNumberList.Clear();
			GameObject firstNumberBox = Instantiate(four) as GameObject;
			firstNumberList.Add(firstNumberBox);
			firstNumberBox.transform.SetParent(hitSpacer1, false); //deals with overflow}
		}
		else if (firstNumber == 5)
		{
			//if (GameObject.Find("fiveFirst") != null)
			//Destroy(firstNumberBox);
			foreach (GameObject box in firstNumberList)
			{
				Destroy(box);
			}
			firstNumberList.Clear();
			GameObject firstNumberBox = Instantiate(five) as GameObject;
			firstNumberList.Add(firstNumberBox);
			firstNumberBox.transform.SetParent(hitSpacer1, false); //deals with overflow}
		}
		else if (firstNumber == 6)
		{
			//if (GameObject.Find("sixFirst") != null)
			//Destroy(firstNumberBox);
			foreach (GameObject box in firstNumberList)
			{
				Destroy(box);
			}
			firstNumberList.Clear();
			GameObject firstNumberBox = Instantiate(six) as GameObject;
			firstNumberList.Add(firstNumberBox);
			firstNumberBox.transform.SetParent(hitSpacer1, false); //deals with overflow}
		}
		else if (firstNumber == 7)
		{
			//if (GameObject.Find("sevenFirst") != null)
			//Destroy(firstNumberBox);
			foreach (GameObject box in firstNumberList)
			{
				Destroy(box);
			}
			firstNumberList.Clear();
			GameObject firstNumberBox = Instantiate(seven) as GameObject;
			firstNumberList.Add(firstNumberBox);
			firstNumberBox.transform.SetParent(hitSpacer1, false); //deals with overflow}
		}
		else if (firstNumber == 8)
		{
			//if (GameObject.Find("eightFirst") != null)
			//Destroy(firstNumberBox);
			foreach (GameObject box in firstNumberList)
			{
				Destroy(box);
			}
			firstNumberList.Clear();
			GameObject firstNumberBox = Instantiate(eight) as GameObject;
			firstNumberList.Add(firstNumberBox);
			firstNumberBox.transform.SetParent(hitSpacer1, false); //deals with overflow}
		}
		else if (firstNumber == 9)
		{
			//if (GameObject.Find("nineFirst") != null)
			//Destroy(firstNumberBox);
			foreach (GameObject box in firstNumberList)
			{
				Destroy(box);
			}
			firstNumberList.Clear();
			GameObject firstNumberBox = Instantiate(nine) as GameObject;
			firstNumberList.Add(firstNumberBox);
			firstNumberBox.transform.SetParent(hitSpacer1, false); //deals with overflow}
		}
		if (comboNumber > 9 && secondNumber == 0)
		{
			//if (GameObject.Find("zeroSecond") != null)
			//Destroy(secondNumberBox);
			foreach (GameObject box in secondNumberList)
			{
				Destroy(box);
			}
			secondNumberList.Clear();
			GameObject secondNumberBox = Instantiate(zero) as GameObject;
			secondNumberList.Add(secondNumberBox);
			secondNumberBox.transform.SetParent(hitSpacer2, false); //deals with overflow}
		}
		else if (secondNumber == 1)
		{
			//if (GameObject.Find("oneSecond") != null)
			//Destroy(secondNumberBox);
			foreach (GameObject box in secondNumberList)
			{
				Destroy(box);
			}
			secondNumberList.Clear();
			GameObject secondNumberBox = Instantiate(one) as GameObject;
			secondNumberList.Add(secondNumberBox);
			secondNumberBox.transform.SetParent(hitSpacer2, false); //deals with overflow}
		}
		else if (secondNumber == 2)
		{
			//if (GameObject.Find("twoSecond") != null)

			foreach (GameObject box in secondNumberList)
			{
				Destroy(box);
			}
			secondNumberList.Clear();
			GameObject secondNumberBox = Instantiate(two) as GameObject;
			secondNumberList.Add(secondNumberBox);
			secondNumberBox.transform.SetParent(hitSpacer2, false); //deals with overflow}
		}
		else if (secondNumber == 3)
		{
			//if (GameObject.Find("threeSecond") != null)
			//Destroy(secondNumberBox);
			foreach (GameObject box in secondNumberList)
			{
				Destroy(box);
			}
			secondNumberList.Clear();
			GameObject secondNumberBox = Instantiate(three) as GameObject;
			secondNumberList.Add(secondNumberBox);
			secondNumberBox.transform.SetParent(hitSpacer2, false); //deals with overflow}
		}
		else if (secondNumber == 4)
		{
			//if (GameObject.Find("fourSecond") != null)
			//Destroy(secondNumberBox);
			foreach (GameObject box in secondNumberList)
			{
				Destroy(box);
			}
			secondNumberList.Clear();
			GameObject secondNumberBox = Instantiate(four) as GameObject;
			secondNumberList.Add(secondNumberBox);
			secondNumberBox.transform.SetParent(hitSpacer2, false); //deals with overflow}
		}
		else if (secondNumber == 5)
		{
			//if (GameObject.Find("fiveSecond") != null)
			//Destroy(secondNumberBox);
			foreach (GameObject box in secondNumberList)
			{
				Destroy(box);
			}
			secondNumberList.Clear();
			GameObject secondNumberBox = Instantiate(five) as GameObject;
			secondNumberList.Add(secondNumberBox);
			secondNumberBox.transform.SetParent(hitSpacer2, false); //deals with overflow}
		}
		else if (secondNumber == 6)
		{
			//if (GameObject.Find("sixSecond") != null)
			//Destroy(secondNumberBox);
			foreach (GameObject box in secondNumberList)
			{
				Destroy(box);
			}
			secondNumberList.Clear();
			GameObject secondNumberBox = Instantiate(six) as GameObject;
			secondNumberList.Add(secondNumberBox);
			secondNumberBox.transform.SetParent(hitSpacer2, false); //deals with overflow}
		}
		else if (secondNumber == 7)
		{
			//if (GameObject.Find("sevenSecond") != null)
			//Destroy(secondNumberBox);
			foreach (GameObject box in secondNumberList)
			{
				Destroy(box);
			}
			secondNumberList.Clear();
			GameObject secondNumberBox = Instantiate(seven) as GameObject;
			secondNumberList.Add(secondNumberBox);
			secondNumberBox.transform.SetParent(hitSpacer2, false); //deals with overflow}
		}
		else if (secondNumber == 8)
		{
			//if (GameObject.Find("eightSecond") != null)
			//Destroy(secondNumberBox);
			foreach (GameObject box in secondNumberList)
			{
				Destroy(box);
			}
			secondNumberList.Clear();
			GameObject secondNumberBox = Instantiate(eight) as GameObject;
			secondNumberList.Add(secondNumberBox);
			secondNumberBox.transform.SetParent(hitSpacer2, false); //deals with overflow}
		}
		else if (secondNumber == 9)
		{
			//if (GameObject.Find("nineSecond") != null)
			//Destroy(secondNumberBox);
			foreach (GameObject box in secondNumberList)
			{
				Destroy(box);
			}
			secondNumberList.Clear();
			GameObject secondNumberBox = Instantiate(nine) as GameObject;
			secondNumberList.Add(secondNumberBox);
			secondNumberBox.transform.SetParent(hitSpacer2, false); //deals with overflow}
		}
	}
	private IEnumerator chainFunction()
	{Rigidbody2D rigidbod2;
	GameObject rigidbody2;
	chainCount += 1;
		GameObject CHAINText = Instantiate(chaintext, transform.position, Quaternion.identity) as GameObject;
		CHAINText.transform.SetParent(BattleCanvas, false);
		rigidbody2 = CHAINText;
		chainBtns.Add(CHAINText);
		rigidbod2 = CHAINText.GetComponent<Rigidbody2D>();
		CHAINTEXT = CHAINText.GetComponent<Text>();
		CHAINTEXT.text = "Chain x " + chainCount;

		rigidbod2.gravityScale = -4.5f; //-1.5f
		//rigidbod.drag = 8f;
		
		rigidbod2.angularDrag = 2f;
		CHAINTEXT.color = Color.white;
		//yield return new WaitForSeconds(2.5f);
		CHAINText.transform.Rotate(Vector3.up * 50); //10000
		rigidbod2.angularDrag = 2f;
		yield return new WaitForSeconds(.1f);
		rigidbody2.transform.Rotate(Vector3.up * 50); //10000
		yield return new WaitForSeconds(.2f);
		rigidbody2.transform.Rotate(Vector3.up * 50); //10000
		yield return new WaitForSeconds(.1f);
		rigidbody2.transform.Rotate(Vector3.up * 50); //10000
		CHAINTEXT.color = Color.blue;
		yield return new WaitForSeconds(.2f);
		yield return new WaitForSeconds(.2f);
		rigidbody2.transform.Rotate(Vector3.down * 50); //10000
		yield return new WaitForSeconds(.2f);
		rigidbody2.transform.Rotate(Vector3.down * 50); //10000
		yield return new WaitForSeconds(.1f);
		rigidbody2.transform.Rotate(Vector3.down * 50); //10000
		yield return new WaitForSeconds(.2f);
		rigidbody2.transform.Rotate(Vector3.down * 50); //10000
		rigidbod2.gravityScale = 1f;
		CHAINTEXT.color = Color.yellow;
		yield return new WaitForSeconds(.8f);
		CHAINTEXT.color = Color.white;
		//yield return new WaitForSeconds(2.5f);
		CHAINText.transform.Rotate(Vector3.up * 50); //10000
		rigidbod2.angularDrag = 2f;
		yield return new WaitForSeconds(.1f);
		rigidbody2.transform.Rotate(Vector3.up * 50); //10000
		yield return new WaitForSeconds(.2f);
		rigidbody2.transform.Rotate(Vector3.up * 50); //10000
		yield return new WaitForSeconds(.1f);
		rigidbody2.transform.Rotate(Vector3.up * 50); //10000
		CHAINTEXT.color = Color.blue;
		yield return new WaitForSeconds(.2f);
		yield return new WaitForSeconds(.2f);
		rigidbody2.transform.Rotate(Vector3.down * 50); //10000
		yield return new WaitForSeconds(.2f);
		rigidbody2.transform.Rotate(Vector3.down * 50); //10000
		yield return new WaitForSeconds(.1f);
		rigidbody2.transform.Rotate(Vector3.down * 50); //10000
		yield return new WaitForSeconds(.2f);
		rigidbody2.transform.Rotate(Vector3.down * 50); //10000
		rigidbod2.gravityScale = 1f;
		CHAINTEXT.color = Color.yellow;
		yield return new WaitForSeconds(.8f);
		CHAINTEXT.color = Color.white;
		//yield return new WaitForSeconds(2.5f);
		CHAINText.transform.Rotate(Vector3.up * 50); //10000
		rigidbod2.angularDrag = 2f;
		yield return new WaitForSeconds(.1f);
		rigidbody2.transform.Rotate(Vector3.up * 50); //10000
		yield return new WaitForSeconds(.2f);
		rigidbody2.transform.Rotate(Vector3.up * 50); //10000
		yield return new WaitForSeconds(.1f);
		rigidbody2.transform.Rotate(Vector3.up * 50); //10000
		CHAINTEXT.color = Color.blue;
		yield return new WaitForSeconds(.2f);
		yield return new WaitForSeconds(.2f);
		rigidbody2.transform.Rotate(Vector3.down * 50); //10000
		yield return new WaitForSeconds(.2f);
		rigidbody2.transform.Rotate(Vector3.down * 50); //10000
		yield return new WaitForSeconds(.1f);
		rigidbody2.transform.Rotate(Vector3.down * 50); //10000
		yield return new WaitForSeconds(.2f);
		rigidbody2.transform.Rotate(Vector3.down * 50); //10000
		rigidbod2.gravityScale = 1f;
		CHAINTEXT.color = Color.yellow;
		yield return new WaitForSeconds(.8f);
		Destroy(CHAINText);

	}
	public IEnumerator waitForDamage()
	{
		GameObject DMGText = Instantiate(BSM.dmgText, transform.position, Quaternion.identity) as GameObject;
		//DMGText.transform.position = new Vector3(1, 1, 1);
		DMGText.transform.SetParent(BSM.BattleCanvas, false);
		rigidbody = DMGText;
		dmgBtns.Add(DMGText);
		rigidbod = DMGText.GetComponent<Rigidbody2D>();
		DMGTEXT = DMGText.GetComponent<Text>();
		//Text dmg_text = DMGText.transform.Find("Text").gameObject.GetComponent<Text>();
		//dmg_text.text = damage;
		DMGTEXT.text = "" + damage;
		//DMGText.transform.position = new Vector3(1, 1, 1);

		//ESM = performer.GetComponent<EnemyStateMachine>();//catch enemy statemachine
		//EnemyStateMachine tempenemy = GameObject.Find(BSM.PerformList[0].AttackersTarget.name)
		//ESM = GameObject.Find(BSM.PerformList[0].AttackersGameObject.name).GetComponent<EnemyStateMachine>();

		/*if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
		{
			//argument is out of range, and that's why it's not working
			//BSM.PerformList.RemoveAt(0);

			BSM.battleStates = BattleStateMachine.PerformAction.WAIT; //this isnt working
			ESM.cur_cooldown = 0f;
			ESM.currentState = EnemyStateMachine.TurnState.PROCESSING;

			ESM.actionStarted = false;
		}*/

		rigidbod.gravityScale = -1.5f; //-2
		rigidbod.drag = 8f;
		DMGText.transform.Rotate(Vector3.right * 50); //10000
		rigidbod.angularDrag = 2f;
		yield return new WaitForSeconds(.1f);
		rigidbody.transform.Rotate(Vector3.right * 50); //10000
		yield return new WaitForSeconds(.1f);
		rigidbody.transform.Rotate(Vector3.right * 50); //10000
		yield return new WaitForSeconds(.1f);
		rigidbody.transform.Rotate(Vector3.right * 50); //10000
		yield return new WaitForSeconds(.1f);
		rigidbody.transform.Rotate(Vector3.left * 200); //10000
		rigidbod.gravityScale = 1f;
		DMGTEXT.color = Color.white;
		yield return new WaitForSeconds(.3f);
		Destroy(DMGText);
	}

		public void TakeDamage(float getDamageAmount)
	{
		hero.curHP -= getDamageAmount;//do damage to hero
		damage = getDamageAmount;
		if (hero.curHP <= 0)
			
		{
			hero.curHP = 0; //if hp less than 0 set to 0 so no negative numbers
			currentState = TurnState.DEAD;
			this.gameObject.GetComponent<SpriteRenderer>().enabled = false;

            AudioManager.instance.PlaySound("Dead", transform.position, 1);
            Instantiate(deathAnim, transform.position, deathAnim.rotation);
			deathAnimTimer = true;
		}StartCoroutine(waitForDamage());
		if (!GMan.MariamHero)
		playerAnimator.Play("FightManRight_Hit");
		else
			playerAnimator.Play("Mariam_GetHit");


		StartCoroutine(damageAnimWait());
		
				
	
		UpdateHeroPanel();

	}
private IEnumerator damageAnimWait()
		{
		RuntimeAnimatorController animc = playerAnimator.runtimeAnimatorController;
		if (!GMan.MariamHero)
		for (int i = 0; i < animc.animationClips.Length; i++)
		{
			if (animc.animationClips[i].name == "FightManRight_Hit")
			{

				yield return new WaitForSeconds(animc.animationClips[i].length);
				playerAnimator.Play("FightManRight_Idle");
			}
		}
		else
			for (int i = 0; i < animc.animationClips.Length; i++)
			{
				if (animc.animationClips[i].name == "Mariam_GetHit")
				{

					yield return new WaitForSeconds(animc.animationClips[i].length);
					playerAnimator.Play("Mariam_Idle");
				}
			}
	}
	void doDamage()
	{
		float calc_damage = hero.curATK + BSM.PerformList[0].chooseanAttack.attackDamage;
		EnemyToAttack.GetComponent<EnemyStateMachine>().TakeDamage(calc_damage);
	}

	void CreateHeroPanel()//creates a new panel for whichever hero is dead
	{
		HeroPanel = Instantiate(HeroPanel) as GameObject;
		stats = HeroPanel.GetComponent<HeroPanelStats>();
		stats.HeroName.text = hero.theName;
		stats.HeroHP.text = "HP: " + hero.curHP;//HP: 456
		stats.HeroMP.text = "MP: " + hero.curMP;

		ProgressBar = stats.ProgressBar;
		HeroPanel.transform.SetParent(HeroPanelSpacer, false); //false takes care of the scalin

	}

	//update stats on damage / heal
	void UpdateHeroPanel()
	{
		stats.HeroHP.text = "HP: " + hero.curHP;
		//when taking damage get reduced get damage amount, and when dead we update hero panel in take damage script
	}

	void ap_update()
	{
		AP_Text.text = "" + GMan.ap;
	}
}

/*

   using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour {
   public BaseHero hero;
   private BattleStateMachine BSM;


   public enum TurnState
   {
	   PROCESSING, //bar fills
	   ADDTOLIST, //add hero to a list
	   WAITING, //idle state
	   SELECTING, //player selects action which we create later on
	   ACTION, //can do an action
	   DEAD
   }

   public TurnState currentState;
   //for the progress bar

   private float cur_cooldown = 0f;
   private float max_cooldown = 5f;
   public Image ProgressBar;
   public GameObject Selector;

   public GameObject EnemyToAttack;
   private bool actionStarted = false;
   private Vector3 startposition;
   private float animSpeed = 10f;
   private bool alive = true;
   //hero Panel
   private HeroPanelStats stats;
   public GameObject HeroPanel;
   private Transform HeroPanelSpacer;
   // Use this for initialization
   void Start () {
		   HeroPanelSpacer = GameObject.Find("BattleCanvas").transform.Find("HeroPanel").transform.Find("HeroPanelSpacer");
		   CreateHeroPanel();
	   //create panel, fill in info 


	   //find spacer
	   startposition = transform.position;
	   cur_cooldown = Random.Range(0, 2.5f);
	   Selector.SetActive(false);
	   BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>(); //connects enemystatemachine to battlestatemachine
	   currentState = TurnState.PROCESSING;
   }

   // Update is called once per frame
   void Update () {

	   //Debug.Log(currentState);

	   switch (currentState) {

		   case (TurnState.PROCESSING):
			   UpdateProgressBar();
			   break;

		   case (TurnState.ADDTOLIST):
			   BSM.HeroesToManage.Add(this.gameObject);
			   currentState = TurnState.WAITING;
			   break;

		   case (TurnState.WAITING):
			   //idle
			   break;

		   case (TurnState.SELECTING):

			   break;

		   case (TurnState.ACTION):
			   StartCoroutine(TimeForAction());
			   break;

		   case (TurnState.DEAD):
			   if (!alive)
			   {
				   return;
			   }
			   else 
			   {
				   this.gameObject.tag = "DeadHero";
				   //change tag

				   //change attackable by enemy 

				   BSM.HeroesInBattle.Remove(this.gameObject);

				   //not able to manage hero anymore

				   BSM.HeroesToManage.Remove(this.gameObject);

				   //deactive the selector if it's on

				   Selector.SetActive(false);

				   //reset gui (get rid of attack / select panel)

				   BSM.AttackPanel.SetActive(false);
				   BSM.EnemySelectPanel.SetActive(false);
				   //remove item from performlist

				   for (int i = 0; i < BSM.PerformList.Count; i++)
				   {
					   if (BSM.PerformList[i].AttackersGameObject == this.gameObject) //if we are current attacker
					   {
						   BSM.PerformList.Remove(BSM.PerformList[i]);

					   }
				   }
				   //change color / play animation

				   this.gameObject.GetComponent<SpriteRenderer>().material.color = new Color32(105, 105, 105, 255);

				   BSM.Heroinput = BattleStateMachine.HeroGUI.ACTIVATE; //if other hero is alive.
				   alive = false;

				   //reset the heroinput


				   alive = false;
			   }
			   break;


	   }


   }

   void UpdateProgressBar()
   {

	   cur_cooldown = cur_cooldown + Time.deltaTime;
	   float calc_cooldown = cur_cooldown / max_cooldown;
	   ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calc_cooldown, 0, 1), ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
	   if (cur_cooldown >= max_cooldown)
	   {
		   currentState = TurnState.ADDTOLIST;
	   }
   }

   private IEnumerator TimeForAction()
   {
	   if (actionStarted)
	   {
		   yield break;
	   }
	   actionStarted = true;

	   //animate enemy near the hero to attack

	   Vector3 heroPosition = new Vector3(EnemyToAttack.transform.position.x + 1.5f, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z);//increasing target's x position by 1.5f units to give space to attack
	   while (MoveTowardsEnemy(heroPosition))
	   {
		   yield return null;
	   }
	   //wait a bit, do damage, and then animate the back to start position.
	   actionStarted = true;
	   yield return new WaitForSeconds(0.5f);
	   doDamage();

	   Vector3 firstPosition = startposition;
	   while (MoveTowardsStart(firstPosition)) { yield return null; }
	   //remove this performer from the list in BSM because you don't want him to do action twice
	   BSM.PerformList.RemoveAt(0);
	   //reset the battle state machine and set monster to wait
	   BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
	   //end coroutine
	   actionStarted = false;
	   //reset this enemy state
	   cur_cooldown = 0f;
	   currentState = TurnState.PROCESSING;
   }

   private bool MoveTowardsEnemy(Vector3 target)
   {
	   return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
   }
   private bool MoveTowardsStart(Vector3 target)
   {
	   return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
   }

   public void TakeDamage(float getDamageAmount)
   {
	   hero.curHP -= getDamageAmount;//do damage to hero
	   if (hero.curHP <= 0)
	   {
		   hero.curHP = 0; //if hp less than 0 set to 0 so no negative numbers
		   currentState = TurnState.DEAD;
	   }
	   UpdateHeroPanel();

   }

   void doDamage()
   {
	   float calc_damage = hero.curATK + BSM.PerformList[0].chooseanAttack.attackDamage;
	   EnemyToAttack.GetComponent <EnemyStateMachine>().TakeDamage(calc_damage);
   }

   void CreateHeroPanel()//creates a new panel for whichever hero is dead
	   {
		   HeroPanel = Instantiate(HeroPanel) as GameObject;
		   stats = HeroPanel.GetComponent<HeroPanelStats>();
		   stats.HeroName.text = hero.theName;
		   stats.HeroHP.text = "HP: " + hero.curHP;//HP: 456
		   stats.HeroMP.text = "MP: " + hero.curMP;

		   ProgressBar = stats.ProgressBar;
		   HeroPanel.transform.SetParent(HeroPanelSpacer, false); //false takes care of the scalin

	   }

   //update stats on damage / heal
   void UpdateHeroPanel()
   {
	   stats.HeroHP.text = "HP: " + hero.curHP;
	   //when taking damage get reduced get damage amount, and when dead we update hero panel in take damage script
   }
}
*/
