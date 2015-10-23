using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MaleShirtMesh : GenericMesh{

	public static readonly NameMeshSO MaleShirt1;
    public static readonly NameMeshSO MaleShirt2;
	
	private static MaleShirtMesh instance;
	new public static MaleShirtMesh Instance{
		get{
			if(instance == null){
				instance = new MaleShirtMesh();
			}
			return instance;
		}
	}
	
	static MaleShirtMesh(){
		MaleShirt1 = Instance.GetResource("MaleChest1");
        MaleShirt2 = Instance.GetResource("MaleChest2");
	}
	
	
	
	
	[ResourcePathMethod]
	new public static IEnumerable<string> GetRecourcePaths() {
		return Instance.MeshList.Select((t) => t.Path);
	}
}
