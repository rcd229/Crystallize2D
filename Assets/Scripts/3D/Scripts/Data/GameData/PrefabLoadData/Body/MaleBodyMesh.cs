using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MaleBodyMesh : GenericMesh {
	public static readonly NameMeshSO MaleBody1;
	
	private static MaleBodyMesh instance;
	new public static MaleBodyMesh Instance{
		get{
			if(instance == null){
				instance = new MaleBodyMesh();
			}
			return instance;
		}
	}
	
	static MaleBodyMesh(){
		MaleBody1 = Instance.GetResource("MaleBody");
	}
	
	
	
	
	[ResourcePathMethod]
	new public static IEnumerable<string> GetRecourcePaths() {
		return Instance.MeshList.Select((t) => t.Path);
	}
}
