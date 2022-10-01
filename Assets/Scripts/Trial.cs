using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Trial
{
	public string caseName;
	public Dialogue[] dialogues;
	public Actor defender;
	public Actor plaintiff;
	public bool isGuilty;
	public Dialogue[] dialogueOnInnocent;
	public Dialogue[] dialogueOnGuilty;
}
