using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Actor : MonoBehaviour
{
	public SpriteRenderer sprite;
	public UnityEvent onReady;

	private Animator animator;

	private void Awake() {
		animator = GetComponent<Animator>();
	}

	public void CallOnReady() {
		onReady.Invoke();
	}

	public void StartTalking() {
		animator.SetBool("IsTalking", true);
	}
	public void StopTalking() {
		animator.SetBool("IsTalking", false);
	}

	public enum Type {
		Defendant,
		Plaintiff,
		Judge,
	}
}
