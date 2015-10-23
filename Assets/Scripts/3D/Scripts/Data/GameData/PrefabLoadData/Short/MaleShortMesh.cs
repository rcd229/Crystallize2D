using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MaleShortMesh : GenericMesh{

	public static readonly NameMeshSO MaleLegs1;
    public static readonly NameMeshSO MaleLegs2;

	private static MaleShortMesh instance;
	new public static MaleShortMesh Instance{
		get{
			if(instance == null){
				instance = new MaleShortMesh();
			}
			return instance;
		}
	}
	
	static MaleShortMesh(){
		MaleLegs1 = Instance.GetResource("MaleLegs1");
        MaleLegs1 = Instance.GetResource("MaleLegs2");
	}
	
	
	
	
	[ResourcePathMethod]
	new public static IEnumerable<string> GetRecourcePaths() {
		return Instance.MeshList.Select((t) => t.Path);
	}
}
