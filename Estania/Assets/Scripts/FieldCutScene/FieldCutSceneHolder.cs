using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FieldCutSceneHolder : MonoBehaviour
{

    private FieldCutSceneManager cMan;

    public AnimationClip[] seerAnims;
    public AnimationClip[] mariamAnims;

    // Use this for initialization
    void Start()
    {
        cMan = FindObjectOfType<FieldCutSceneManager>();
        cMan.seerAnims = seerAnims;
        cMan.mariamAnims = mariamAnims;
        cMan.currentLine = 0;
        cMan.ShowCutscene();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
