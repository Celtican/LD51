using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
	[NonSerialized] public DialogueBubble bubble;

	private void Awake() {
		bubble = GetComponentInChildren<DialogueBubble>();
		print("Added bubble");
	}

	private void Start() {
		print("Starting thing?");
	}

	public enum Type {
		Defendant,
		Plaintiff,
		Judge,
	}
}
