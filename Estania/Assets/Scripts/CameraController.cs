using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour {

	public bool inbattle;
	public Scene m_Scene;
	public string sceneName;
	public GameObject followTarget;
	private Vector3 targetPos;
	public float moveSpeed;
	private Vector3 minPosition;
	private Vector3 maxPosition;
	private Camera theCamera;
	private float halfHeight;
	private float halfWidth;
	public BoxCollider2D boundBox;
	public GameManager GMan;

	private static bool cameraExists;

	// Use this for initialization
	void Start() {
		//Debug.Log("AWAKE!");
		m_Scene = SceneManager.GetActiveScene();
		sceneName = m_Scene.name;
		//Debug.Log(sceneName);
		if (sceneName == "MenBattleScene")
		{
			inbattle = true;
			//Debug.Log("inbattle");
		}
		
		GMan = GameObject.Find("GameManager").GetComponent<GameManager>();
		/*if (inbattle)
			Destroy(this.gameObject);
		DontDestroyOnLoad(transform.gameObject);
		
		if (!cameraExists)
		{
			cameraExists = true;
			DontDestroyOnLoad(transform.gameObject);
		}
		else
		{
			Destroy(this.gameObject); ;
		}*/

		minPosition = boundBox.bounds.min;
		maxPosition = boundBox.bounds.max;

		theCamera = GetComponent<Camera>();
		halfHeight = theCamera.orthographicSize;
		halfWidth = halfHeight * Screen.width / Screen.height;
	}

	// Update is called once per frame
	void LateUpdate() {

		if (followTarget == null)
		{
			followTarget = GameObject.Find("Player");
		}
		//Debug.Log("Need player!");
		if (GameManager.instance.instantiated == true && GameObject.Find("Player") != null)

		{
			targetPos = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
			//targetPos = Mathf.Max(levelMinX, Mathf.Min(levelMaxX, cameraTarget.transform.position.x));
			transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
			if (!inbattle)
			{
				if (boundBox == null)
				{
					boundBox = FindObjectOfType<Bounds>().GetComponent<BoxCollider2D>();
					minPosition = boundBox.bounds.min;
					maxPosition = boundBox.bounds.max;
				}
				float clampedX = Mathf.Clamp(transform.position.x, minPosition.x + halfWidth, maxPosition.x - halfWidth);
				float clampedY = Mathf.Clamp(transform.position.y, minPosition.y + halfHeight, maxPosition.y - halfHeight);
				transform.position = new Vector3(clampedX, clampedY, transform.position.z);
			}
		}

    }
  
    public void SetBounds(BoxCollider2D newBounds)
		{
		boundBox = newBounds;
		minPosition = boundBox.bounds.min;
		maxPosition = boundBox.bounds.max;
	}

		//if (transform.position.x < minPosition)
		//{
		//transform.position.x = 12.1;
		//}
		//transform.position = Vector2.Min(targetPos, maxPosition);
		//targetX = Mathf.Max(levelMinX, Mathf.Min(levelMaxX, cameraTarget.transform.position.x));
	
}
