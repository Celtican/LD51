using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialController : MonoBehaviour
{
	public GameObject actorContainer;
	public Vector3 defendantPosition;
	public Vector3 plaintiffPosition;

	public Trial trial;
	private List<Dialogue> dialogues;
	private Actor defendant;
	private Actor plaintiff;

	private void Start() {
		StartCase(trial);
	}

	public void StartCase(Trial trial) {
		this.trial = trial;
		InstantiateActors();
		dialogues = new List<Dialogue>(trial.dialogues);
		NextDialogue();
	}
	private void InstantiateActors() {
		if (defendant != null) Destroy(defendant.gameObject);
		if (plaintiff != null) Destroy(plaintiff.gameObject);
		defendant = Instantiate(trial.defendantPrefab, defendantPosition, Quaternion.identity, actorContainer.transform).GetComponent<Actor>();
		plaintiff = Instantiate(trial.plaintiffPrefab, plaintiffPosition, Quaternion.identity, actorContainer.transform).GetComponent<Actor>();
		defendant.bubble.onDialogueComplete.AddListener(NextDialogue);
		plaintiff.bubble.onDialogueComplete.AddListener(NextDialogue);
	}

	private void NextDialogue() {
		if (dialogues.Count > 0) {
			Dialogue dialogue = dialogues[0];
			dialogues.RemoveAt(0);
			switch (dialogue.actor) {
				case Actor.Type.Defendant: defendant.bubble.Speak(dialogue.text); break;
				case Actor.Type.Plaintiff: plaintiff.bubble.Speak(dialogue.text); break;
				case Actor.Type.Judge: defendant.bubble.Speak("Judge is not yet implemented"); break;
				default: throw new Exception("Unknown actor");
			}
		}
	}
}
