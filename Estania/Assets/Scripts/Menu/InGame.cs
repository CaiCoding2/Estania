using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame : MonoBehaviour {

    public static bool GameIsPaused = false;
    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;
    public GameObject aboutsHolder;
    public Slider[] volumeSliders;
    private bool isMain = true;

    public Button settingsButton;
    void Start()
    {
        volumeSliders[0].value = AudioManager.instance.masterVolumePercent;
        volumeSliders[1].value = AudioManager.instance.musicVolumePercent;
        volumeSliders[2].value = AudioManager.instance.sfxVolumePercent;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (GameIsPaused && isMain)
            {

                Resume();
            }
            else if(isMain == true )
            {
                AudioManager.instance.PlaySound("Select", transform.position, 1);
                Pause();
                settingsButton.Select();
            }
        }
    }

    void Resume()
    {
        mainMenuHolder.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

    }

    void Pause()
    {
        mainMenuHolder.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void OptionsMenu()
    {
        isMain = false;
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
        aboutsHolder.SetActive(false);
    }

    public void MainMenu()
    {
        isMain = true;
        mainMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
        aboutsHolder.SetActive(false);
    }


    public void Abouts()
    {
        isMain = false;
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(false);
        aboutsHolder.SetActive(true);
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

    public void playClick()
    {
        AudioManager.instance.PlaySound("Click", transform.position, 1);
    }

    public void selectNextButton(Button button)
    {
        button.Select();
    }

    public void selectNextSlider(Slider slider)
    {
        slider.Select();
    }
}
