using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MaleHairMesh : GenericMesh{

	public static readonly NameMeshSO MaleHair1;
	public static readonly NameMeshSO MaleHair2;
	
	private static MaleHairMesh instance;
	new public static MaleHairMesh Instance{
		get{
			if(instance == null){
				instance = new MaleHairMesh();
			}
			return instance;
		}
	}
	
	static MaleHairMesh(){
		MaleHair1 = Instance.GetResource("MaleHair1");
		MaleHair2 = Instance.GetResource("MaleHair2");
	}
	
	
	
	
	[ResourcePathMethod]
	new public static IEnumerable<string> GetResourcePaths() {
		return Instance.MeshList.Select((t) => t.Path);
	}
}
