using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trial", menuName = "Kangaroo Court/Trial")]
public class Trial : ScriptableObject
{
	public string caseName;
	public GameObject defendantPrefab;
	public GameObject plaintiffPrefab;
	public bool isGuilty;
	public Dialogue[] dialogues;
	public Dialogue[] dialogueOnInnocent;
	public Dialogue[] dialogueOnGuilty;
	[TextArea(3,20)]
	public string docket;
}
