using UnityEngine;
using System.Collections;
using System;

public abstract class EnergyConsumer : MonoBehaviour, IEnergyConsumer {

	string _consumername = "";
	public event EventHandler<EventArgs<int>> OnInteraction;

	public string CharacterName{
		get{
			return _consumername;
		}set{
			if(_consumername == ""){
				_consumername = value;
			}
			else{
				Debug.LogError("Consumer name has already been set");
			}
		}
	}

	protected void RaiseCost(int cost)
	{
		OnInteraction.Raise(this, new EventArgs<int>(cost));
	}
	
}
