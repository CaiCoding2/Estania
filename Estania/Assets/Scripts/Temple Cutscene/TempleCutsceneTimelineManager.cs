using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TempleCutsceneTimelineManager : MonoBehaviour {

    public bool cutsceneActive;

    public AnimationClip[] hartAnims;
    public AnimationClip[] alexaAnims;
    public AnimationClip[] mariamAnims;

    public Animator hart;
    public Animator alexa;
    public Animator mariam;

    public Image blackScreen;
    public int currentLine;

    private bool doorOpen = false;
    private bool isWalking = true;

    // Use this for initialization
    void Start () {
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0f);	
	}
	
	// Update is called once per frame
	void Update () {
		if(cutsceneActive && Input.GetKeyDown(KeyCode.A))
        {
            currentLine++;
        }
        //cutScend sound effect
        if (currentLine == 0)
        {
            if (!doorOpen)
            {
                AudioManager.instance.PlaySound("DoorOpen", transform.position, 1);
                doorOpen = true;
            }
            if (isWalking)
            {
                AudioManager.instance.PlaySound("FootStepLong", transform.position, 1);
                isWalking = false;
            }
        }
        if (currentLine > 0 && currentLine < 4) { isWalking = true; }
        if (currentLine == 4)
        {
            if (isWalking)
            {
                AudioManager.instance.PlaySound("FootStepLong", transform.position, 1);
                isWalking = false;
            }
        }
        if (currentLine >= mariamAnims.Length)
        {
            cutsceneActive = false;
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1f);
            currentLine = 0;
            AudioManager.instance.PlaySound("DoorClose", transform.position, 1);
            StartCoroutine(waitForLoad());
        }

        if (currentLine >= mariamAnims.Length)
        {
            cutsceneActive = false;
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1f);
            currentLine = 0;
			StartCoroutine(waitForLoad());
        }
        if (cutsceneActive)
        {
            hart.Play(hartAnims[currentLine].name);
            alexa.Play(alexaAnims[currentLine].name);
            mariam.Play(mariamAnims[currentLine].name);
        }
            //Debug.Log(currentLine);
    }

    public void ShowCutscene()
    {
        cutsceneActive = true;
    }
	private IEnumerator waitForLoad()
{ yield return new WaitForSeconds(1.25f);
	SceneManager.LoadScene("RodrikPreContender");
}
}

