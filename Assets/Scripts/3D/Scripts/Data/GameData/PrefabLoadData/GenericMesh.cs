using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GenericMesh {

	private static GenericMesh instance;
	public static GenericMesh Instance{
		get{
			if(instance == null){
				instance = new GenericMesh();
			}
			return instance;
		}
	}

	int count  = 0;
	public int Count{get{return count;}}
	public List<NameMeshSO> MeshList = new List<NameMeshSO>();
		

	[ResourcePathMethod]
	public static IEnumerable<string> GetRecourcePaths() {
		return Instance.MeshList.Select((t) => t.Path);
	}
	
	protected NameMeshSO GetResource(string path){
		NameMeshSO resource = Resources.Load<NameMeshSO>(path);
        if (resource == null) {
            Debug.LogError("Resource not found at " + path + "; " + GetType());
			return null;
        }
		else{

			resource.Path = path;
			MeshList.Add(resource);

			count++;
			return resource;
		}
	}
}
