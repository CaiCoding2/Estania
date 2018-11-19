using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour {

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

 
    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
	}

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void playClick()
    {
        AudioManager.instance.PlaySound("Click", transform.position, 1);
    }

    public void SetMasterVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);
    }

    public void SetSfxVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Sfx);
    }
}
