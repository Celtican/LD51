using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueBubble : MonoBehaviour
{
	public float charactersPerSecond = 5f;
	public float timeDelayAfterTextComplete = 1f;
	public UnityEvent onDialogueComplete;

	private TMP_Text textContainer;
	private SpriteRenderer textBack;
	private string targetText = string.Empty;
	private float startTime;
	private bool isSpeaking = false;
	private bool complete = false;

	private void Awake() {
		textContainer = GetComponentInChildren<TMP_Text>();
		textBack = GetComponentInChildren<SpriteRenderer>(true);
	}

	private void Update() {
		if (isSpeaking) {
			float timeSinceStart = Time.time - startTime;
			int numCharacters = GetNumVisibleCharacters();
			if (numCharacters < targetText.Length) {
				textContainer.text = targetText.Remove(numCharacters);
			} else {
				textContainer.text = targetText;
				if (!complete && timeSinceStart - (1 / charactersPerSecond * targetText.Length) > timeDelayAfterTextComplete) {
					complete = true;
					onDialogueComplete.Invoke();
				} else if (complete && timeSinceStart - (1 / charactersPerSecond * targetText.Length) > timeDelayAfterTextComplete*2) {
					HideBubble();
				}
			}
		}
	}

	public void Speak(string text) {
		textBack.enabled = true;
		textContainer.text = string.Empty;
		startTime = Time.time;
		targetText = text;
		isSpeaking = true;
		complete = false;
	}

	public void InterruptSpeaking() {
		if (isSpeaking && GetNumVisibleCharacters() < targetText.Length) {
			targetText = textContainer.text + "-";
		}
	}

	public void HideBubble() {
		textBack.enabled = false;
		textContainer.text = string.Empty;
		isSpeaking = false;
	}

	private int GetNumVisibleCharacters() {
		return Mathf.CeilToInt((Time.time - startTime) * charactersPerSecond);
	}
}
