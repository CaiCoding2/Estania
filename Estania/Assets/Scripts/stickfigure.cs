using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class stickfigure : MonoBehaviour {
	Animator m_Animator;

	// Use this for initialization
	void Start () {
			Animator m_Animator;
		m_Animator = GameObject.Find("StickFigure").GetComponent<Animator>();
		m_Animator.Play("stickrunning");
		transform.DOMove(new Vector3(5f, 5f, 0f), 10);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}




