using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class NameSO <T> : ScriptableObject{
	public List<string> Tags;
	public T Content;
}
