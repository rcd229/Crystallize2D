using UnityEngine;
using System.Collections;

public class AreaConnectionGameData {

	public int AreaID { get; set; }
	public Vector3 Position { get; set; }

	public AreaConnectionGameData(){
		AreaID = -1;
	}

	public AreaConnectionGameData(int areaID, Vector3 position){
		AreaID = areaID;
		Position = position;
	}

}
