using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FemaleShortMesh : GenericMesh {
	public static readonly NameMeshSO FemaleLegs1;
    public static readonly NameMeshSO FemaleLegs2;
	
	private static FemaleShortMesh instance;
	new public static FemaleShortMesh Instance{
		get{
			if(instance == null){
				instance = new FemaleShortMesh();
			}
			return instance;
		}
	}
	
	static FemaleShortMesh(){
		FemaleLegs1 = Instance.GetResource("FemaleLegs1");
        FemaleLegs2 = Instance.GetResource("FemaleLegs2");
	}
	
	
	
	
	[ResourcePathMethod]
	new public static IEnumerable<string> GetRecourcePaths() {
		return Instance.MeshList.Select((t) => t.Path);
	}

}
