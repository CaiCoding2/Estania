using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public GameObject mainMenuHolder;
	public GameObject optionsMenuHolder;
	public GameObject creditsHolder;
	public Slider[] volumeSliders;
	

	void Start()
	{

		SetMusicVolume(.5f);
		volumeSliders [0].value = AudioManager.instance.masterVolumePercent;
		volumeSliders [1].value = AudioManager.instance.musicVolumePercent;
		volumeSliders [2].value = AudioManager.instance.sfxVolumePercent;

	}
	
	public void Play() {
		SceneManager.LoadScene ("Temple CutScene");
	}
	
	public void Quit() {
		Application.Quit ();
	}

	public void OptionsMenu() {
		mainMenuHolder.SetActive (false);
		optionsMenuHolder.SetActive (true);
		creditsHolder.SetActive (false);
	}

	public void MainMenu() {
		mainMenuHolder.SetActive (true);
		optionsMenuHolder.SetActive (false);
		creditsHolder.SetActive (false);
	}
	
	
	public void Credits() {
		mainMenuHolder.SetActive (false);
		optionsMenuHolder.SetActive (false);
		creditsHolder.SetActive (true);
	}

	public void SetMasterVolume(float value) {
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Master);
	}

	public void SetMusicVolume(float value) {
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Music);
	}

	public void SetSfxVolume(float value) {
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Sfx);
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