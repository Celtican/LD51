using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    void Start()
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
		if (innocent.OverlapPoint(mousePos)) {
            onHitInnocent.Invoke();
		} else if (guilty.OverlapPoint(mousePos)) {
			onHitGuilty.Invoke();
		}
	}
}
