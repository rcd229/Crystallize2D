using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionMenuInput : MonoBehaviour {

	public List<GameMenuItem> ItemList { get; set;}
	public string Prefab;

	public SelectionMenuInput(List<GameMenuItem> lst, string path){
		ItemList = lst;
		Prefab = path;
	}

}
