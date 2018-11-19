using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cure : BaseAttack
{

	public Cure()
	{
		attackName = "Cure";
		attackDescription = "Recovers health 10 MP";
		attackDamage = 100f;
		attackCost = 10f;
	}
}
