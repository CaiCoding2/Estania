using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TempleTimelineHolder : MonoBehaviour {

    private TempleCutsceneTimelineManager cMan;

    public AnimationClip[] hartAnims;
    public AnimationClip[] alexaAnims;
    public AnimationClip[] mariamAnims;

    // Use this for initialization
    void Start () {
        cMan = FindObjectOfType<TempleCutsceneTimelineManager>();
        cMan.hartAnims = hartAnims;
        cMan.alexaAnims = alexaAnims;
        cMan.mariamAnims = mariamAnims;
        cMan.currentLine = 0;
        cMan.ShowCutscene();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
