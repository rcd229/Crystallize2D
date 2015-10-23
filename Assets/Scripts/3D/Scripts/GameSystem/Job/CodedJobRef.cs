using UnityEngine;
using System;
using System.Collections;

public class CodedJobRef : IJobRef {

    public JobGameData GameDataInstance { get; private set; }
    public JobPlayerData PlayerDataInstance { get; private set; }

	public CodedJobRef(Guid id, JobGameData gameData, JobPlayerData playerData) {
		GameDataInstance = gameData;
		PlayerDataInstance = playerData;
	}
}
