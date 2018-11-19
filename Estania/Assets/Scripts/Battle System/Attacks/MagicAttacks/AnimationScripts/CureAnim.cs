//old
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureAnim : MonoBehaviour
{
	public HeroStateMachine HSM;
	public Animator playerAnimator;
	public BattleStateMachine BSM;

	void Start()

	{
		BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>(); //connects enemystatemachine to battlestatemachine
		playerAnimator = GameObject.Find("Cure").GetComponent<Animator>();
		HSM = GameObject.Find("Hero 1").GetComponent<HeroStateMachine>();
	}

	public IEnumerator cureAnimation()
	{
		Debug.Log("AT CURE ANIMATION");
		RuntimeAnimatorController ac = playerAnimator.runtimeAnimatorController;
		BSM.infoPanel.SetActive(true);
		BSM.InfoText.text = "Cure";
		AudioManager.instance.PlaySound("Cure", transform.position, 1);
		yield return new WaitForSeconds(0.40f); //was at 1.5, was at .75, was at .40
		StartCoroutine(waitForInfo());
		playerAnimator.Play("Cure");
		if (HSM.spellName == "Cure")
			for (int i = 0; i < ac.animationClips.Length; i++)
			{
				Debug.Log(ac.animationClips[i].name);
				if (ac.animationClips[i].name == "Cure")
				{

					//HSM.battleAnim = true;
					yield return new WaitForSeconds(ac.animationClips[i].length);
				}
			}
		Debug.Log("Playing no cure");

		playerAnimator.Play("Nocure");
		HSM.heal();
		StartCoroutine(HSM.waitForHeal());
		BSM.PerformList.RemoveAt(0);
		if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
		{
			BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
			HSM.cur_cooldown = 0f;
			HSM.currentState = HeroStateMachine.TurnState.PROCESSING;
			HSM.actionStarted = false;
		}
		HSM.battleAnim = false;
		HSM.magicAnim = false;
		BSM.magicalAtk = false;
		HSM.attacking = false;

		HSM.actionStarted = false;
	}
	public IEnumerator waitForInfo()
	{
		yield return new WaitForSeconds(.43f);
		BSM.infoPanel.SetActive(false);
	}
}
