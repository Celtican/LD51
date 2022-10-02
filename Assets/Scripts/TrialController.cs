using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class TrialController : MonoBehaviour
{
	public Docket docket;
	public GameObject bubbleContainer;
	public GameObject plaintiffBubblePrefab;
	public GameObject defendantBubblePrefab;
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
	private DialogueBubble latestBubble;
	private Actor defendant;
	private Actor plaintiff;
	private bool ended = false;
	private bool prosecuted = false;

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
		prosecuted = false;
		allTrials.Remove(trial);
		InstantiateActors();
		StartDialogues(trial.dialogues, false);
		docket.SetDocket(trial.caseName, trial.docket);
	}
	private void InstantiateActors() {
		if (defendant != null) {
			Destroy(defendant.gameObject);
			defendant = null;
		}
		if (plaintiff != null) {
			Destroy(plaintiff.gameObject);
			plaintiff = null;
		}

		if (trial.defendantPrefab != null) {
			defendant = Instantiate(trial.defendantPrefab, defendantPosition, Quaternion.identity, actorContainer.transform).GetComponent<Actor>();
			defendant.onReady.AddListener(() => {
				NextDialogue();
				defendant.sprite.flipX = true;
				onNewTrial.Invoke();
			});
		}
		if (trial.plaintiffPrefab != null) {
			plaintiff = Instantiate(trial.plaintiffPrefab, plaintiffPosition, Quaternion.identity, actorContainer.transform).GetComponent<Actor>();
			if (defendant == null) {
				plaintiff.onReady.AddListener(() => {
					NextDialogue();
					onNewTrial.Invoke();
				});
			}
		}
		if (defendant == null && plaintiff == null) {
			NextDialogue();
			onNewTrial.Invoke();
		}
	}

	private void StartDialogues(Dialogue[] dialogues, bool start) {
		DialogueBubble[] bubbles = FindObjectsOfType<DialogueBubble>();
		foreach (DialogueBubble bubble in bubbles) {
			Destroy(bubble.gameObject);
		}
		this.dialogues = new List<Dialogue>(dialogues);
		if (start) NextDialogue();
	}

	private void NextDialogue() {
		if (plaintiff != null) plaintiff.StopTalking();
		if (defendant != null) defendant.StopTalking();

		if (dialogues.Count > 0) {
			Dialogue dialogue = dialogues[0];
			dialogues.RemoveAt(0);
			GameObject prefab;
			switch (dialogue.actor) {
				case Actor.Type.Judge:
				case Actor.Type.Defendant: prefab = defendantBubblePrefab; defendant.StartTalking(); break;
				case Actor.Type.Plaintiff: prefab = plaintiffBubblePrefab; plaintiff.StartTalking(); break;
				default: throw new Exception("Unknown actor");
			}
			DialogueBubble bubble = Instantiate(prefab, bubbleContainer.transform).GetComponent<DialogueBubble>();
			latestBubble = bubble;
			bubble.onDialogueComplete.AddListener(NextDialogue);
			bubble.Speak(dialogue.text);
		} else {
			onEndDialogue.Invoke();
			if (prosecuted) StartRandomTrial(); // the judge made their decision
		}
	}

	public void ChooseInnocent() {
		if (prosecuted) return;
		prosecuted = true;
		InterruptSpeaking();
		StartDialogues(trial.dialogueOnInnocent, true);
	}

	public void ChooseGuilty() {
		if (prosecuted) return;
		prosecuted = true;
		InterruptSpeaking();
		StartDialogues(trial.dialogueOnGuilty, true);
	}

	public void EndDialogue() {
		ended = true;
		dialogues.Clear();
		InterruptSpeaking();
	}

	public void InterruptSpeaking() {
		if (latestBubble != null) latestBubble.InterruptSpeaking();
	}
}
