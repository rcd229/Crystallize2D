using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FemaleHairMesh : GenericMesh {
	public static readonly NameMeshSO FemaleHair1;
	public static readonly NameMeshSO FemaleHair2;
	
	private static FemaleHairMesh instance;
	new public static FemaleHairMesh Instance{
		get{
			if(instance == null){
				instance = new FemaleHairMesh();
			}
			return instance;
		}
	}
	
	static FemaleHairMesh(){
		FemaleHair1 = Instance.GetResource("FemaleHair1");
		FemaleHair2 = Instance.GetResource("FemaleHair2");
	}
	
	
	
	
	[ResourcePathMethod]
	new public static IEnumerable<string> GetRecourcePaths() {
		return Instance.MeshList.Select((t) => t.Path);
	}
}
