using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NPCQuestFlag {
	public static IEnumerable<NamedGuid> GetIDs() {
		return NamedGuid.GetIDs<NPCQuestFlag>();
	}
	//for testing
	public static readonly Guid CatColorKnown = new Guid("81ca16f5b1be4243805c05888701683d");
	public static readonly Guid CatFound = new Guid("7206f8c1852c45b8be49a0aa203c4302");
	//sakura quest
	public static readonly Guid SakuraIntroduced = new Guid("4d6d73be20ab4de2aff661f19ef42887");
	public static readonly Guid AnnaIntroduced = new Guid("760a9f975beb4f7f8404ed71a5a64df9");
	//flag that is never raised
	public static readonly Guid NeverRaisedFlag = new Guid("4328e546df69489eadb7eb64dc9c1bb1");

    public static readonly Guid TutorialConversations = new Guid("df0c3272274447da95f4bc2441dab482");

    public static readonly Guid PointPlaceUnlockedFlag = new Guid("7b01c92d00224a898e9bee1c45304dcf");

}
