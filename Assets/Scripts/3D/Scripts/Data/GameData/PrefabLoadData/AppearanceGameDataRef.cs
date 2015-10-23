using UnityEngine;
using System.Collections;
using System.Linq;

public class AppearanceGameDataRef  {

	public static GameObject GetGenderPrefab(AppearanceGenderData gender){
		return Resources.Load<GameObject>(gender.ResourcePath);
	}

	public static Mesh GetMesh<T>(T data)
		where T : AppearanceDataRef
	{
		return data.MeshData.Content;
	}
	
	public static Material GetMaterial<T>(T data)
		where T : AppearanceDataRef
	{
		if(data.MaterialIndex < 0 || data.MaterialIndex >= data.MeshData.materials.Count){
			return data.MeshData.materials.FirstOrDefault().Content;
		}
		else{
			return data.MeshData.materials[data.MaterialIndex].Content;
		}
	}
}
