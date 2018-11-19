//old
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissileAnim : MonoBehaviour
{
	float animSpeed;
	public HeroStateMachine HSM;
	public Animator playerAnimator;
	public BattleStateMachine BSM;

	void Start()

	{
		animSpeed = 45f;
		BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>(); //connects enemystatemachine to battlestatemachine
		playerAnimator = GameObject.Find("Magic Missile").GetComponent<Animator>();
		HSM = GameObject.Find("Hero 1").GetComponent<HeroStateMachine>();
	}

	public IEnumerator magicMissileAnimation()
	{
		Debug.Log("AT CURE ANIMATION");
		RuntimeAnimatorController ac = playerAnimator.runtimeAnimatorController;
		BSM.infoPanel.SetActive(true);
		BSM.InfoText.text = "Magic Missile";
		AudioManager.instance.PlaySound("MagicMissile", transform.position, 1);
		yield return new WaitForSeconds(0.40f); //was at 1.5, was at .75, was at .40
		StartCoroutine(waitForInfo());
		playerAnimator.Play("Magic Missile");
		if (HSM.spellName == "Magic Missile")
			for (int i = 0; i < ac.animationClips.Length; i++)
			{
				Debug.Log(ac.animationClips[i].name);
				if (ac.animationClips[i].name == "Magic Missile")
				{

					//HSM.battleAnim = true;
					yield return new WaitForSeconds(ac.animationClips[i].length);
				}
			}
		Debug.Log("Playing no magic missile");
		Vector3 originalPosition = new Vector3(0, 0, 0) ;
		originalPosition = transform.position;
		playerAnimator.enabled = false;
		Vector3 heroPosition = new Vector3(HSM.EnemyToAttack.transform.position.x, HSM.EnemyToAttack.transform.position.y, HSM.EnemyToAttack.transform.position.z);
		while(MoveTowardsEnemy(heroPosition))
			yield return null;
		playerAnimator.enabled = true;
		playerAnimator.Play("NoMagicMissile");
		transform.position = originalPosition;
		HSM.doDamage();
		yield return new WaitForSeconds(1f);
		//need to move object back to original position
		BSM.PerformList.RemoveAt(0);
		if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
		{
			BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
			HSM.cur_cooldown = 0f;
			HSM.currentState = HeroStateMachine.TurnState.PROCESSING;
			//HSM.actionStarted = false;
		}
		HSM.battleAnim = false;
		HSM.magicAnim = false;
		HSM.spellName = "";
	}
	public bool MoveTowardsEnemy(Vector3 target)
	{
		return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
	}
	public IEnumerator waitForInfo()
	{
		yield return new WaitForSeconds(.43f);
		BSM.infoPanel.SetActive(false);
	}
}