using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStateMachine : MonoBehaviour {

	public LightningAnim LA;
	public Animator playerAnimator;
	private GameManager GMan;
	public GameObject GMana;
	public List<GameObject> dmgBtns = new List<GameObject>();
	private BattleStateMachine BSM;
	public BaseEnemy enemy;
	public Transform deathAnim;
	public bool deathAnimTimer;
	public Rigidbody2D rigidbod;
	public GameObject rigidbody;
	public Text DMGTEXT;
	private HeroStateMachine HSM;
	public enemyencounterspawner EES;
	public GameObject HSMObject;
	public float damage;
	public int num;

	public enum TurnState
	{
		PROCESSING, //bar fills
		CHOOSEACTION, //add hero to a list
		WAITING, //idle state
		ACTION, //can do an action
		DEAD
	}

	public TurnState currentState;
	//for the progress bar

	public float cur_cooldown = 0f;
	public float max_cooldown = 10f;

	//this gameobject
	private Vector3 startposition;
	public GameObject Selector;

	//timeforaction stuff
	public bool actionStarted = false;
	public GameObject HeroToAttack;
	private float animSpeed = 10f;

	//alive
	private bool alive = true;
	
	// Use this for initialization
	void Start()
	{
		EES = GameObject.Find("BeastSpawner").GetComponent<enemyencounterspawner>();
		LA = GameObject.Find("lightning true 1").GetComponent<LightningAnim>();
		GMana = GameObject.Find("GameManager");
		
		GMan = GMana.GetComponent<GameManager>();
		playerAnimator = this.gameObject.GetComponent<Animator>();
		HSMObject = GameObject.Find("Hero 1");
		HSM = HSMObject.GetComponent<HeroStateMachine>();
		deathAnimTimer = false; 
		Selector.SetActive(false);
		currentState = TurnState.PROCESSING;
		BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>(); //connects enemystatemachine to battlestatemachine
		startposition = transform.position; //so we can move from left to right or right to left when attacking with animations
	}

	// Update is called once per frame
	void Update()
	{

		//Debug.Log(currentState);

		switch (currentState)
		{

			case (TurnState.PROCESSING):
				UpdateProgressBar();
				if (BSM.battleStates == BattleStateMachine.PerformAction.WIN || BSM.battleStates == BattleStateMachine.PerformAction.LOSE)
				{
					BSM.battleStates = BattleStateMachine.PerformAction.CHECKALIVE;
					currentState = TurnState.WAITING;
				}
				break;

			case (TurnState.CHOOSEACTION):
                Selector.SetActive(true);
                ChooseAction();
				currentState = TurnState.WAITING;
				break;

			case (TurnState.WAITING):
				//idle state
				break;

			case (TurnState.ACTION):
                Selector.SetActive(false);
                StartCoroutine(TimeForAction());
				break;

			case (TurnState.DEAD):
				if (!alive)
				{
					return;
				}
				else
				{
					//change tag of enemy
					this.gameObject.tag = "DeadEnemy";
					//not attackable by heroes
					BSM.EnemiesInBattle.Remove(this.gameObject);
					//disable the selector
					Selector.SetActive(false);
					//remove all inputs heroattacks
					if (BSM.EnemiesInBattle.Count > 0)
					{
						for (int i = 0; i < BSM.PerformList.Count; i++)
						{
							if (i != 0)
							{
								if (BSM.PerformList[i].AttackersGameObject == this.gameObject)
								{
									BSM.PerformList.Remove(BSM.PerformList[i]);
								}

								else if (BSM.PerformList[i].AttackersTarget == this.gameObject)//choose different target if target is dead
								{
									BSM.PerformList[i].AttackersTarget = BSM.EnemiesInBattle[Random.Range(0, BSM.EnemiesInBattle.Count)];
								}
							}
						}
					}
					//change the color to gray / play dead animation
					this.gameObject.GetComponent<SpriteRenderer>().material.color = new Color32(105, 105, 105, 255);
					//set alive false
					alive = false;
					BSM.EnemyButtons();
					//check alive
					BSM.battleStates = BattleStateMachine.PerformAction.CHECKALIVE;
				}

				break;


		}


	}

	void UpdateProgressBar()
	{

		cur_cooldown = cur_cooldown + Time.deltaTime;
		
		if (cur_cooldown >= max_cooldown)
		{
			currentState = TurnState.CHOOSEACTION;
		}
	}

	void ChooseAction()//populate turn with actions that the enemy is going to choose automatically
	{
        
		HandleTurns myAttack = new HandleTurns(); //new instance of HandleTurns is what we populate chooseattack with. We can send any information in handleturns here through myattack
		myAttack.Attacker = enemy.theName; //storing the enemy's name into myattack
		myAttack.Type = "Enemy";
		myAttack.AttackersGameObject = this.gameObject;
		myAttack.AttackersTarget = BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)];

		if (GMan.MariamHero)
		{
			num = Random.Range(0, 2);//enemy does a random attack
			if (EES.boss)
			{
				myAttack.chooseanAttack = enemy.MagicAttacks[0];
				BSM.CollectActions(myAttack);
			}
			else
			{
				if (num == 0)
				{
					myAttack.chooseanAttack = enemy.attacks[0];
					Debug.Log(this.gameObject.name + " has chosen " + myAttack.chooseanAttack.attackName + " and does " + myAttack.chooseanAttack.attackDamage + " damage");
					BSM.CollectActions(myAttack);
				}
				if (num == 1)
				{
					myAttack.chooseanAttack = enemy.MagicAttacks[0];
					BSM.CollectActions(myAttack);
				}
			}
		}
		else
		{
			myAttack.chooseanAttack = enemy.attacks[0];
			Debug.Log(this.gameObject.name + " has chosen " + myAttack.chooseanAttack.attackName + " and does " + myAttack.chooseanAttack.attackDamage + " damage");
			BSM.CollectActions(myAttack);
		}

		/*int num = Random.Range(0, enemy.attacks.Count);//enemy does a random attack
		myAttack.chooseanAttack = enemy.attacks[num];
		Debug.Log(this.gameObject.name + " has chosen " + myAttack.chooseanAttack.attackName + " and does " + myAttack.chooseanAttack.attackDamage + " damage");
		BSM.CollectActions(myAttack);*/
	}

	private IEnumerator TimeForAction()
	{ HSM.battleAnim = true;
		if (EES.boss)
			num = 1;
		//num = 1;
		if (num == 1)
		{
			//BSM.magicalAtk = false;
			if (actionStarted)
			{
				yield break;
			}
			actionStarted = true;

			//animate enemy near the hero to attack

			// run to enemy

			Vector3 magicStart = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);

			// EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z
			Vector3 magicEnd = new Vector3(this.gameObject.transform.position.x + 1.5f, this.gameObject.transform.position.y, this.gameObject.transform.position.z);//increasing target's x position by 1.5f units to give space to attack

			//playerAnimator.Play("stickrunleft");
			//running = true;
			//MoveTowardsEnemy(heroPosition);
			//RuntimeAnimatorController ac = playerAnimator.runtimeAnimatorController;
			if (!EES.boss)
			{
				while (MoveTowardsEnemy(magicEnd))
				{
					playerAnimator.Play("Beast_Moving");
                  
					//playerAnimator.Play("stickrunleft");
					//running = true;
					yield return null;
				}
			}
			if (!EES.boss)
				playerAnimator.Play("Beast_CastSpell");
			else
				playerAnimator.Play("Beast2_CastSpell");
			RuntimeAnimatorController ac = playerAnimator.runtimeAnimatorController;

			for (int i = 0; i < ac.animationClips.Length; i++)
			{
				Debug.Log(ac.animationClips[i].name);
				if (!EES.boss)
				{
					if (ac.animationClips[i].name == "Beast_CastSpell")
					{
                        AudioManager.instance.PlaySound("enchant", transform.position, 1);
                        //battleAnim = true;
                        yield return new WaitForSeconds(ac.animationClips[i].length);
					}
				}
				else
				{
					if (ac.animationClips[i].name == "Beast2_CastSpell")
					{
                        AudioManager.instance.PlaySound("enchant", transform.position, 1);
                        //battleAnim = true;
                        yield return new WaitForSeconds(ac.animationClips[i].length);


					}
				}
			}
			//StartCoroutine(ReturnToSpot());
			for (int i = 0; i < ac.animationClips.Length; i++)
			{
				Debug.Log(ac.animationClips[i].name);
				if (!EES.boss)
				{ if (ac.animationClips[i].name == "Beast_CastSpell")
					{
                        AudioManager.instance.PlaySound("enchant", transform.position, 1);
                        //battleAnim = true;
                        yield return new WaitForSeconds(ac.animationClips[i].length);
					}
				}
				else
				{ if (ac.animationClips[i].name == "Beast2_CastSpell")
					{
                        AudioManager.instance.PlaySound("enchant", transform.position, 1);
                        //battleAnim = true;
                        yield return new WaitForSeconds(ac.animationClips[i].length);


					}
				}
			}
		//	DoDamage();


			//BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
			//end coroutine
			//reset this enemy state
			//cur_cooldown = 0f;
			//currentState = TurnState.PROCESSING;
			if (!EES.boss)
			{
				playerAnimator.Play("BeastMoveLeft");
				Vector3 firstPosition = startposition;

				while (MoveTowardsStart(firstPosition))
				{
					yield return null;
				}
			}
			if (EES.boss)
				playerAnimator.Play("Beast2_Idle");
			else
				playerAnimator.Play("Beast_Idle");

			//magicAnim = true;
			Debug.Log("AT MAGICAL ATTACK");

                AudioManager.instance.PlaySound("magicalEnergy", transform.position, 1);
				StartCoroutine(LA.lightningAnimation());
				

				/*playerAnimator.Play("CureTest1");
				if (spellName == "Fire 1")
				for (int i = 0; i < ac.animationClips.Length; i++)
				{
					Debug.Log(ac.animationClips[i].name);
					if (ac.animationClips[i].name == "CureTest1")
					{

						battleAnim = true;
						yield return new WaitForSeconds(ac.animationClips[i].length);
					}
				}*/
			


			/*BSM.PerformList.RemoveAt(0);
				if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
		{
			BSM.battleStates = BattleStateMachine.PerformAction.WAIT; //this isnt working
			cur_cooldown = 0f;
			currentState = TurnState.PROCESSING;

			actionStarted = false;
		}
			else
			{ BSM.battleStates = BattleStateMachine.PerformAction.CHECKALIVE;
				currentState = TurnState.WAITING;
			}
			playerAnimator.Play("Beast_Idle");*/


			for (int i = 0; i < ac.animationClips.Length; i++)
			{
				Debug.Log(ac.animationClips[i].name);
					if (!EES.boss)
					{
						if (ac.animationClips[i].name == "Beast_CastSpell")
						{

							//battleAnim = true;
							yield return new WaitForSeconds(ac.animationClips[i].length);
						}
					}
					else
					{
						if (ac.animationClips[i].name == "Beast2_CastSpell")
						{

							//battleAnim = true;
							yield return new WaitForSeconds(ac.animationClips[i].length);


						}
					}
				}
			//running = false;

			//atPosition = true;




			//if (!GMan.MariamHero)
			//playerAnimator.Play("FightManRight_Idle");
			//else
			if (EES.boss)
				playerAnimator.Play("Beast2_Idle");
			else
			playerAnimator.Play("Beast_Idle");

		}

		else
		{
			if (actionStarted)
			{
				yield break;
			}
			actionStarted = true;

			//animate enemy near the hero to attack
			if (!GMan.MariamHero)
				playerAnimator.Play("FightManLeft_Forward");
			else
				playerAnimator.Play("Beast_Moving");
			Vector3 heroPosition = new Vector3(HeroToAttack.transform.position.x - 1.5f, HeroToAttack.transform.position.y, HeroToAttack.transform.position.z);//increasing target's x position by 1.5f units to give space to attack
			while (MoveTowardsEnemy(heroPosition))
			{
				yield return null;
			}
			//wait a bit, do damage, and then animate the back to start position.
			actionStarted = true;

            if (!GMan.MariamHero)
            {
                AudioManager.instance.PlaySound("punch", transform.position, 1);
                playerAnimator.Play("FightManLeft_Punch");
            
            }
                
			else
				;
			//playerAnimator.Play("FightManLeft_Punch");

			RuntimeAnimatorController ac = playerAnimator.runtimeAnimatorController;
			if (!GMan.MariamHero)
			{

				for (int i = 0; i < ac.animationClips.Length; i++)
				{
					Debug.Log(ac.animationClips[i].name);
					if (ac.animationClips[i].name == "FightManLeft_Punch")
					{
						yield return new WaitForSeconds(ac.animationClips[i].length);
						DoDamage(); //switched places with button command in update
					}
				}
			}
			else
			{ DoDamage(); }
			Vector3 firstPosition = startposition;
			if (!GMan.MariamHero)
				playerAnimator.Play("FightManLeftBackward");
			else
				playerAnimator.Play("BeastMoveLeft");
			while (MoveTowardsStart(firstPosition)) { yield return null; }
			if (!GMan.MariamHero)
				playerAnimator.Play("FightManLeft_Idle");
			else
				playerAnimator.Play("Beast_Idle");
			//remove this performer from the list in BSM because you don't want him to do action twice
			BSM.PerformList.RemoveAt(0);
			//reset the battle state machine and set monster to wait
			if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
			{
				Debug.Log("PROCESSING AGAIN!");
				BSM.battleStates = BattleStateMachine.PerformAction.WAIT;

				actionStarted = false;
				cur_cooldown = 0f;
				currentState = TurnState.PROCESSING;

			}
			else
			{ BSM.battleStates = BattleStateMachine.PerformAction.CHECKALIVE;
				currentState = TurnState.WAITING;
			}

			//BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
			//end coroutine
			actionStarted = false;
			//reset this enemy state
			//cur_cooldown = 0f;
			//currentState = TurnState.PROCESSING;
			HSM.battleAnim = false;
		}
        //Selector.SetActive(false);
    }

	private bool MoveTowardsEnemy(Vector3 target)
	{
		return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
	}
	private bool MoveTowardsStart(Vector3 target)
	{
		return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
	}

	public void DoDamage()//
	{
		float calc_damage = BSM.PerformList[0].chooseanAttack.attackDamage + BSM.randomAttack;//base attack damage + the damage from the type of attack chosen
																						  //float calc_damage = enemy.curATK + BSM.PerformList[0].chooseanAttack.attackDamage;//base attack damage + the damage from the type of attack chosen
		HeroToAttack.GetComponent<HeroStateMachine>().TakeDamage(calc_damage);

        /*if (enemy.theName == "Ruffian_1" )
        {
           // AudioManager.instance.PlaySound("punch", transform.position, 1);

        }
        else
        {*/
            
            AudioManager.instance.PlaySound("Spell1", transform.position, 1);
        //}
    }

	public void TakeDamage(float getDamageAmount)
	{
		enemy.curHP -= getDamageAmount;
		damage = getDamageAmount;
		if (enemy.curHP <= 0)
		{
			enemy.curHP = 0;
			currentState = TurnState.DEAD;
			this.gameObject.GetComponent<SpriteRenderer>().enabled = false;

            AudioManager.instance.PlaySound("Dead", transform.position, 1);
            Instantiate(deathAnim, transform.position, deathAnim.rotation);
			deathAnimTimer = true;
		}
		
		//DMGText.transform.Rotate(Vector3.left * 1000); //10000
		StartCoroutine(waitForDamage());
		if (HSM.comboNumber <= 1)
		{
			if (!GMan.MariamHero)
			{
				playerAnimator.Play("FightManLeft_Hit");

				StartCoroutine(damageAnimWait());
			}
			else
			{
				if (!EES.boss)
					{
					playerAnimator.Play("Beast_GetHit");

					StartCoroutine(damageAnimWait()); }
				else
				{playerAnimator.Play("Beast2_GetHit");

					StartCoroutine(damageAnimWait());  }
			}

		}
	}

	private IEnumerator damageAnimWait()
	{
		RuntimeAnimatorController animc = playerAnimator.runtimeAnimatorController;
		if (!GMan.MariamHero)
		{
			for (int i = 0; i < animc.animationClips.Length; i++)
			{
				if (animc.animationClips[i].name == "FightManLeft_Hit")
				{

					yield return new WaitForSeconds(animc.animationClips[i].length);
					playerAnimator.Play("FightManLeft_Idle");
				}
			}
		}
		else

		{
			if (!EES.boss)
			{
				for (int i = 0; i < animc.animationClips.Length; i++)
				{
					if (animc.animationClips[i].name == "Beast_GetHit")
					{

						yield return new WaitForSeconds(animc.animationClips[i].length);
						playerAnimator.Play("Beast_Idle");
					}
				}
			}
			else
			{
				for (int i = 0; i < animc.animationClips.Length; i++)
				{
					if (animc.animationClips[i].name == "Beast2_GetHit")
					{

						yield return new WaitForSeconds(animc.animationClips[i].length);
						playerAnimator.Play("Beast2_Idle");
					}
				}
			}
		}
	}

	public IEnumerator waitForDamage()
{
		GameObject DMGText = Instantiate(BSM.dmgText, transform.position, Quaternion.identity) as GameObject;
		//DMGText.transform.position = new Vector3(1, 1, 1);
		DMGText.transform.SetParent(BSM.BattleCanvas, false);
		//Text dmg_text = DMGText.transform.Find("Text").gameObject.GetComponent<Text>();
		//dmg_text.text = damage;
		rigidbody = DMGText;
		dmgBtns.Add(DMGText);
		rigidbod = DMGText.GetComponent<Rigidbody2D>();
		DMGTEXT = DMGText.GetComponent<Text>();
		DMGTEXT.text = "" + damage;
		if (HSM.comboNumber <= 1)
		{
			
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

			if (BSM.magicalAtk)
			{
				if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
				{
					BSM.battleStates = BattleStateMachine.PerformAction.WAIT; //this isnt working
					HSM.cur_cooldown = 0f;
					HSM.currentState = HeroStateMachine.TurnState.PROCESSING;
					BSM.magicalAtk = false;
					HSM.attacking = false;
				
					HSM.actionStarted = false;
				}
			}
			//reset the battle state machine and set monster to wait

			//BSM.battleStates = BattleStateMachine.PerformAction.WAIT;

			//end coroutine
			
			//reset this enemy state
			//cur_cooldown = 0f;
			//currentState = TurnState.PROCESSING;
			//BSM.PerformList.RemoveAt(0);
	}

		else if (HSM.comboNumber == 2)
		{
			rigidbod.gravityScale = -3.75f; //-2
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
			rigidbod.gravityScale = 2f;
			DMGTEXT.color = Color.yellow;
			yield return new WaitForSeconds(.3f);
			Destroy(DMGText);
		}
		else if (HSM.comboNumber == 3)
		{
			rigidbod.gravityScale = -5.5f; //-2
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
			rigidbod.gravityScale = 3f;
			DMGTEXT.color = Color.red;
			yield return new WaitForSeconds(.3f);
			Destroy(DMGText);
		}
		else if (HSM.comboNumber > 3)
		{	rigidbod.gravityScale = -7.5f; //-2
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
		rigidbod.gravityScale = 4f;
		DMGTEXT.color = Color.magenta;
		yield return new WaitForSeconds(.3f);
		Destroy(DMGText);
			}//else if (HSM.comboNumber == 5)
		//else if (HSM.comboNumber == 6)

		//foreach (GameObject dmg in dmgBtns)
		//	Destroy(dmg);

		/*yield return new WaitForSeconds(.3f);
		rigidbod.gravityScale = .9f;
		yield return new WaitForSeconds(.3f);
		rigidbod.gravityScale = 0f;
		yield return new WaitForSeconds(.5f);
		this one is slower but works more like final fantasy
		*/
		/*rigidbod.gravityScale = 1f;
		yield return new WaitForSeconds(.5f);
		foreach (GameObject dmg in dmgBtns)
		Destroy(dmg);
		*/
	}
}



/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStateMachine : MonoBehaviour
{

	private BattleStateMachine BSM;
	public BaseEnemy enemy;

	public enum TurnState
	{
		PROCESSING,
		CHOOSEACTION,
		WAITING,
		ACTION,
		DEAD
	}

	public TurnState currentState;
	//for the ProgressBar
	//cd == cooldown
	private float cur_cd = 0f;
	private float max_cd = 10f;
	public Image ProgressBar;
	//this gameoject
	private Vector3 startPosition;
	//Waiting Time
	private bool actionStarted = false;
	public GameObject HeroToAttack;
	public GameObject selector;
	private float animSpeed = 10f;
	//Dead or Not
	private bool Alive = true;
	//Enemy Panel



	void Start()
	{


		currentState = TurnState.PROCESSING;
		BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
		startPosition = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		switch (currentState)
		{
			case (TurnState.PROCESSING):
				UpgradeProgressBar();
				break;
			case (TurnState.CHOOSEACTION):
				ChooseAction();
				currentState = TurnState.WAITING;
				break;
			case (TurnState.WAITING):
				//idle
				break;
			case (TurnState.ACTION):
				StartCoroutine(TimeForAction());

				break;
			
		}
	}
	void UpgradeProgressBar()
	{
		cur_cd = cur_cd + Time.deltaTime;
		if (cur_cd >= max_cd)
		{
			currentState = TurnState.CHOOSEACTION;
		}
	}
	void ChooseAction()
	{
		HandleTurns myAttack = new HandleTurns();
		//get name
		myAttack.Attacker = enemy.name;
		myAttack.Type = "Enemy";
		//which one is attacking
		myAttack.AttackersGameObject = this.gameObject;
		//random choose a target 
		myAttack.AttackersTarget = BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)];

		
		BSM.CollectActions(myAttack);
	}
	private IEnumerator TimeForAction()
	{
		if (actionStarted)
		{
			yield break;
		}

		actionStarted = true;

		//animate the enemy near the hero to attack
		Vector3 HeroPosition = new Vector3(HeroToAttack.transform.position.x - 1.5f, HeroToAttack.transform.position.y, HeroToAttack.transform.position.z);
		while (MoveTowardsEnemy(HeroPosition)) { yield return null; }

		//wait
		yield return new WaitForSeconds(0.5f);
		//damage
		//animate return to start position
		Vector3 firstPosition = startPosition;
		while (MoveTowardsStart(firstPosition)) { yield return null; }

		//remove this perform for the list
		BSM.PerformList.RemoveAt(0);

		//reset the BSM > wait
		BSM.battleStates = BattleStateMachine.PerformAction.WAIT;

		actionStarted = false;
		cur_cd = 0f;
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

	


}
*/