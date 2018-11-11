using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneDialogueManager : MonoBehaviour {
    
    public GameObject dBox;
    public Text dText;
    public Text dSpeaker;

    public bool dialogActive;

    public string[] speakers;
    public string[] dialogLines;
    public int currentLine;
    //public AudioClip selectSound;
    //public AudioSource musicSource;

    // Use this for initialization
    void Start()
    {
        //musicSource.clip = selectSound;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AudioManager.instance.PlaySound("Select", transform.position, 1);
        }
        

        if (dialogActive && Input.GetKeyDown(KeyCode.A))
        {
            currentLine++;
        }
        if (currentLine >= dialogLines.Length)
        {
            dBox.SetActive(false);
            dialogActive = false;

            currentLine = 0;
        }

        dText.text = dialogLines[currentLine];
        dSpeaker.text = speakers[currentLine];
        //Debug.Log(currentLine);
    }


    public void ShowBox(string dialogue)
    {
        dialogActive = true;
        dBox.SetActive(true);
        dText.text = dialogue;
    }

    public void ShowDialogue()
    {
        dialogActive = true;
        dBox.SetActive(true);

    }
}
