using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogueManager : MonoBehaviour {

    public GameObject dBox;
    public Text dText;
    public Text dSpeaker;

    public bool dialogActive;

    //public string[] speakers;
    //public string[] dialogLines;
    public int currentLine;
    public AudioClip selectSound;
    public AudioSource musicSource;

    private PlayerController thePlayer;

    public bool keyUpFromExit = false;

    // Use this for initialization
    void Start()
    {
        musicSource.clip = selectSound;
        //dialogActive = false;
        thePlayer = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //musicSource.Play();
        }


        //if (currentLine >= dialogLines.Length)
        //{
        //    dBox.SetActive(false);
        //    dialogActive = false;

        //    currentLine = 0;
        //}

        //dText.text = dialogLines[currentLine];
        //dSpeaker.text = speakers[currentLine];
        //Debug.Log(currentLine);
    }

    public void hideBox()
    {
        dBox.SetActive(false);
        dialogActive = false;
        musicSource.Play();
        //currentLine++;
        thePlayer.canMove = true;
    }

    public void ShowBox(string name, string line)
    {
        dialogActive = true;
        dBox.SetActive(true);
        dSpeaker.text = name;
        dText.text = line;
        thePlayer.canMove = false;
        //dText.text = dialogue;
        //speakers = ppl;
        //dialogLines = lines;
    }

    public void ShowDialogue()
    {
        dialogActive = true;
        dBox.SetActive(true);

    }
}
