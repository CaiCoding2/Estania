using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownCutSceneDialogueHolder : MonoBehaviour {

    //public string dialogue;
    private CutsceneDialogueManager dMan;

    public string[] speakers;
    public string[] dialogueLines;

    // Use this for initialization
    void Start()
    {
        dMan = FindObjectOfType<CutsceneDialogueManager>();
        StartCoroutine(beginDialogue());

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator beginDialogue()
    {
        yield return new WaitForSeconds(1);
        dMan.dialogLines = dialogueLines;
        dMan.speakers = speakers;
        dMan.currentLine = 0;
        dMan.ShowDialogue();
    }
}
