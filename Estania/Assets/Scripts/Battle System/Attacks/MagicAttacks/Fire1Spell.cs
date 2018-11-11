using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire1Spell : BaseAttack {

	public Fire1Spell()
	{
		attackName = "Fire 1";
		attackDescription = "Lights enemy ablaze 10 MP";
		attackDamage = 20f;
		attackCost = 10f;
	}
}
