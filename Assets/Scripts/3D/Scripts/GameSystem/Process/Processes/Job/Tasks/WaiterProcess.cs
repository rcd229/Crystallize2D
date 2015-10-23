using UnityEngine;
using System.Collections;

[JobProcessType]
public class WaiterProcess : IProcess<JobTaskRef, object> {

    public ProcessExitCallback OnExit { get; set; }

    int count = 5;

    int selectIndex = 0;
    GameObject person;
    JobTaskRef task;

    public void Initialize(JobTaskRef param1) {
        task = param1;
        person = new SceneObjectRef(task.Data.Actor).GetSceneObject();
        ProcessLibrary.Conversation.Get(new ConversationArgs(person, task.Data.Dialogue, GetNewContextData()), HandleConversationExit, this);
    }

    void HandleConversationExit(object sender, object obj) {
        count--;
        if (count <= 0) {
            var amt = (int)(PlayerData.Instance.Session.RestQuality * 10000);
            var s = string.Format("You made {0} yen today.", amt);
            PlayerDataConnector.AddMoney(amt);
            ProcessLibrary.MessageBox.Get(s, HandleMessage, this);
        } else {
            person.GetComponent<MaterialRandomizer>().Randomize();
            ProcessLibrary.MessageBox.Get("A new person has arrived.", HandleNewPerson, this);
        }
    }

    void HandleNewPerson(object sender, object args) {
        ProcessLibrary.Conversation.Get(new ConversationArgs(person, task.Data.Dialogue, GetNewContextData()), HandleConversationExit, this);
    }

    void HandleMessage(object sender, object obj) {
        Exit();
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

    ContextData GetNewContextData() {
        //var countPhrases = task.Data.Line.Phrase.PhraseElements;
        var countPhrases = new System.Collections.Generic.List<PhraseSequenceElement>(new PhraseSequenceElement[]{
            new PhraseSequenceElement(PhraseSequenceElementType.Text, "1"),
            new PhraseSequenceElement(PhraseSequenceElementType.Text,"2"),
            new PhraseSequenceElement(PhraseSequenceElementType.Text,"3")
        });
        selectIndex = Random.Range(0, countPhrases.Count);
        var c = new ContextData();
        var ps = new PhraseSequence();
        ps.Add(countPhrases[selectIndex]);
        c.Set("people", ps);
        return c;
    }

}