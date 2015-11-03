using UnityEngine;
using System.Collections;

public class PersonApproachedEventArgs : System.EventArgs {

	public int WorldID { get; set; }

	public PersonApproachedEventArgs(int worldID){
		WorldID = worldID;
	}

}
