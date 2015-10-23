//Contains a quest in the game. A quest has progresses
//Provides API's that gives dialogues to a npc based on quest progress
//provides API's for querying quest progress
public class QuestGameData : IQuestGameData {
    public QuestTypeID ID { get; set; }
    public string QuestName { get; set; }
    public bool IsRepeatable { get; set; }
    public IQuestStateMachine StateMachine { get; set; }
    //public List<QuestDialogueData> SerializedDialogueTable { get; set; }

    public QuestTypeID Key {
        get {
            return ID;
        }
	}

    public QuestGameData() {
        //SerializedDialogueTable = new List<QuestDialogueData>();
        StateMachine = null;
        QuestName = "";
    }

    public void SetKey(QuestTypeID key) {
        ID = key;
    }

	//test for dictionary invariant
    //public bool IsDictionaryInvariant() {
    //    foreach (var list in SerializedDialogueTable) {
    //        if (!list.DialogueStates.TrueForAll(s => StateMachine.ContainState(s.questState))) {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    
}
