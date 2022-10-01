using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
	[NonSerialized] public DialogueBubble bubble;

	private void Awake() {
		bubble = GetComponentInChildren<DialogueBubble>();
	}

	public enum Type {
		Defendant,
		Plaintiff,
		Judge,
	}
}
