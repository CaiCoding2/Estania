using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepCutsceneDialogueHolder : MonoBehaviour {

    private CutsceneDialogueManager dMan;

    public string[] dialogueLines;
    // Use this for initialization
    void Start () {
        dMan = FindObjectOfType<CutsceneDialogueManager>();
        StartCoroutine(beginDialogue());

    }

    // Update is called once per frame
    void Update () {
		
	}

    IEnumerator beginDialogue()
    {
        yield return new WaitForSeconds(0);
        dMan.dialogLines = dialogueLines;
        dMan.currentLine = 0;
        dMan.ShowDialogue();
    }
}
