using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EndingCutsceneTimelineManager : MonoBehaviour {

    public bool cutsceneActive;

    //change to the corresponding animators on screen
    public AnimationClip[] mariamAnims;
    public AnimationClip[] chiefAnims;
    public AnimationClip[] manALeftAnims;
    public AnimationClip[] manARightAnims;
    public AnimationClip[] manBAnims;
    public AnimationClip[] womanAAnims;

    public Animator mariam;
    public Animator chief;
    public Animator manALeft;
    public Animator manARight;
    public Animator manB;
    public Animator womanA;

    public Image blackScreen;
    private float blackOpacity = 0;
    public int currentLine;

    private bool isWalking = true;
    private bool isChained = true;

    // Use this for initialization
    void Start () {
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0f);
    }
	
	// Update is called once per frame
	void Update () {
        if (cutsceneActive && Input.GetKeyDown(KeyCode.A) && !InGame.GameIsPaused)
        {
            currentLine++;
        }

        //cutscene sound effect
        if (currentLine == 0)
        {
            if (isWalking)
            {
                AudioManager.instance.PlaySound("FootStepDelay", transform.position, 1);
                isWalking = false;
            }
        }
        if (currentLine > 0 && currentLine < 6) { isWalking = true; }

        if (currentLine == 1)
        {
            if (isChained)
            {
                AudioManager.instance.PlaySound("StepChain", transform.position, 1);
                isChained = false;
            }

        }
        if (currentLine == 6)
        {
            if (isWalking)
            {
                AudioManager.instance.PlaySound("FootStepDelay", transform.position, 1);
                isWalking = false;
            }

        }

        if (currentLine >= 6)
        {
            if (blackOpacity < 1.0f) blackOpacity += 0.01f;
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackOpacity);
			StartCoroutine(waitForLoad());
		}

        if (currentLine >= mariamAnims.Length)
        {
            cutsceneActive = false;
            currentLine = 0;
        }

        if (cutsceneActive)
        {
            mariam.Play(mariamAnims[currentLine].name);
            chief.Play(chiefAnims[currentLine].name);
            manALeft.Play(manALeftAnims[currentLine].name);
            manARight.Play(manARightAnims[currentLine].name);
            manB.Play(manBAnims[currentLine].name);
            womanA.Play(womanAAnims[currentLine].name);
        }

        //Debug.Log(currentLine);
    }

    public void ShowCutscene()
    {
        cutsceneActive = true;
    }
	private IEnumerator waitForLoad()
	{
		yield return new WaitForSeconds(2.5f);
		SceneManager.LoadScene("Finale");
	}
}
