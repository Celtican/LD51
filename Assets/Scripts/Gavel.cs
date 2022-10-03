using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

public class Gavel : MonoBehaviour
{
    public Collider2D innocent;
    public Collider2D guilty;

    public float lowestY = 0.15f;
    public float highestY = 4;
    public float lowestAngle = 20;
    public float highestAngle = -26;
    private float defaultAngle;

    public ParticleSystem wackParticles;
    public float maximumWackHeight = -3;
    public float wackTime = 0.2f;
    public float wackHeight = 0.5f;
    private float loweredTime;
    private float loweredHeight;
    private Vector3 loweredHeadPosition;

    public GameObject head;

    public UnityEvent onHitInnocent;
    public UnityEvent onHitGuilty;
    public UnityEvent onWack;

    private bool isGrabbed = false;
	private Vector3 startPosition;
	private new Collider2D collider;

    // Start is called before the first frame update
    void Awake()
    {
        collider = GetComponent<Collider2D>();
        startPosition = transform.position;
        defaultAngle = transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        if (Input.GetMouseButtonDown(0) && collider.OverlapPoint(mousePos)) {
            Grab();
        } else if (isGrabbed && Input.GetMouseButtonUp(0)) {
            Release();
        }

        if (isGrabbed) {
            Vector3 curPosition = transform.position;
            if (mousePos.y < curPosition.y && mousePos.y < maximumWackHeight) {
                loweredTime = Time.time;
                loweredHeight = mousePos.y;
                loweredHeadPosition = head.transform.position;
			} else if ((mousePos.y > curPosition.y) &&
                    (Time.time - loweredTime < wackTime) &&
                    (mousePos.y > loweredHeight+wackHeight)) {
                Wack();
            }
			transform.position = mousePos;

			Vector3 angle = transform.eulerAngles;
            float yPos = Mathf.Clamp(transform.localPosition.y, lowestY, highestY);
            float percentY = (yPos - lowestY)/(highestY-lowestY);
            float angleDif = highestAngle - lowestAngle;
            angle.z = lowestAngle + angleDif * percentY;
            transform.eulerAngles = angle;
		}
    }

    private void Grab() {
		isGrabbed = true;
	}

    private void Wack() {
		loweredTime = 0;
		onWack.Invoke();
        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        emitParams.position = loweredHeadPosition;
        wackParticles.Emit(emitParams, 1);
		if (innocent != null && innocent.OverlapPoint(loweredHeadPosition)) {
			onHitInnocent.Invoke();
		} else if (guilty != null && guilty.OverlapPoint(loweredHeadPosition)) {
			onHitGuilty.Invoke();
		}
	}

    public void Release() {
		isGrabbed = false;
		transform.position = startPosition;
        transform.eulerAngles = new Vector3(0, 0, defaultAngle);
	}

    private void OnDisable() {
        Release();
    }

    public void SwitchScenes(string scene) {
        SceneManager.LoadScene(scene);
    }
}
