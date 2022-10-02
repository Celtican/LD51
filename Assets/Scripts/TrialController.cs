using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class TrialController : MonoBehaviour
{
	public Gavel gavel;
	public Docket docket;
	public GameObject actorContainer;
	public Vector3 defendantPosition;
	public Vector3 plaintiffPosition;
	public Trial debugTrial;
	public UnityEvent onNewTrial;
	public UnityEvent onEndDialogue;
	public UnityEvent onWin;

	private List<Trial> allTrials;
	private Trial trial;
	private List<Dialogue> dialogues;
	private Actor defendant;
	private Actor plaintiff;
	private bool ended = false;

	private void Start() {
		allTrials = new List<Trial>(Resources.LoadAll<Trial>("Trials"));
		if (debugTrial != null) StartTrial(debugTrial);
		else StartRandomTrial();
	}

	public void StartRandomTrial() {
		if (ended) return;
		if (allTrials.Count == 0) onWin.Invoke();
		else StartTrial(allTrials[Random.Range(0, allTrials.Count)]);
	}
	public void StartTrial(Trial trial) {
		if (ended) return;
		this.trial = trial;
		allTrials.Remove(trial);
		InstantiateActors();
		StartDialogues(trial.dialogues);
		gavel.enabled = true;
		onNewTrial.Invoke();
		docket.SetDocket(trial.caseName, trial.docket);
	}
	private void InstantiateActors() {
		if (defendant != null) Destroy(defendant.gameObject);
		if (plaintiff != null) Destroy(plaintiff.gameObject);

		if (trial.defendantPrefab != null) {
			defendant = Instantiate(trial.defendantPrefab, defendantPosition, Quaternion.identity, actorContainer.transform).GetComponent<Actor>();
			defendant.bubble.onDialogueComplete.AddListener(NextDialogue);
		}
		if (trial.plaintiffPrefab != null) {
			plaintiff = Instantiate(trial.plaintiffPrefab, plaintiffPosition, Quaternion.identity, actorContainer.transform).GetComponent<Actor>();
			plaintiff.bubble.onDialogueComplete.AddListener(NextDialogue);
		}
	}

	private void StartDialogues(Dialogue[] dialogues) {
		this.dialogues = new List<Dialogue>(dialogues);
		NextDialogue();
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
		} else {
			onEndDialogue.Invoke();
			if (!gavel.enabled) StartRandomTrial(); // the judge made their decision
		}
	}

	public void ChooseInnocent() {
		InterruptSpeaking();
		StartDialogues(trial.dialogueOnInnocent);
	}

	public void ChooseGuilty() {
		InterruptSpeaking();
		StartDialogues(trial.dialogueOnGuilty);
	}

	public void EndDialogue() {
		ended = true;
		dialogues.Clear();
		InterruptSpeaking();
	}

	public void InterruptSpeaking() {
		if (defendant != null) defendant.bubble.InterruptSpeaking();
		if (plaintiff != null) plaintiff.bubble.InterruptSpeaking();
	}
}
