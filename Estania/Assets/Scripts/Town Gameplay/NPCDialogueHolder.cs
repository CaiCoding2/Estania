using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueHolder : MonoBehaviour
{

    private NPCDialogueManager dMan;

    public string speaker;
    public string dialogueLine;

    private bool triggerDialogue = false;

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
        //Debug.Log("dialogueActive");
        //Debug.Log(dMan.dialogActive);

        if (!dMan.dialogActive && Input.GetKeyDown(KeyCode.A) && triggerDialogue)
        {
            //Debug.Log("SHOW BOX");
            dMan.ShowBox(speaker, dialogueLine);
            

            if (transform.parent.GetComponent<NPCVertical>() != null)
            {
                transform.parent.GetComponent<NPCVertical>().canMove = false;
            }
            if (transform.parent.GetComponent<NPCHorizontal>() != null)
            {
                transform.parent.GetComponent<NPCHorizontal>().canMove = false;
            }
            
            return;
        }

        
        if(dMan.dialogActive && Input.GetKeyDown(KeyCode.A) && triggerDialogue)
        {
            //Debug.Log("HIDE BOX");
            dMan.hideBox();
        }
        

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            //Debug.Log("Enter");
            triggerDialogue = true;    
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.name == "Player")
        {
            triggerDialogue = false;
            //Debug.Log("Exit");
        }
    }


}
