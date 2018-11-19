using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissile : BaseAttack
{

	public MagicMissile()
	{
		attackName = "Magic Missile";
		attackDescription = "Launches Shards of Ice 10 MP";
		attackDamage = 80f;
		attackCost = 10f;
	}
}
