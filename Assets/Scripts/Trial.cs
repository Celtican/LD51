using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trial", menuName = "Kangaroo Court/Trial")]
public class Trial : ScriptableObject
{
	public string caseName;
	public int caseNumber;
	public GameObject defendantPrefab;
	public GameObject plaintiffPrefab;
	public Verdict verdict = Verdict.Innocent;
	public Dialogue[] dialogues;
	public Dialogue[] dialogueOnInnocent;
	public Dialogue[] dialogueOnGuilty;
	public bool showDocket = true;
	public Sprite docketSprite;
	[TextArea(3,20)]
	public string docket;

	public enum Verdict {
		Innocent,
		Guilty,
		Either,
		NoJudgment
	}
}
