using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Energy manager manages the energy the players has per day
/// There is only on manager for each player at any time
/// </summary>
public class EnergyManager {

    private static EnergyManager _instance;
    public static EnergyManager Instance {
        get {
            if (_instance == null) {
                _instance = new EnergyManager();
            }
            return _instance;
        }
    }

	//private int _energy;

	//check if energy is zero every time it is changed
	public int Energy{
		get{
            return PlayerData.Instance.Session.Confidence; //_energy;
		}
		set{
            PlayerDataConnector.SetConfidence(value);
            //_energy = value;
			//TaskState.Instance.SetState("Energy", _energy.ToString());
            if (PlayerData.Instance.Session.Confidence <= 0) {
				EndDay();
			}
		}
	}

	public event EventHandler<EventArgs<object>> NoEnergy;

	void EndDay(){
		RemoveAll();
		_instance = null;
		NoEnergy.Raise(this, null);
	}


	Dictionary<string, EnergyConsumer> ConsumerTable = new Dictionary<string, EnergyConsumer>();

	///If the consumer does not exist, add it. Otherwise do nothing
	public void AddConsumerOrDefault(EnergyConsumer cos){
		if(!ConsumerTable.ContainsKey(cos.CharacterName)){
			ConsumerTable.Add(cos.CharacterName, cos);
            //HookConsumer(cos);
		}

	}

	public void AddConsumers(IEnumerable<IEnergyConsumer> consumers){
		foreach(var e in consumers){
			AddOrReplaceConsumer((EnergyConsumer) e);
		}
	}

	///Add or replace a consumer
	public void AddOrReplaceConsumer(EnergyConsumer cos){
		ConsumerTable[cos.CharacterName] = cos;
        //HookConsumer(cos);
	}

	///remove a cosumer from energy management
	public void RemoveConsumer(EnergyConsumer cos){
		if(ConsumerTable.ContainsKey(cos.CharacterName)){
            //ConsumerTable[cos.ConsumerName].OnInteraction -= DeductMoney;
			ConsumerTable.Remove(cos.CharacterName);
		}
	}

	/// <summary>Add a consumer to energy list</summary>
    //void HookConsumer (EnergyConsumer cos)
    //{
    //    cos.OnInteraction += DeductMoney;
    //}

	void RemoveAll(){
		var all = ConsumerTable.Values.ToArray();
		for(int i = 0; i < all.Length; i++){
			RemoveConsumer(all[i]);
		}
	}

    void DeductMoney(object sender, EventArgs<int> arg) {
        Energy -= arg.Data;
    }
}
