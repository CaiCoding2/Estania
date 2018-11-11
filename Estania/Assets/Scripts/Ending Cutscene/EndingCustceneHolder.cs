using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCustceneHolder : MonoBehaviour {

    private EndingCutsceneTimelineManager eMan;

    public AnimationClip[] mariamAnims;
    public AnimationClip[] chiefAnims;
    public AnimationClip[] manALeftAnims;
    public AnimationClip[] manARightAnims;
    public AnimationClip[] manBAnims;
    public AnimationClip[] womanAAnims;

    // Use this for initialization
    void Start () {
        eMan = FindObjectOfType<EndingCutsceneTimelineManager>();
        eMan.mariamAnims = mariamAnims;
        eMan.chiefAnims = chiefAnims;
        eMan.manALeftAnims = manALeftAnims;
        eMan.manARightAnims = manARightAnims;
        eMan.manBAnims = manBAnims;
        eMan.womanAAnims = womanAAnims;
        eMan.ShowCutscene();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
