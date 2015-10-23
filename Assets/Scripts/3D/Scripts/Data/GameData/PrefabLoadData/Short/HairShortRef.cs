using UnityEngine;
using System.Collections;

public class ShortDataRef : AppearanceDataRef {
	
	public ShortDataRef(NameMeshSO shorts) : base(shorts){
	}

	public ShortDataRef() : base(){
	}

	public ShortDataRef(NameMeshSO shorts, int index) : base(shorts, index){
	}
}
