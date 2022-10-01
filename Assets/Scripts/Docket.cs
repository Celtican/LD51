using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Docket : MonoBehaviour
{
	public Vector3 raisedPos;
	public TMP_Text title;
	public TMP_Text body;

	private new Collider2D collider;
	private Vector3 startPos;

	private void Start() {
		collider = GetComponent<Collider2D>();
		startPos = transform.position;
	}

	private void Update() {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (collider.OverlapPoint(mousePos)) {
			transform.position = raisedPos;
		} else {
			transform.position = startPos;
		}
	}

	public void SetDocket(string title, string body) {
		this.title.text = title;
		this.body.text = body;
	}
}
