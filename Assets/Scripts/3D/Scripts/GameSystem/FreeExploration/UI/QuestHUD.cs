using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Linq;

public class QuestHUDItem {
    public string Title { get; private set; }
    public List<QuestHUDSubItem> SubItems { get; private set; }
    public QuestHUDItem(string title, params QuestHUDSubItem[] subItems) {
        this.Title = title;
        this.SubItems = new List<QuestHUDSubItem>(subItems);
    }
}

public class QuestHUDSubItem {
    public bool IsComplete { get; private set; }
    public string Text { get; private set; }
    public QuestHUDSubItem(string text, bool isComplete) {
        this.Text = text;
        this.IsComplete = isComplete;
    }
}

public interface IHasQuestStateDescription {
    QuestHUDItem GetDescriptionForState(QuestRef quest);
}

[ResourcePath("UI/QuestHUD")]
public class QuestHUD : UIMonoBehaviour, ITemporaryUI<object, object> {

    public static QuestHUD Instance { get; private set; }

	public event EventHandler<EventArgs<object>> Complete;

	//public Text questListText;
	string questListString;
    public RectTransform childParent;
    public GameObject questItemPrefab;

    List<GameObject> instances = new List<GameObject>();

	public void Initialize (object param1)
	{
        Instance = this;
	}

	public void Close ()
	{
        if (this) {
            PlayerDataConnector.QuestStatusPanelOpenStatus = false;
            GameObject.Destroy(this.gameObject);
        }
	}

    void OnEnable() {
        Refresh();
        CrystallizeEventManager.PlayerState.QuestStateChanged += PlayerState_QuestStateChanged;
    }

    void OnDisable() {
        if (CrystallizeEventManager.Alive) {
            CrystallizeEventManager.PlayerState.QuestStateChanged -= PlayerState_QuestStateChanged;
        }
    }

    void PlayerState_QuestStateChanged(object sender, EventArgs<object> e) {
        Debug.Log("Refreshing");
        Refresh();
    }

    void Refresh(){
        instances.DestroyAndClear();

        var unlockedQuests = QuestUtil.UnlockedQuests
            .Where(q => q.GameDataInstance.StateMachine is IHasQuestStateDescription)
            .OrderBy(q => q.QuestName);

        foreach (var q in unlockedQuests) {
            var desc = ((IHasQuestStateDescription)q.GameDataInstance.StateMachine).GetDescriptionForState(q);
            if (desc != null) {
                var instance = Instantiate<GameObject>(questItemPrefab);
                instance.GetComponent<IInitializable<QuestHUDItem>>().Initialize(desc);
                instance.transform.SetParent(childParent, false);
                instances.Add(instance);
            }
        }
    }
	
	// Update is called once per frame
    //void Update () {
    //    var UnlockedQuest = FreeExploreQuestManager.UnlockedQuests.OrderBy(s => s).ToList();
    //    var ViewedQuest = FreeExploreQuestManager.ViewedQuests.OrderBy(s => s).ToList();
    //    int viewedCount = 0;
    //    questListString = "";

    //    for(int i = 0; i < UnlockedQuest.Count; i++){
    //        questListString += '\n';
    //        questListString += UnlockedQuest[i];

    //        if(viewedCount < ViewedQuest.Count && ViewedQuest[viewedCount] == UnlockedQuest[i]){
    //            questListString += " (viewed)";
    //            viewedCount++;
    //        }
    //        questListString += '\n';
    //    }

    //    questListText.text = questListString;
    //}
}
