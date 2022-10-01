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

	private TMP_Text textContainer;
	private float curTime;

	private void Awake() {
		textContainer = GetComponent<TMP_Text>();
		curTime = maxTime;
	}

	private void Update() {
		curTime -= Time.deltaTime;
		textContainer.text = Mathf.CeilToInt(curTime).ToString();
		if (curTime <= 0) {
			enabled = false;
			onTimeOver.Invoke();
		}
	}

	private void OnDisable() {
		if (curTime > 0) onPause.Invoke(curTime);
	}

	public void RestartTimer() {
		enabled = true;
		curTime = maxTime;
	}

	public void AddTime(float time) {
		curTime = Mathf.Clamp(curTime + time, 0, maxTime);
		textContainer.text = Mathf.CeilToInt(curTime).ToString();
	}
}
