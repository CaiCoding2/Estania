using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinaleCutsceneHolder : MonoBehaviour {

    private FinaleCutsceneManager fMan;

    public AnimationClip[] mariamAnims;

    // Use this for initialization
    void Start () {
        fMan = FindObjectOfType<FinaleCutsceneManager>();
        fMan.mariamAnims = mariamAnims;
        fMan.ShowCutscene();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
