using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimerController : MonoBehaviour
{
	public float maxTime = 10;
	public UnityEvent onTimeOver;
	public UnityEvent<float> onPause;
	public UnityEvent onTick;
	public UnityEvent onTock;
	public Animator hourglassAnimator;
	public GameObject addedTimePrefab;

	private TMP_Text textContainer;
	private Animator animator;
	private float curTime;

	private void Awake() {
		animator = GetComponent<Animator>();
		textContainer = GetComponent<TMP_Text>();
		curTime = maxTime;
	}

	private void Update() {
		float newTime = curTime - Time.deltaTime;
		if (newTime < curTime) {
			if (newTime < 5) {
				if (((int)newTime) != ((int)curTime)) {
					animator.SetTrigger("Pulse");
					onTock.Invoke();
					hourglassAnimator.SetTrigger("Flip");
				} else if ((newTime % 1) < 0.5f && (curTime % 1) > 0.5f) {
					animator.SetTrigger("Pulse");
					onTick.Invoke();
				}
			} else if (((int)newTime) != ((int)curTime)) {
				hourglassAnimator.SetTrigger("Flip");
				if (((int)newTime)%2 == 0) {
					animator.SetTrigger("Pulse");
					onTock.Invoke();
				} else {
					animator.SetTrigger("Pulse");
					onTick.Invoke();
				}
			}
		}
		curTime = newTime;
		textContainer.text = Mathf.CeilToInt(curTime).ToString();
		if (curTime <= 0) {
			enabled = false;
			onTimeOver.Invoke();
		}
	}

	private void OnDisable() {
		if (curTime > 0) onPause.Invoke(curTime);
		animator.SetTrigger("Hide");
	}

	public void RestartTimer() {
		enabled = true;
		curTime = maxTime;
	}

	public float GetTimeLeft() {
		return curTime;
	}

	public void AddTime(float time) {
		curTime = Mathf.Clamp(curTime + time, 0, maxTime);
		textContainer.text = Mathf.CeilToInt(curTime).ToString();
		if (addedTimePrefab != null) {
			GameObject addedTime = Instantiate(addedTimePrefab, transform.parent);
			TMP_Text textContainer = addedTime.GetComponentInChildren<TMP_Text>();
			if (time > 0) {
				if (curTime < maxTime) {
					textContainer.text = "+" + (int)time;
					textContainer.color = Color.green;
				}
			} else {
				textContainer.text = ((int)time).ToString();
				textContainer.color = Color.red;
			}
		}
	}

	public void LoseTime(float time) {
		AddTime(-time);
	}
}
