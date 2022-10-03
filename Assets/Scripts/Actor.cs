using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Actor : MonoBehaviour
{
	public SpriteRenderer sprite;
	public UnityEvent onReady;
	public UnityEvent onExit;
	public AudioClip themeSong;

	private AudioClip previousSong;
	private Animator animator;

	private void Awake() {
		animator = GetComponent<Animator>();
	}

	private void Start() {
		if (themeSong != null) {
			AudioSource music = GetMusicSource();
			previousSong = music.clip;
			music.Stop();
			music.clip = themeSong;
			music.Play();
		}
	}

	private AudioSource GetMusicSource() {
		return GameObject.Find("Music").GetComponent<AudioSource>();
	}

	public void CallOnReady() {
		onReady.Invoke();
	}
	public void CallOnExit() {
		onExit.Invoke();
		if (previousSong != null) {
			AudioSource music = GetMusicSource();
			music.Stop();
			music.clip = previousSong;
			music.Play();
		}
	}

	public void StartTalking() {
		animator.SetBool("IsTalking", true);
	}
	public void StopTalking() {
		animator.SetBool("IsTalking", false);
	}

	public void Exit() {
		animator.SetTrigger("Exit");
	}

	public enum Type {
		Defendant,
		Plaintiff,
		Judge,
	}
}
