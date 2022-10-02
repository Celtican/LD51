using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Gavel : MonoBehaviour
{
    public Collider2D innocent;
    public Collider2D guilty;

    public UnityEvent onHitInnocent;
    public UnityEvent onHitGuilty;

    private bool isGrabbed = false;
	private Vector3 startPosition;
    private Vector3 grabbedOffset;
	private new Collider2D collider;

    // Start is called before the first frame update
    void Awake()
    {
        collider = GetComponent<Collider2D>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && collider.OverlapPoint(mousePos)) {
            Grab();
        } else if (isGrabbed && Input.GetMouseButtonUp(0)) {
            Release();
        }

        if (isGrabbed) {
			transform.position = mousePos + grabbedOffset;
		}
    }

    private void Grab() {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		isGrabbed = true;
		grabbedOffset = startPosition - mousePos;
	}

    private void Release() {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		isGrabbed = false;
		transform.position = startPosition;
		if (innocent != null && innocent.OverlapPoint(mousePos)) {
			enabled = false;
			onHitInnocent.Invoke();
		} else if (guilty != null && guilty.OverlapPoint(mousePos)) {
			enabled = false;
			onHitGuilty.Invoke();
		}
	}

    private void OnDisable() {
        transform.position = startPosition;
    }

    public void SwitchScenes(string scene) {
        SceneManager.LoadScene(scene);
    }
}
