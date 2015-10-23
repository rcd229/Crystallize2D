using UnityEngine;
using System.Collections;

public class ValuedMenuNode : MenuNodeLeaf {
	public int Value {get;set;}
	
	public ValuedMenuNode(){
		Value = 0;
	}
}
