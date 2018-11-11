using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueHolder : MonoBehaviour
{

    private NPCDialogueManager dMan;

    public string speaker;
    public string dialogueLine;

    // Use this for initialization
    void Start()
    {
        dMan = FindObjectOfType<NPCDialogueManager>();
        //dMan.dialogLines = dialogueLines;
        //dMan.speakers = speakers;
        //dMan.currentLine = 0;
        //dMan.ShowDialogue();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.name == "Player")
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                dMan.ShowBox(speaker, dialogueLine);
            }

            if (transform.parent.GetComponent<NPCVertical>() != null)
            {
                transform.parent.GetComponent<NPCVertical>().canMove = false;
            }else if (transform.parent.GetComponent<NPCHorizontal>() != null)
            {
                transform.parent.GetComponent<NPCHorizontal>().canMove = false;
            }
        }
        
    }


}
