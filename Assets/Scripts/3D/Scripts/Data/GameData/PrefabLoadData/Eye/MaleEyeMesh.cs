using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MaleEyeMesh : GenericMesh{

//	static int count  = 0;
//	public static int Count{get{return count;}}
//	public static List<NameMeshSO> MaleEyes = new List<NameMeshSO>();
//	public static readonly NameMeshSO MaleEye1 = GetResource("MaleEyes1");
//	public static readonly NameMeshSO MaleEye2 = GetResource("MaleEyes2");
	public static readonly NameMeshSO MaleEye1;

	private static MaleEyeMesh instance;
	new public static MaleEyeMesh Instance{
		get{
			if(instance == null){
				instance = new MaleEyeMesh();
			}
			return instance;
		}
	}

	static MaleEyeMesh(){
		MaleEye1 = Instance.GetResource("MaleEyes");
	}




	[ResourcePathMethod]
	new public static IEnumerable<string> GetRecourcePaths() {
		return Instance.MeshList.Select((t) => t.Path);
	}

//	static NameMeshSO GetResource(string path){
//		NameMeshSO resource = Resources.Load<NameMeshSO>(path);
//		
//		resource.Path = path;
//		MaleEyes.Add(resource);
//		count++;
//		return resource;
//	}
}
