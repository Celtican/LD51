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
	public GameObject judgeBubble;
	public GameObject actorContainer;
	public Vector3 defendantPosition;
	public Vector3 plaintiffPosition;
	public Trial debugTrial;
	public UnityEvent onNewTrial;
	public UnityEvent onEndTrial;
	public UnityEvent onEndDialogue;
	public UnityEvent onWin;

	private List<Trial> trials;
	private Trial trial;
	private List<Dialogue> dialogues;
	private DialogueBubble latestBubble;
	private Actor defendant;
	private Actor plaintiff;
	private bool ended = false;
	private bool prosecuted = false;

	private void Start() {
		List<Trial> allTrials = new List<Trial>(Resources.LoadAll<Trial>("Trials"));
		List<Trial> specificTrials = new List<Trial>();
		for (int i = allTrials.Count-1; i >= 0; i--) {
			if (allTrials[i].caseNumber > 0) {
				for (int j = specificTrials.Count - 1; j >= 0; j--) {
					if (specificTrials[j].caseNumber == allTrials[i].caseNumber) {
						throw new Exception("Two trials have the same case number. Case numbers above 0 must be unique.");
					}
				}
				specificTrials.Add(allTrials[i]);
				allTrials.RemoveAt(i);
			} else if (allTrials[i].caseNumber < 0) {
				allTrials.RemoveAt(i);
			}
		}
		trials = new List<Trial>();
		while (allTrials.Count > 0 || specificTrials.Count > 0) {
			for (int i = specificTrials.Count - 1; i >= 0; i--) {
				if (specificTrials[i].caseNumber == trials.Count+1) {
					trials.Add(specificTrials[i]);
					specificTrials.RemoveAt(i);
				}
			}
			if (allTrials.Count > 0) {
				int r = Random.Range(0, allTrials.Count);
				trials.Add(allTrials[r]);
				allTrials.RemoveAt(r);
			}
		}

		if (debugTrial != null) StartTrial(debugTrial);
		else StartNextTrial();
	}

	public void StartNextTrial() {
		if (ended) return;
		if (trials.Count == 0) {
			onWin.Invoke();
			return;
		}
		StartTrial(trials[0]);
		trials.RemoveAt(0);
	}
	public void StartTrial(Trial trial) {
		if (ended) return;
		onEndTrial.Invoke();
		this.trial = trial;
		prosecuted = false;
		if (defendant != null || plaintiff != null) {
			if (defendant != null) {
				defendant.onExit.AddListener(() => {
					InstantiateActors();
					StartDialogues(trial.dialogues, false);
				});
			} else if (plaintiff != null) {
				plaintiff.onExit.AddListener(() => {
					InstantiateActors();
					StartDialogues(trial.dialogues, false);
				});
			}
			ExitActors();
		} else {
			InstantiateActors();
			StartDialogues(trial.dialogues, false);
		}
		if (trial.showDocket) docket.SetDocket(trial.caseName, trial.docket);
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

	private void ExitActors() {
		if (defendant != null) defendant.Exit();
		if (plaintiff != null) plaintiff.Exit();
		DialogueBubble[] bubbles = FindObjectsOfType<DialogueBubble>();
		foreach (DialogueBubble bubble in bubbles) {
			bubble.HideBubble();
		}
	}

	private void StartDialogues(Dialogue[] dialogues, bool start) {
		DialogueBubble[] bubbles = FindObjectsOfType<DialogueBubble>();
		foreach (DialogueBubble bubble in bubbles) {
			if (!bubble.isJudge) Destroy(bubble.gameObject);
		}
		this.dialogues = new List<Dialogue>(dialogues);
		if (start) NextDialogue();
	}

	public void NextDialogue() {
		if (plaintiff != null) plaintiff.StopTalking();
		if (defendant != null) defendant.StopTalking();

		if (dialogues.Count > 0) {
			Dialogue dialogue = dialogues[0];
			dialogues.RemoveAt(0);
			GameObject prefab;
			switch (dialogue.actor) {
				case Actor.Type.Judge:
					prefab = null;
					judgeBubble.GetComponent<DialogueBubble>().Speak(dialogue.text);
					break;
				case Actor.Type.Defendant:
					prefab = defendantBubblePrefab;
					defendant.StartTalking();
					break;
				case Actor.Type.Plaintiff:
					prefab = plaintiffBubblePrefab;
					plaintiff.StartTalking();
					break;
				default: throw new Exception("Unknown actor");
			}
			if (prefab != null) {
				DialogueBubble bubble = Instantiate(prefab, bubbleContainer.transform).GetComponent<DialogueBubble>();
				latestBubble = bubble;
				bubble.onDialogueComplete.AddListener(NextDialogue);
				bubble.onTextFilled.AddListener(StopSpeaking);
				bubble.Speak(dialogue.text);
			}
		} else {
			onEndDialogue.Invoke();
			if (prosecuted || trial.verdict == Trial.Verdict.NoJudgment) StartNextTrial();
		}
	}

	public void ChooseInnocent() {
		if (prosecuted) return;
		prosecuted = true;
		InterruptSpeaking();
		docket.Dispose();
		StartDialogues(trial.dialogueOnInnocent, true);
	}

	public void ChooseGuilty() {
		if (prosecuted) return;
		prosecuted = true;
		InterruptSpeaking();
		docket.Dispose();
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
	public void StopSpeaking() {
		if (plaintiff != null) plaintiff.StopTalking();
		if (defendant != null) defendant.StopTalking();
	}
}
