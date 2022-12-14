using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class Docket : MonoBehaviour
{
	public TMP_Text title;
	public TMP_Text body;
	public bool obtain;
	public AudioRandomizer liftSound;
	public SpriteRenderer specialSprite;

	private new Collider2D collider;
	private Animator animator;
	private new AudioRandomizer audio;

	private void Awake() {
		collider = GetComponent<Collider2D>();
		animator = GetComponent<Animator>();
		audio = GetComponent<AudioRandomizer>();
	}
	private void Start() {
		if (obtain) animator.SetTrigger("Obtain");
	}

	private void Update() {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		animator.SetBool("IsRisen", collider.OverlapPoint(mousePos));
	}

	public void SetDocket(string title, string body, Sprite sprite) {
		animator.SetTrigger("Obtain");
		this.title.text = title;
		this.body.text = body;
		PlayLiftSound();
		specialSprite.sprite = sprite;
	}
	public void Dispose() {
		animator.SetTrigger("Dispose");
		audio.RandomPlay();
	}

	public void PlayLiftSound() {
		liftSound.RandomPlay();
	}

	private void OnDisable() {
		animator.SetBool("IsRisen", false);
	}
}
