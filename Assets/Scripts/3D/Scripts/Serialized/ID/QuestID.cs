using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class QuestInstanceID : UniqueID {
    public QuestInstanceID() : base() { }
    public QuestInstanceID(string id) : base(id) { }
    public QuestInstanceID(Guid id) : base(id) { }
}

public class QuestTypeID : UniqueID {

    public static IEnumerable<NamedGuid> GetIDs() {
        return NamedGuid.GetIDs<QuestTypeID>();
    }

    public static readonly QuestTypeID FindCatQuest = new QuestTypeID("d97de310190743c9a026532d2de12b0a");
    public static readonly QuestTypeID PersonHungryQuest = new QuestTypeID("58e87fd8c7c1452ba8dc7118097b32ec");
    public static readonly QuestTypeID KnowSakuraQuest = new QuestTypeID("9d281ade53f841f982894c79b9c48dbd");
    public static readonly QuestTypeID SakuraFirstQuest = new QuestTypeID("472c5f33582a47aaafe590bb530af138");
    public static readonly QuestTypeID BecomeFriendWithAnna = new QuestTypeID("22aa4431ddce47fa92ac609b02629deb");

    public static readonly QuestTypeID PointPlaceQuest = new QuestTypeID("4ec669d566e64ca689f5d4dfc2ce0c87");

    public QuestTypeID() : base() { }
    public QuestTypeID(string id) : base(id) { }
    public QuestTypeID(Guid id) : base(id) { }

}
