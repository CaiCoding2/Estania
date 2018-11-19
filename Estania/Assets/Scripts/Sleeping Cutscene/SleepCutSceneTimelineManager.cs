using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SleepCutSceneTimelineManager : MonoBehaviour {

    public bool cutsceneActive;

    public AnimationClip[] hartAnims;
    public AnimationClip[] alexaAnims;
    public AnimationClip[] mariamAnims;

    public Animator hart;
    public Animator alexa;
    public Animator mariam;

    public Image blackScreen;
    public int currentLine;


    // Use this for initialization
    void Start () {
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (cutsceneActive && Input.GetKeyDown(KeyCode.A) && !InGame.GameIsPaused)
        {
            currentLine++;
        }
        if (currentLine >= mariamAnims.Length)
        {
            cutsceneActive = false;
           // blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1f);
            currentLine = 0;
			StartCoroutine(waitForLoad());
		}

        if (cutsceneActive)
        {
            hart.Play(hartAnims[currentLine].name);
            alexa.Play(alexaAnims[currentLine].name);
            mariam.Play(mariamAnims[currentLine].name);

        }
    }

    public void ShowCutscene()
    {
        cutsceneActive = true;
    }

	private IEnumerator waitForLoad()
	{
		//yield return new WaitForSeconds(1.25f);
		blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1f);
		yield return new WaitForSeconds(1.25f);
		SceneManager.LoadScene("FieldScene");
	}
}
