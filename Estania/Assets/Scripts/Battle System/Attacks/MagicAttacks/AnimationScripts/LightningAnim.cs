//old
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAnim : MonoBehaviour
{
	public HeroStateMachine HSM;
	public Animator playerAnimator;
	public BattleStateMachine BSM;
	public EnemyStateMachine ESM;

	void Start()

	{
		BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>(); //connects enemystatemachine to battlestatemachine
		playerAnimator = GameObject.Find("lightning true 1").GetComponent<Animator>();
		HSM = GameObject.Find("Hero 1").GetComponent<HeroStateMachine>();
	}

	public IEnumerator lightningAnimation()
	{
		HSM.battleAnim = true;
		Debug.Log("at lightning");
		RuntimeAnimatorController ac = playerAnimator.runtimeAnimatorController;
		BSM.infoPanel.SetActive(true);
		BSM.InfoText.text = "Lightning";
		yield return new WaitForSeconds(0.40f); //was at 1.5, was at .75, was at .40
		StartCoroutine(waitForInfo());
		playerAnimator.Play("lightning");

		for (int i = 0; i < ac.animationClips.Length; i++)
		{
			Debug.Log(ac.animationClips[i].name);
			if (ac.animationClips[i].name == "lightning")
			{

				//HSM.battleAnim = true;
				yield return new WaitForSeconds(ac.animationClips[i].length);
			}
		}
		Debug.Log("Playing no lightning");

		playerAnimator.Play("Nolightning");
		ESM = BSM.PerformList[0].AttackersGameObject.GetComponent<EnemyStateMachine>();
		ESM.DoDamage();

		yield return new WaitForSeconds(.5f);
		BSM.PerformList.RemoveAt(0);
		if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
		{
			BSM.battleStates = BattleStateMachine.PerformAction.WAIT; //this isnt working
			ESM.cur_cooldown = 0f;
			ESM.currentState = EnemyStateMachine.TurnState.PROCESSING;

			ESM.actionStarted = false;
		}
		else
		{
			BSM.battleStates = BattleStateMachine.PerformAction.CHECKALIVE;
			ESM.currentState = EnemyStateMachine.TurnState.WAITING;
		}
		playerAnimator.Play("Beast_Idle");
		HSM.battleAnim = false;
	}
		public IEnumerator waitForInfo()
	{
		yield return new WaitForSeconds(.43f);
		BSM.infoPanel.SetActive(false);
	}
}
