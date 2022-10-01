using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueBubble : MonoBehaviour
{
	public float charactersPerSecond = 5f;

	private TMP_Text textContainer;
	private SpriteRenderer textBack;
	private string targetText = string.Empty;
	private float startTime;
	private bool isTyping = false;

	private void Start() {
		textContainer = GetComponentInChildren<TMP_Text>();
		textBack = GetComponentInChildren<SpriteRenderer>();
		Speak("Why hello there! My name is pablo. I'm pabl. The pablo. Blah blah blah. Text. Text. TEXT TEXTE TEXTH TEH HSDD FSDFG FS I KILLED A MAN. I did. Don't believe me? Heh.");
	}

	private void Update() {
		if (isTyping) {
			int numCharacters = Mathf.CeilToInt((Time.time - startTime) * charactersPerSecond);
			if (numCharacters < targetText.Length) textContainer.text = targetText.Remove(numCharacters);
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
