using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// Used for classes that implement QuestGameData and contain a default constructor
/// </summary>
public class SerializedQuestAttribute : Attribute { }

/// <summary>
/// Used for classes that contain static fields or properties that are QuestGameData
/// </summary>
public class SerializedQuestsAttribute : Attribute { }

public static class SerializedQuestExtensions {
    public static IEnumerable<IQuestGameData> GetAllQuests() {
        var q1 = from questType in Assembly.GetAssembly(typeof(SerializedQuestAttribute)).GetTypes()
                 where typeof(IQuestGameData).IsAssignableFrom(questType) 
                 && questType.HasAttribute<SerializedQuestAttribute>() 
                 && questType.GetConstructor(Type.EmptyTypes) != null
                 select Activator.CreateInstance(questType) as IQuestGameData;
        var q2 = from qs in Assembly.GetAssembly(typeof(SerializedQuestsAttribute)).GetTypes()
                 where qs.HasAttribute<SerializedQuestsAttribute>()
                 from f in qs.GetFieldsAndProperties<IQuestGameData>(BindingFlags.Static | BindingFlags.Public)
                 select f.GetMemberValue(null) as IQuestGameData;
        return q1.Concat(q2);
    }
}