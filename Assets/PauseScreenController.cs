using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseScreenController : MonoBehaviour
{
    public GameObject screen;
	public AudioMixer musicMixer;
	public AudioMixer soundMixer;
	public UnityEvent onPause;
	public UnityEvent onResume;

	public bool isPaused = false;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause() {
        Time.timeScale = 0;
        screen.SetActive(true);
        onPause.Invoke();
    }
    public void Resume() {
		Time.timeScale = 1;
		screen.SetActive(false);
		onResume.Invoke();
    }

    public void SetMusicVolume(float percent) {
        musicMixer.SetFloat("MasterVolume", PercentToDecibels(percent));
    }
    public void SetSoundVolume(float percent) {
		soundMixer.SetFloat("MasterVolume", PercentToDecibels(percent));
	}

    private float PercentToDecibels(float percent) {
		return percent == 0 ? -80 : Mathf.Log10(percent) * 20;
	}

    public void MainMenu() {
		Time.timeScale = 1;
		SceneManager.LoadScene("Scenes/Main Menu");
    }

    public void Quit() {
        Application.Quit();
    }
}
