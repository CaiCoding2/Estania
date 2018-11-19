using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TownCutSceneManager : MonoBehaviour
{

    public bool cutsceneActive;

    public AnimationClip[] mariamAnims;
    public AnimationClip[] townmanAnims;

    public Animator townSpeaker;
    public Animator mariam;
    public GameObject Mist_Screen;
    public Image mist;
    public Image blackScreen;
    public int currentLine;

    private float mist_opacity;

    private bool isWalking = true;
    private bool secondMovement = true;

    // Use this for initialization
    void Start()
    {
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0f);
        mist.color = new Color(mist.color.r, mist.color.g, mist.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (cutsceneActive && Input.GetKeyDown(KeyCode.A) && !InGame.GameIsPaused)
        {
            currentLine++;
        }

        //cutscene sound effect
        if (currentLine == 0)
        {
            if (isWalking)
            {
                AudioManager.instance.PlaySound("FootStepLong", transform.position, 1);
                isWalking = false;
            }
        }
        if (currentLine == 1)
        {
            if (secondMovement)
            {
                AudioManager.instance.PlaySound("FootStepLong", transform.position, 1);
                secondMovement = false;
            }
        }
        if (currentLine == 4)
        {
            if (isWalking)
            {
                AudioManager.instance.PlaySound("FootStepLong", transform.position, 1);
                isWalking = false;
            }
        }
        if (currentLine >= 1 && currentLine < 4) { isWalking = true; }

        if (currentLine >= 5)
        {
            if (mist_opacity < 1.0f) mist_opacity += 0.01f;
            mist.color = new Color(mist.color.r, mist.color.g, mist.color.b, mist_opacity);
        }

        if (currentLine >= mariamAnims.Length)
        {
            cutsceneActive = false;
			StartCoroutine(waitForLoad());
			//blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1f);
			currentLine = 0;

        }

        if (cutsceneActive)
        {
            townSpeaker.Play(townmanAnims[currentLine].name);
            mariam.Play(mariamAnims[currentLine].name);
        }
        //Debug.Log(currentLine);
    }

    public void ShowCutscene()
    {
        cutsceneActive = true;
    }
	private IEnumerator waitForLoad()
	{
		//yield return new WaitForSeconds(1.25f);
		mist.color = new Color(mist.color.r, mist.color.g, mist.color.b, 0);
		blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1f);
		yield return new WaitForSeconds(1.25f);
		SceneManager.LoadScene("Sleeping Hart");
	}
}
