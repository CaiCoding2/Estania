using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastIdle : MonoBehaviour {

	Animator m_Animator;

	void Start()
	{
		//Fetch the Animator from your GameObject
		m_Animator = GameObject.Find("FightManRight_idle_00").GetComponent<Animator>();
	}

	private void Update()
	{
	
			m_Animator.Play("FightManLeft_Punch");
		
	}
}


