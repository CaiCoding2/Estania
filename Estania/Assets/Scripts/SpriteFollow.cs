using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFollow : MonoBehaviour {
    public Rigidbody2D attachment;
    private Rigidbody2D myRigidbody2D;

    private Animator anim;
    public bool asleep;
    //public Animation sleep;
    

    // Use this for initialization
    void Start () {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

       
        if(asleep) { anim.SetBool("fallSleep", true); }
        
	}
	
	// Update is called once per frame
	void Update () {
        myRigidbody2D.velocity = attachment.velocity;

        if (asleep) { anim.SetBool("fallSleep", true); }
        else { anim.SetBool("fallSleep", false); }
    }

    
}
