using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TownCutsceneHolder : MonoBehaviour
{

    private TownCutSceneManager cMan;

    public AnimationClip[] townmanAnims;
    public AnimationClip[] mariamAnims;

    // Use this for initialization
    void Start()
    {
        cMan = FindObjectOfType<TownCutSceneManager>();
   
        cMan.townmanAnims = townmanAnims;
        cMan.mariamAnims = mariamAnims;
        cMan.currentLine = 0;
        cMan.ShowCutscene();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
