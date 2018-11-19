using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinaleCutsceneManager : MonoBehaviour {
    public bool cutsceneActive;

    public AnimationClip[] mariamAnims;

    public Animator mariam;

    public Image blackScreen;
    public int currentLine;
    private float blackOpacity = 0f;

    public Text theEnd;
    private float theEndOpacity = 0f;

    private bool isLock = false;

    // Use this for initialization
    void Start () {
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0f);
        theEnd.color = new Color(theEnd.color.r, theEnd.color.g, theEnd.color.b, 0f);
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
            if (!isLock)
            {
                AudioManager.instance.PlaySound("Lock", transform.position, 1);
                isLock = true;
            }
        }

        if (currentLine >= 2)
        {
            if(blackOpacity <1.0f) blackOpacity += 0.01f;
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackOpacity);
            if(blackOpacity > 0.5f)
            {
                if(theEndOpacity<1.0f) theEndOpacity += 0.01f;
                theEnd.color = new Color(theEnd.color.r, theEnd.color.g, theEnd.color.b, theEndOpacity);
            }
        }

        if (currentLine >= mariamAnims.Length)
        {
            cutsceneActive = false;
            //blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1f);
            currentLine = 0;
            StartCoroutine(waitForLoad());
        }

        
        mariam.Play(mariamAnims[currentLine].name);
    }

    public void ShowCutscene()
    {
        cutsceneActive = true;
    }
    private IEnumerator waitForLoad()
    {
        yield return new WaitForSeconds(1.25f);
        SceneManager.LoadScene("Menu");
    }
}
