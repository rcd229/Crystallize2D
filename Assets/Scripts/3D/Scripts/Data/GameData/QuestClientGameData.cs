using UnityEngine;
using System.Collections;

public class QuestClientGameData : ISerializableDictionaryItem<int> {

	public int Key { 
		get {
			return ClientID;
		}
	}

	public int ClientID { get; set; }
	public int AreaID { get; set; }
	public Vector3 Position { get; set; }
	public string Name { get; set; }


	public QuestClientGameData(){
		ClientID = -1;
		Name = "Unknown";
	}

	public QuestClientGameData(int id, int areaID, Vector3 position, string name){
		ClientID = id;
		AreaID = areaID;
		Position = position;
		Name = name;
	}

}
