using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FemaleBodyMesh : GenericMesh {
	public static readonly NameMeshSO FemaleBody1;
	
	private static FemaleBodyMesh instance;
	new public static FemaleBodyMesh Instance{
		get{
			if(instance == null){
				instance = new FemaleBodyMesh();
			}
			return instance;
		}
	}
	
	static FemaleBodyMesh(){
		FemaleBody1 = Instance.GetResource("FemaleBody");
	}//
	
	
	
	
	[ResourcePathMethod]
	new public static IEnumerable<string> GetRecourcePaths() {
		return Instance.MeshList.Select((t) => t.Path);
	}
}
