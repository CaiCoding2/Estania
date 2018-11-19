using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FieldCutSceneManager : MonoBehaviour
{

    public bool cutsceneActive;

    public AnimationClip[] seerAnims;
    public AnimationClip[] mariamAnims;

    public Animator seer;
    public Animator mariam;

    public Image blackScreen;
    public int currentLine;

    private bool isWalking = true;
    private bool isVanish = true;

    // Use this for initialization
    void Start()
    {
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (cutsceneActive && Input.GetKeyDown(KeyCode.A) && !InGame.GameIsPaused)
        {
            currentLine++;
        }
        //cutscene SoundEffect
        if (currentLine == 0)
        {
            if (isWalking)
            {
                AudioManager.instance.PlaySound("FootStepDelay", transform.position, 1);
                isWalking = false;
            }
        }
        if (currentLine == 5)
        {
            if (isVanish)
            {
                AudioManager.instance.PlaySound("ghostbreath", transform.position, 1);
                isVanish = false;
            }
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
            seer.Play(seerAnims[currentLine].name);
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
		yield return new WaitForSeconds(1.25f);
		SceneManager.LoadScene("AfterSeerField");
	}
}
