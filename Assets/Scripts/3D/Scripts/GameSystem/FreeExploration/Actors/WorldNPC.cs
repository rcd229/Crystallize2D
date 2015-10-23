using UnityEngine;
using System.Collections;
using System;

public class WorldNPC : EnergyConsumer {
	
	public int Friendliness{get;set;}
	public bool Unlocked {get;set;}

	public void AddFriendliness (int addition){
		Friendliness += addition;
	}
	
	public void ReduceFriendliness (int reduction) {
		Friendliness -= reduction;
	}
}
