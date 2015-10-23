using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AreaGameData : ISerializableDictionaryItem<int> {

	public int Key {
		get {
			return AreaID;
        }
        set {
            AreaID = value;
        }
	}

	public int AreaID { get; set; }
	public List<AreaConnectionGameData> Connections { get; set; }
	public string LevelName { get; set; }
	public string AreaName { get; set; }
    public int Cost { get; set; }

	public AreaGameData(){
		AreaID = -1;
		LevelName = "";
		AreaName = "";
        Connections = new List<AreaConnectionGameData>();
	}

    public AreaGameData(int areaID) : this() {
        AreaID = areaID;
    }

	public AreaGameData(int id, List<AreaConnectionGameData> connections, string levelID, string name){
		this.AreaID = id;
		this.Connections = connections;
		this.LevelName = levelID;
		this.AreaName = name;
	}

	public AreaConnectionGameData GetConnection(int areaID){
		return (from c in Connections where c.AreaID == areaID select c).FirstOrDefault ();
	}

}
