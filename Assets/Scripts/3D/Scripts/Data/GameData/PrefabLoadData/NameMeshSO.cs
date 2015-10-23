using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class NameMeshSO : NameSO<Mesh> {
	public List<NameMaterialSO> materials = new List<NameMaterialSO>();
	[HideInInspector] 
	public string Path;
}
