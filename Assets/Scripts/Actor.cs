using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
	public SpriteRenderer sprite;

	public enum Type {
		Defendant,
		Plaintiff,
		Judge,
	}
}
