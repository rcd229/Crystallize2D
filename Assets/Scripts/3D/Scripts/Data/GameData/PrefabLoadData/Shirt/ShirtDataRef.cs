using UnityEngine;
using System.Collections;

public class ShirtDataRef  : AppearanceDataRef{

	public ShirtDataRef(NameMeshSO shirt) : base(shirt){
	}

	public ShirtDataRef() : base(){
	}

	public ShirtDataRef(NameMeshSO shirt, int index) : base(shirt, index){
	}
}
