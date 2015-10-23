using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class NPCID : UniqueID {

    public static IEnumerable<NamedGuid> GetIDs() {
        return NamedGuid.GetIDs<NPCID>();
    }

    public static readonly NPCID TutorialNPC1 = new NPCID("dacb614647074a8383a665874a7b832f");
    public static readonly NPCID TutorialNPC2 = new NPCID("0a2a9f6931124ec2b74bdfb7cab9d413");
    //public static readonly NPCID TutorialNPC3 = new NPCID("ba66644b9e4e471eadf7568c96916743");
    public static readonly NPCID TutorialNPC4 = new NPCID("08be603bd64b40d08cbe3c43171556ed");
    //public static readonly NPCID NewStudent = new NPCID("021a0cab4ccb48c8bae1304fa9eea3bf");

    public static readonly NPCID TestNPC1 = new NPCID("d3d286711c734af2aaaf01957e6b2499");
    public static readonly NPCID TestNPC2 = new NPCID("2e6967f46d284aff8438416ea6cb1e32");
    public static readonly NPCID TestNPC3 = new NPCID("251804df353647419f03e91a02ff48b5");
    public static readonly NPCID HelpNPC1 = new NPCID("c52194ca29cd4abab0732acb6a325ba3");
    public static readonly NPCID FindCatNPC = new NPCID("d390e707c1444ddf9eec27f9fe6aa30b");
    public static readonly NPCID FeelHungryNPC = new NPCID("59e5b23268ac45e9a43f3faba5408ef6");
    //for Sakura quest
    public static readonly NPCID Sakura = new NPCID("3beddb848b1f46e3a9933ccb13064106");
    public static readonly NPCID Tourist1 = new NPCID("f1374e3a046d478d804afa7e42a89bc0");
    public static readonly NPCID Anna = new NPCID("2cec75ab1a704d1386a201e2166607d4");

    public static readonly NPCID LostPerson = new NPCID("51adf7e460264c4da41b309ae04c1726");

    public NPCID() : base() { }
    public NPCID(Guid id) : base(id) { }
    public NPCID(string id) : base(id) { }
}
