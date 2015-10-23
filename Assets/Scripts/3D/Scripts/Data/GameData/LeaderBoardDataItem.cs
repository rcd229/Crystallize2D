using UnityEngine;
using System.Collections;
using System;

public class LeaderBoardDataItem<T> {

	public string Name{get; set;}

	public T Data{get;set;}

	public LeaderBoardDataItem(string name, T data){
		Name = name;
		Data = data;
	}

	public LeaderBoardDataItem(){
		Name = "";
		Data = default(T);
	}
}
