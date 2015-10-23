using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NPCGroupFormation : ResourceType<GameObject> {
    public static readonly NPCGroupFormation DialoguePair = new NPCGroupFormation("ActorPair");
    public static readonly NPCGroupFormation LookPair = new NPCGroupFormation("LookPair");

    protected override string ResourceDirectory { get { return "NPC/"; } }
    public NPCGroupFormation(string name) : base(name) { }
}
