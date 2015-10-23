using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;

public class AppearanceLibrary  {


	public static AppearanceGameData GetRandomAppearance(){
		AppearanceGameData gameData = new AppearanceGameData();
		//roll gender
		AppearanceGenderData gender = UnityEngine.Random.Range(0, 2) == 0 ? AppearanceGenderData.Male : AppearanceGenderData.Female;
		gameData.Gender = gender;
		gameData.Hair = GetRandomPart<HairDataRef>(gender, MaleHairMesh.Instance, FemaleHairMesh.Instance);
		gameData.Eye = GetRandomPart<EyeDataRef>(gender, MaleEyeMesh.Instance, FemaleEyeMesh.Instance);
		gameData.Body = GetRandomPart<BodyDataRef>(gender, MaleBodyMesh.Instance, FemaleBodyMesh.Instance);
		gameData.Shirt = GetRandomPart<ShirtDataRef>(gender, MaleShirtMesh.Instance, FemaleShirtMesh.Instance);
		gameData.Short = GetRandomPart<ShortDataRef>(gender, MaleShortMesh.Instance, FemaleShortMesh.Instance);


		return gameData;
	}

	static V GetRandomPart<V> (AppearanceGenderData gender, GenericMesh maleInstance, GenericMesh femaleInstance) 
		where V : AppearanceDataRef, new() 
	{
		int roll;
		NameMeshSO data;
		if (gender == AppearanceGenderData.Male) {
			roll = UnityEngine.Random.Range(0, maleInstance.Count);
			data = maleInstance.MeshList[roll];
		}
		else{
			roll = UnityEngine.Random.Range(0, femaleInstance.Count);
			data = femaleInstance.MeshList[roll];
		}
		V ret = new V();
		ret.MaterialIndex = UnityEngine.Random.Range (0, data.materials.Count);
		ret.MeshData = data;
		return ret;
	}

	//get a random mesh of particular kind specified by a tag 
	static V GetRandomPartWithTag<V> (AppearanceGenderData gender, GenericMesh maleInstance, GenericMesh femaleInstance, string tag) 
		where V : AppearanceDataRef, new() 
	{
		if(tag == "")
			return GetRandomPart<V>(gender, maleInstance, femaleInstance);

		int roll;
		NameMeshSO data;
		List<NameMeshSO> meshList;
		meshList = new List<NameMeshSO>(gender == AppearanceGenderData.Female ? femaleInstance.MeshList : maleInstance.MeshList);
		//slow
		var selectedMeshList = (from meshData in meshList
								where meshData.Tags.Contains(tag) || meshData.materials.Any(e => e.Tags.Contains(tag))
								select meshData).ToList();

		roll = UnityEngine.Random.Range(0, selectedMeshList.Count);
		data = selectedMeshList[roll];
		V ret = new V();
		ret.MeshData = data;
		//mesh has tag, choose a random material
		if(data.Tags.Contains(tag)){
			ret.MaterialIndex = UnityEngine.Random.Range (0, data.materials.Count);
		}
		//choose from materials with the tag
		else{
			var validMaterials = (from m in data.materials
									where m.Tags.Contains(tag)
									select m).ToList();
			int tempIndex = UnityEngine.Random.Range(0, validMaterials.Count);
			ret.MaterialIndex = data.materials.IndexOf(validMaterials[tempIndex]);
		}

		return ret;
	}

	//random mesh without a tag
	static V GetRandomPartWithoutTag<V> (AppearanceGenderData gender, GenericMesh maleInstance, GenericMesh femaleInstance, params string[] tags) 
		where V : AppearanceDataRef, new() 
	{
		if(tags.Length == 0)
			return GetRandomPart<V>(gender, maleInstance, femaleInstance);

		int roll;
		NameMeshSO data;
		List<NameMeshSO> meshList;
		meshList = new List<NameMeshSO>(gender == AppearanceGenderData.Female ? femaleInstance.MeshList : maleInstance.MeshList);
		//slow
		//a mesh can be chosen if it dosent have the tag itself and not all its materials have the tag
		var selectedMeshList = (from meshData in meshList
		                        where !meshData.Tags.Any((t) => tags.Contains(t)) && !meshData.materials.All(e => e.Tags.Any((t) => tags.Contains(t)))
		                        select meshData).ToList();
		roll = UnityEngine.Random.Range(0, selectedMeshList.Count);
		data = selectedMeshList[roll];
		V ret = new V();
		ret.MeshData = data;
		//make sure to choose the material without the tag
		var validMaterials = (from m in data.materials
		                      where !m.Tags.Any((t) => tags.Contains(t)) //.Contains(tag)
		                      select m).ToList();
		int tempIndex = UnityEngine.Random.Range(0, validMaterials.Count);
		ret.MaterialIndex = data.materials.IndexOf(validMaterials[tempIndex]);
		return ret;
	}


	
	public static GameObject CreateObject(AppearanceGameData data){
		//load avatar directly if it is a preset avatar
		if(data is AppearanceGameDataPreset){
			AppearanceGameDataPreset castData = (AppearanceGameDataPreset) data;
//			Debug.Log(castData.ResourcePath);
			return GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(castData.ResourcePath));
		}
		else{
			//TODO lots of logic to create an avatar from appearance game data
			GameObject avatar;
			//gender TODO if multiple attributes then this should only sets one of them instead of creating the entire avatar
            avatar = GameObject.Instantiate<GameObject>(AppearanceGameDataRef.GetGenderPrefab(data.Gender));
			//hair
			replaceComponent(avatar, "Hair", data.Hair);
			//eye
			replaceComponent(avatar, "Eyes", data.Eye);
			//body
			replaceComponent(avatar, "Body", data.Body);
			//shirt
			replaceComponent(avatar, "Shirt", data.Shirt);
			//shorts
			replaceComponent(avatar, "Shorts", data.Short);
			//collect tags
			List<string> tags = new List<string>();
			tags.Add(data.Gender == AppearanceGenderData.Male ? "Male" : "Female");
			tags.AddRange(CollectTags(data.Hair));
			tags.AddRange(CollectTags(data.Eye));
			tags.AddRange(CollectTags(data.Body));
			tags.AddRange(CollectTags(data.Shirt));
			tags.AddRange(CollectTags(data.Short));
			//add tag to the avatar
			avatar.AddComponent<AppearanceTags>().AddTagList(tags);
			return avatar;
		}
	}

	//TODO a gender must be specified now. IF dont want this, need to look up both gender's data and see which one contains all the tags
	public static AppearanceGameData RandomAppearanceWithTag(
		string genderTag, 
		string hairTag = "", 
		string eyeTag = "", 
		string bodyTag = "", 
		string shirtTag = "", 
		string shortTag = "")
	{
        AppearanceGenderData gender;
        if (genderTag == "Male") {
            gender = AppearanceGenderData.Male;
        } else if (genderTag == "Female") {
            gender = AppearanceGenderData.Female;
        } else {
            if (UnityEngine.Random.value > 0.5f) {
                gender = AppearanceGenderData.Male;
            } else {
                gender = AppearanceGenderData.Female;
            }
        }
		AppearanceGameData gameData = new AppearanceGameData();
		gameData.Gender = gender;
		gameData.Hair = GetRandomPartWithTag<HairDataRef>(gender, MaleHairMesh.Instance, FemaleHairMesh.Instance, hairTag);
		gameData.Eye = GetRandomPartWithTag<EyeDataRef>(gender, MaleEyeMesh.Instance, FemaleEyeMesh.Instance, eyeTag);
		gameData.Body = GetRandomPartWithTag<BodyDataRef>(gender, MaleBodyMesh.Instance, FemaleBodyMesh.Instance, bodyTag);
		gameData.Shirt = GetRandomPartWithTag<ShirtDataRef>(gender, MaleShirtMesh.Instance, MaleShirtMesh.Instance, shirtTag);
		gameData.Short = GetRandomPartWithTag<ShortDataRef>(gender, MaleShortMesh.Instance, FemaleShortMesh.Instance, shortTag);

		return gameData;
	}

    public static AppearanceGameData GetRandomAppearanceWithoutTag(params string[] tags) {
        AppearanceGenderData gender;
        if (tags.Contains("Male")) {
            gender = AppearanceGenderData.Male;
        } else if (tags.Contains("Female")) {
            gender = AppearanceGenderData.Female;
        } else {
            if(UnityEngine.Random.value > 0.5f){
                gender = AppearanceGenderData.Male;
            } else {
                gender = AppearanceGenderData.Female;
            }
        }
        AppearanceGameData gameData = new AppearanceGameData();
        gameData.Gender = gender;
        gameData.Hair = GetRandomPartWithoutTag<HairDataRef>(gender, MaleHairMesh.Instance, FemaleHairMesh.Instance, tags);
        gameData.Eye = GetRandomPartWithoutTag<EyeDataRef>(gender, MaleEyeMesh.Instance, FemaleEyeMesh.Instance, tags);
        gameData.Body = GetRandomPartWithoutTag<BodyDataRef>(gender, MaleBodyMesh.Instance, FemaleBodyMesh.Instance, tags);
        gameData.Shirt = GetRandomPartWithoutTag<ShirtDataRef>(gender, MaleShirtMesh.Instance, MaleShirtMesh.Instance, tags);
        gameData.Short = GetRandomPartWithoutTag<ShortDataRef>(gender, MaleShortMesh.Instance, FemaleShortMesh.Instance, tags);

        return gameData;
    }

	public static AppearanceGameData RandomAppearanceWithoutTag(
		string genderTag, 
		string hairTag = "", 
		string eyeTag = "", 
		string bodyTag = "", 
		string shirtTag = "", 
		string shortTag = "")
	{
        AppearanceGenderData gender;
        //Debug.Log("gender is: " + genderTag);
        if (genderTag == "Male") {
            gender = AppearanceGenderData.Female;
        } else if (genderTag == "Female") {
            gender = AppearanceGenderData.Male;
        } else {
            if (UnityEngine.Random.value > 0.5f) {
                gender = AppearanceGenderData.Male;
            } else {
                gender = AppearanceGenderData.Female;
            }
        }
		AppearanceGameData gameData = new AppearanceGameData();
		gameData.Gender = gender;
		gameData.Hair = GetRandomPartWithoutTag<HairDataRef>(gender, MaleHairMesh.Instance, FemaleHairMesh.Instance, hairTag);
		gameData.Eye = GetRandomPartWithoutTag<EyeDataRef>(gender, MaleEyeMesh.Instance, FemaleEyeMesh.Instance, eyeTag);
		gameData.Body = GetRandomPartWithoutTag<BodyDataRef>(gender, MaleBodyMesh.Instance, FemaleBodyMesh.Instance, bodyTag);
		gameData.Shirt = GetRandomPartWithoutTag<ShirtDataRef>(gender, MaleShirtMesh.Instance, MaleShirtMesh.Instance, shirtTag);
		gameData.Short = GetRandomPartWithoutTag<ShortDataRef>(gender, MaleShortMesh.Instance, FemaleShortMesh.Instance, shortTag);
		
		return gameData;
	}

	public static AppearanceGameData CreateRandomWithoutTag(string genderTag, string hairTag = "", string bodyTag = "", string shirtTag = "", string shortTag = ""){
		return null;
	}

	static void replaceComponent(GameObject avatar, string partName, AppearanceDataRef dataRef)
	{
		var part = avatar.transform.Find(partName);
		var renderer = part.gameObject.GetComponent<SkinnedMeshRenderer>();
		renderer.material = AppearanceGameDataRef.GetMaterial(dataRef);
		renderer.sharedMesh = AppearanceGameDataRef.GetMesh(dataRef);
	}

	static List<string> CollectTags(AppearanceDataRef component){
		List<string> tags = new List<string>(component.MeshData.Tags);
		tags.AddRange(component.MeshData.materials[component.MaterialIndex].Tags);
		return tags;
	}
}
