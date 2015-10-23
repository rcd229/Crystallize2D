using UnityEngine;
using System.Collections;

public class EyeDataRef  : AppearanceDataRef{

	public EyeDataRef(NameMeshSO eye) : base(eye){
	}

	public EyeDataRef() : base(){
	}

	public EyeDataRef(NameMeshSO hair, int index) : base(hair, index){
	}
}
