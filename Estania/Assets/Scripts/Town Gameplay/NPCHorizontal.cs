using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHorizontal : MonoBehaviour {

    public float moveSpeed;
    public float moveDistance;

    private Rigidbody2D myRigidbody;

    private bool moving;

    public float timeBetweenMove;
    private float timeBetweenMoveCounter;
    public float timeToMove;
    private float timeToMoveCounter;

    private Vector3 moveDirection;
    public bool startDirectionRight;
    private bool moveLeft;

    public AnimationClip leftAnim;
    public AnimationClip rightAnim;
    private Animator anim;

    public bool canMove;
    private NPCDialogueManager NPCDM;

    // Use this for initialization
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        timeBetweenMoveCounter = timeBetweenMove;
        timeToMoveCounter = timeToMove;

        moveLeft = startDirectionRight ? true : false;

        anim = GetComponent<Animator>();

        canMove = true;
        NPCDM = FindObjectOfType<NPCDialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!NPCDM.dialogActive)
        {
            canMove = true;
        }

        if (!canMove)
        {
            myRigidbody.velocity = Vector2.zero;
            return;
        }

        if (moving)
        {
            timeToMoveCounter -= Time.deltaTime;
            myRigidbody.velocity = moveDirection;

            if (timeToMoveCounter < 0f)
            {
                moving = false;
                timeBetweenMoveCounter = timeBetweenMove;
            }

            anim.speed = moveSpeed;
            if (moveLeft)
            {
                anim.Play(leftAnim.name);
            }
            else
            {
                anim.Play(rightAnim.name);
            }
        }
        else
        {
            timeBetweenMoveCounter -= Time.deltaTime;
            myRigidbody.velocity = Vector2.zero;

            anim.speed = 0;
            if (timeBetweenMoveCounter < 0f)
            {
                moving = true;
                timeToMoveCounter = timeToMove;


                if (moveLeft)
                {
                    moveDirection = new Vector3(moveDistance * moveSpeed, 0f , 0f);
                    moveLeft = false;
                }
                else
                {
                    moveDirection = new Vector3(-moveDistance * moveSpeed, 0f , 0f);
                    moveLeft = true;
                }

            }
        }
    }
}
