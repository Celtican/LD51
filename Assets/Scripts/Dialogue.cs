using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{
	[TextArea(1,2)] public string text;
	public Actor.Type actor;
}
