using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class VariationListSelectorGameData : JobTaskSelectorGameData {

	/// <summary>
	/// require the player to repeat /RepetitionRequirement/ number of times before unlocking a higher level
	/// A higher level reveals a longer list and requires longer repetition inside game
	/// </summary>
	/// <value>The repetition requirement.</value>
    public int RepetitionRequirement { get; set; }

	public VariationListSelectorGameData()
	: base(typeof(VariationListSelectorProcess)) {
		RepetitionRequirement = 1;
    }

	public VariationListSelectorGameData(int repetition) : this() {
		RepetitionRequirement = repetition;
    }

}
