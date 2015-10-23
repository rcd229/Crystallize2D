using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FemaleShirtMesh : GenericMesh {
	public static readonly NameMeshSO FemaleShirt1;
    public static readonly NameMeshSO FemaleShirt2;

	private static FemaleShirtMesh instance;
	new public static FemaleShirtMesh Instance{
		get{
			if(instance == null){
				instance = new FemaleShirtMesh();
			}
			return instance;
		}
	}
	
	static FemaleShirtMesh(){
		FemaleShirt1 = Instance.GetResource("FemaleChest1");
        FemaleShirt2 = Instance.GetResource("FemaleChest2");
	}
	
	
	
	
	[ResourcePathMethod]
	new public static IEnumerable<string> GetRecourcePaths() {
		return Instance.MeshList.Select((t) => t.Path);
	}
}
