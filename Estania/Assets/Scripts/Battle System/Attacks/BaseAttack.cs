using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAttack: MonoBehaviour
{
	public string attackName;
	public string attackDescription;
	public float attackDamage;//base damage 15, melee lvl 10 stamina 35 = basedmg + lvl + stamina
	public float attackCost;//Manacost
}
