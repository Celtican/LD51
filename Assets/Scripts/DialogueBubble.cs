using System.Collections;
using System.Collections.Generic;
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
	private bool isTyping = false;

	private void Awake() {
		textContainer = GetComponentInChildren<TMP_Text>();
		textBack = GetComponentInChildren<SpriteRenderer>(true);
	}

	private void Update() {
		if (isTyping) {
			float timeSinceStart = Time.time - startTime;
			int numCharacters = Mathf.CeilToInt((Time.time - startTime) * charactersPerSecond);
			if (numCharacters < targetText.Length) {
				textContainer.text = targetText.Remove(numCharacters);
			} else {
				textContainer.text = targetText;
				if (timeSinceStart - (1 / charactersPerSecond * targetText.Length) > timeDelayAfterTextComplete) {
					StopSpeaking();
					onDialogueComplete.Invoke();
				}
			}
		}
	}

	public void Speak(string text) {
		textBack.enabled = true;
		textContainer.text = string.Empty;
		startTime = Time.time;
		targetText = text;
		isTyping = true;
	}

	public void StopSpeaking() {
		textBack.enabled = false;
		textContainer.text = string.Empty;
		isTyping = false;
	}
}
