using UnityEngine;
using System.Collections;

public class AppearanceDataRef {

	public NameMeshSO MeshData {get; set;}
	//index. Different Mesh will contain a different list.
	public int MaterialIndex {get; set;}

	public AppearanceDataRef(){
		MeshData = null;
		MaterialIndex = 0;
	}
	
	public AppearanceDataRef(NameMeshSO data){
		MeshData = data;
		MaterialIndex = UnityEngine.Random.Range(0, data.materials.Count);
	}
	
	public AppearanceDataRef(NameMeshSO data, int index){
		MeshData = data;
		MaterialIndex = index;
	}

}
