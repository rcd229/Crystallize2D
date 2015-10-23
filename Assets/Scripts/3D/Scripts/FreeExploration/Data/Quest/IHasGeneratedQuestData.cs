using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public interface IHasGeneratedQuestData {
    //GeneratedQuestData GenerateNewQuestData();
}

public interface IHasGeneratedQuestData<T> : IHasGeneratedQuestData where T : GeneratedQuestData { }

public interface IGeneratedQuest<T> : IHasGeneratedQuestData<T>, IQuestGameData where T : GeneratedQuestData { }

public static class GeneratedQuestDataExtensions {
    public static void SetGeneratedQuestDatInstance<T>(this IGeneratedQuest<T> quest, T questData) where T : GeneratedQuestData {
        PlayerData.Instance.QuestData.GetOrCreateItem(quest.ID).GeneratedQuestData = questData;
    }

    public static T GetGeneratedDataInstance<T>(this IGeneratedQuest<T> quest) where T : GeneratedQuestData {
        var data = PlayerData.Instance.QuestData.GetOrCreateItem(quest.ID).GeneratedQuestData;
        //Debug.Log("Data is: " + data);
        return data as T;
    }

    //public static T GetOrCreateGeneratedDataInstance<T>(this IGeneratedQuest<T> quest) where T : GeneratedQuestData {
    //    if (quest.GetGeneratedDataInstance() == null) {
    //        quest.SetGeneratedQuestDatInstance();
    //    }
    //    return quest.GetGeneratedDataInstance();
    //}
}