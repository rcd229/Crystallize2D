using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class AppearanceGenderData  {

	string name;

	public string ResourcePath{
		get{
			return "Avatar/" + name;
		}
	}

	public static readonly AppearanceGenderData Male = new AppearanceGenderData("MaleBase");
	public static readonly AppearanceGenderData Female = new AppearanceGenderData("FemaleBase");

	public AppearanceGenderData(string name){
		this.name = name;
		AddRequiredResource(ResourcePath);
	}

	[ResourcePathMethod]
	public static IEnumerable<string> GetRecourcePaths() {
		return new List<string> (){Male.ResourcePath, Female.ResourcePath};
	}

	void AddRequiredResource(string path) {
		GameDataInitializer.AddRequiredResource(path);
	}
}
