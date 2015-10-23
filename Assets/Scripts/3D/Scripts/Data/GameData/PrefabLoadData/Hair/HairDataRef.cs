using UnityEngine;
using System.Collections;

public class HairDataRef : AppearanceDataRef {

//	public NameMeshSO Hair {get; set;}
//	//index. Different Hair will contain a different list.
//	public int MaterialIndex {get; set;}
	public HairDataRef() : base(){
	}

	public HairDataRef(NameMeshSO hair) : base(hair){
	}

	public HairDataRef(NameMeshSO hair, int index) : base(hair, index){
	}
}
