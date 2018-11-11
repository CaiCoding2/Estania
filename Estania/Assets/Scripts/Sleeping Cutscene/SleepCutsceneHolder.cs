using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepCutsceneHolder : MonoBehaviour {

    private SleepCutSceneTimelineManager sMan;

    public AnimationClip[] hartAnims;
    public AnimationClip[] alexaAnims;
    public AnimationClip[] mariamAnims;

    // Use this for initialization
    void Start () {
        sMan = FindObjectOfType<SleepCutSceneTimelineManager>();
        sMan.mariamAnims = mariamAnims;
        sMan.alexaAnims = alexaAnims;
        sMan.hartAnims = hartAnims;
        sMan.ShowCutscene();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
