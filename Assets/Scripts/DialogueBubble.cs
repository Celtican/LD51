using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueBubble : MonoBehaviour
{
	public float charactersPerSecond = 20f;
	public float timeDelayAfterTextComplete = 1f;
	public UnityEvent onTextFilled;
	public UnityEvent onDialogueComplete;
	public bool isJudge = false;

	private TMP_Text textContainer;
	private Image textBack;
	private string targetText = string.Empty;
	private float startTime;
	private bool isSpeaking = false;
	private bool complete = false;
	private bool interrupted = false;

	private void Awake() {
		textContainer = GetComponentInChildren<TMP_Text>();
		textBack = GetComponentInChildren<Image>(true);
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
					onTextFilled.Invoke();
				} else if (complete && timeSinceStart - (1 / charactersPerSecond * targetText.Length) > timeDelayAfterTextComplete*2) {
					if (isJudge) HideBubble();
					else isSpeaking = false;
					if (!interrupted) onDialogueComplete.Invoke();
				}
			}
		}
	}

	public void Speak(string text) {
		textBack.enabled = true;
		textContainer.text = string.Empty;
		startTime = Time.time;
		targetText = text.Trim();
		isSpeaking = true;
		complete = false;
	}

	public void InterruptSpeaking() {
		interrupted = true;
		if (isSpeaking && GetNumVisibleCharacters() < targetText.Length) {
			targetText = textContainer.text.TrimEnd() + "-";
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
