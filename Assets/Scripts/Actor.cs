using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
	public Type type;

	public enum Type {
		Judge,
		Defendant,
		Plaintiff,
	}
}
