using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MusicManager : MonoBehaviour
{
    public AudioClip menuTheme;
    //cutsene Music
    public AudioClip TempleTheme;
    public AudioClip TownTheme;
    public AudioClip SleepTheme;
    public AudioClip FieldTheme;
    public AudioClip EndingTheme;
    public AudioClip FinaleTheme;

    //Battle Music
    public AudioClip NormalBattle;
    public AudioClip BossBattle;

    //Map;
    public AudioClip MainTown;
    public AudioClip MistField;
   


    string sceneName;

    
    void Start() {
        OnLevelWasLoaded (0);
    }


    void OnLevelWasLoaded(int sceneIndex) {
        string newSceneName = SceneManager.GetActiveScene ().name;
        if (newSceneName != sceneName) {
            sceneName = newSceneName;
            Invoke ("PlayMusic", .2f);
        }
    }
    
    void PlayMusic()
    {
        AudioClip clipToPlay = null;

        if (sceneName == "Menu")
        {
            clipToPlay = menuTheme;
        }
        
        else if (sceneName == "Temple Cutscene")
        {
            clipToPlay = TempleTheme;
        }

        else if (sceneName == "TownScene")
        {
            clipToPlay = TownTheme;
        }

        else if (sceneName == "Sleeping Hart")
        {
            clipToPlay = SleepTheme;
        }

        else if (sceneName == "FieldScene")
        {
            clipToPlay = FieldTheme;
        }

        else if (sceneName == "Ending Cutscene")
        {
            clipToPlay = EndingTheme;
        }

        else if (sceneName == "Finale")
        {
            clipToPlay = FinaleTheme;
        }

        else if (sceneName == "MenBattleScene" || sceneName == "FieldLavosSpawn")
        {
            clipToPlay = NormalBattle;
        }

        else if (sceneName == "FieldLavos")
        {
            clipToPlay = BossBattle;
        }

        else if (sceneName == "RodrikPreContender")
        {
            clipToPlay = MainTown;
        }

        else if (sceneName == "AfterSeerField")
        {
            clipToPlay = MistField;
        }

        if (clipToPlay != null)
        {
            AudioManager.instance.playMusic(clipToPlay, 0f);
            Invoke("PlayMusic", clipToPlay.length);
        }


    }
}
