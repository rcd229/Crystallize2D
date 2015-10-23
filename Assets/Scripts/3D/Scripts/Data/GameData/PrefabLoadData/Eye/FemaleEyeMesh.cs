using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FemaleEyeMesh : GenericMesh{
	public static readonly NameMeshSO FemaleEye1;
	
	private static FemaleEyeMesh instance;
	new public static FemaleEyeMesh Instance{
		get{
			if(instance == null){
				instance = new FemaleEyeMesh();
			}
			return instance;
		}
	}
	
	static FemaleEyeMesh(){
		FemaleEye1 = Instance.GetResource("FemaleEyes");
	}
	
	[ResourcePathMethod]
	new public static IEnumerable<string> GetRecourcePaths() {
		return Instance.MeshList.Select((t) => t.Path);
	}
}
