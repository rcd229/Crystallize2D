using UnityEngine;
using System.Collections;

public class BodyDataRef  : AppearanceDataRef{

	public BodyDataRef(NameMeshSO body) : base(body){
	}

	public BodyDataRef() : base(){
	}

	public BodyDataRef(NameMeshSO body, int index) : base(body, index){
	}
}
