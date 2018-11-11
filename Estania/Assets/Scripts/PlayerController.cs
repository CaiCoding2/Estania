using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
	public bool inbattle;
    private Animator anim;
    private Rigidbody2D myRigidbody;
	public GameManager GMan;
    private bool playerMoving;
    public Vector2 lastMove;

	Vector3 curPos, lastPos;

    //effect
    public GameObject dust;
    public Transform feet;
    public GameObject scent;
    private bool scentTime = true;

    private static bool playerExists;

    public bool canMove;

	void Start()
	{
		transform.position = GameManager.instance.nextPlayerPosition;
		GMan = GameObject.Find("GameManager").GetComponent<GameManager>();
		if (inbattle)
			Destroy(this.gameObject);

		anim = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        /*
        if (!playerExists)
        {
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
		*/
        InvokeRepeating("walking", 0.0f, 0.3f);
        canMove = true;
	}
	
	// Update is called once per frame
	void Update () {
   
		if (inbattle)
		{
			Debug.Log("player in battle");
			//this.gameObject.GetComponent<SpriteRenderer>().SortingLayer = Default;
		}

		playerMoving = false;

        if (!canMove)
        {
            myRigidbody.velocity = Vector2.zero;
            return;
        }

		if(Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            //transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
            myRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, myRigidbody.velocity.y);
            playerMoving = true;
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            //transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed);
            playerMoving = true;
            lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
        }

        if(Input.GetAxisRaw("Horizontal") < 0.5f && Input.GetAxisRaw("Horizontal") > -0.5f)
        {
            myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
        }

        if (Input.GetAxisRaw("Vertical") < 0.5f && Input.GetAxisRaw("Vertical") > -0.5f)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0f);
        }

        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        anim.SetBool("PlayerMoving", playerMoving);
        anim.SetFloat("LastMoveX",lastMove.x);
        anim.SetFloat("LastMoveY",lastMove.y);

		curPos = transform.position;
		if (curPos == lastPos)
		{ GameManager.instance.isWalking = false;
			Debug.Log("Not walking");
		}
		else
		{
			Debug.Log("Walking");
			GameManager.instance.isWalking = true;
		}
		lastPos = curPos;
		
	}
void onTriggerEnter(Collider other)
		{
		Debug.Log("Collision from ontriger enter!");
			if (other.tag == "EnterArea")
			{
				CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
				GameManager.instance.nextPlayerPosition = other.gameObject.GetComponent<CollisionHandler>().spawnPoint.transform.position;
				GameManager.instance.sceneToLoad = col.sceneToLoad;
				GameManager.instance.loadNextScene();
			}

			if (other.tag == "LeaveArea")
			{
				CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
				GameManager.instance.nextPlayerPosition = other.gameObject.GetComponent<CollisionHandler>().spawnPoint.transform.position;
				GameManager.instance.sceneToLoad = col.sceneToLoad;
				GameManager.instance.loadNextScene();
			}
		if (other.tag == "region1")
		{
		GameManager.instance.curRegions = 0;
		}
	if (other.tag == "region2")
		{
	GameManager.instance.curRegions = 1;
	}
		}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "region1" || other.tag == "region2")
		{
			GameManager.instance.canGetEncountered = true;
		}
        if (other.gameObject.tag == "flower")
        {
            if (scentTime)
            {
                Instantiate(scent, other.transform.position, Quaternion.identity);
                scentTime = false;
            }

        }



    }
	/*void onTriggerStay(Collider other)
	{
		Debug.Log("colliding.");
		if (other.tag == "region1" || other.tag == "region2")
		{
			GameManager.instance.canGetEncountered = true;
		}
	}*/

	void onTriggerExit2D(Collider2D other)
	{
		if (other.tag == "region1" || other.tag == "region2")
		{
			GameManager.instance.canGetEncountered = false;
		}
       
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "flower")
        {
            if (scentTime)
            {
                AudioManager.instance.PlaySound("leave", transform.position, 1);
                Instantiate(scent, other.transform.position, Quaternion.identity);
                scentTime = false;
            }
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "flower")
        {
            scentTime = true;
        }
    }
    

    void walking()
    {
        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
        {
            if(Input.GetAxisRaw("Vertical") < 0)
            {
                AudioManager.instance.PlaySound("FootStep", transform.position, 1);
            }
            else
            {
                Instantiate(dust, feet.position, Quaternion.identity);
                AudioManager.instance.PlaySound("FootStep", transform.position, 1);

            }
            
        }

    }
  
}
