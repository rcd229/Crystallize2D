using UnityEngine;
using System.Collections;

public class ValuedItem : GameMenuItem {

	public bool ShowValue {get;set;}
	public PhraseSequence Text { get; set;}
	public int Value { get; set;}
	public PhraseSequence ValueText{get;set;}
}
