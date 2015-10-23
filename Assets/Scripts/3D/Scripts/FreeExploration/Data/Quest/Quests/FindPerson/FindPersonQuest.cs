using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using CrystallizeData;
using System.Xml.Serialization;

[XmlExtraType]
public class FindPersonGeneratedQuestData : SearchPersonGeneratedQuestData {
    public string PersonKey { get; set; }
    public string HairKey { get; set; }

    public FindPersonGeneratedQuestData() {
        PersonKey = "";
        HairKey = "";
    }

    public FindPersonGeneratedQuestData(ContextData context, List<Vector3> points, string personKey, string hairKey = "")
        : base(context, points) {
        this.PersonKey = personKey;
        this.HairKey = hairKey;
    }
}

// TODO: make this storable
public abstract class FindPersonQuest : SearchPersonQuest<FindPersonGeneratedQuestData> {

    static readonly int[] Tiers = new int[] { 0, 5 };

    public override string QuestName { get { return "Find person"; } }

    public abstract int Tier { get; }
    public abstract FindPersonGeneratedQuestData GenerateNewQuestData(Vector3 client);

    public override void UpdateSceneForState(QuestRef quest) {
        if (quest.PlayerDataInstance.State.IsEmptyOrNull()) {
            SceneResourceManager.Instance.SetResources(quest.ID.guid, null);
        } else {
            SceneResourceManager.Instance.SetResources(quest.ID.guid, GenerateActors());
        }
    }

    IEnumerable<GameObject> GenerateActors() {
        var genData = this.GetGeneratedDataInstance();
        var actors = PersonAttributes.GetNewActorInstances(genData.Points, genData.PersonKey, genData.HairKey, "");
        foreach (var a in actors) {
            var actor = a;
            MaterialFadeIn.Get(actor);
            actor.GetOrAddComponent<IndicatorComponent>().Initialize(
                "Lost-looking person", new OverheadIcon(IconType.QuestionMark, Color.yellow), new MapIndicator(MapResourceType.Standard, Color.yellow), false);
            var correct = actor.name.Contains("Correct");
            if (genData.PersonKey == PersonAttributes.Person) {
                actor.AddComponent<SceneInteractableObject>().HandleInteraction = (s, e) => HandleInteraction(actor, true, s, e);
            } else {
                actor.AddComponent<SceneInteractableObject>().HandleInteraction = (s, e) => HandleInteraction(actor, correct, s, e);
            }
        }
        return actors;
    }

    public virtual bool CanSpawn(SpawnNPCContext context) {
        var t = GetTier();
        if (Tier != t) {
            return false;
        }
        var canSpawn = !context.ContainsQuestNPC(NPCID);
        //Debug.Log("tier is " + t + "; " + Tier + "; " + canSpawn);
        return canSpawn;
    }

    int GetTier() {
        int currentReps = PlayerData.Instance.QuestData.Get(ID).FinishedTimes;
        for (int i = Tiers.Length - 1; i >= 0; i--) {
            if (currentReps >= Tiers[i]) {
                return i;
            }
        }
        return 0;
    }

    public GameObject SpawnNPC(SpawnNPCContext context) {
        var go = DialogueActorUtil.GetNewActor(CharacterData);
        var npc = new QuestNPCItemData();
        npc.OverrideOverheadName = "Looking for someone";
        npc.EntryDialogue = Introduction;
        npc.ID = NPCID;
        npc.QuestID = ID;
        go.AddComponent<QuestNPC>().Initialize(npc);

        CoroutineManager.Instance.WaitAndDo(
            () => {
                var genData = GenerateNewQuestData(go.transform.position);
                this.SetGeneratedQuestDatInstance(genData);
                go.GetComponent<DialogueActor>().GetOrCreateRandomContext().Add(genData.Context);
            });

        return go;
    }

    void HandleInteraction(GameObject target, bool correct, ProcessExitCallback<object> callback, IProcess process) {
        new TalkToCandidateSubprocess(this, correct).Run(target, callback, process);
    }

    class TalkToCandidateSubprocess : EnumeratorProcess<GameObject, object> {
        FindPersonQuest quest;
        bool correct;
        public TalkToCandidateSubprocess(FindPersonQuest quest, bool correct) {
            this.quest = quest;
            this.correct = correct;
        }

        public override IEnumerator<SubProcess> Run(GameObject target) {
            var convArgs = new ConversationArgs(target, quest.PersonApproached, null, true);
            yield return Get(ProcessLibrary.BeginConversation, convArgs);
            yield return Get(ProcessLibrary.YesNo, new YesNoArgs(new PhraseSequence("Is this the right person?"), false));
            if ((bool)result == true) {
                if (correct) {
                    convArgs.Dialogue = quest.CorrectPersonConfirmed;
                    yield return Get(ProcessLibrary.ConversationSegment, convArgs);
                    yield return Get(ProcessLibrary.EndConversation, convArgs);
                    var exitArgs = new ConversationArgs(NPCManager.Instance.GetNPC(quest.NPCID), quest.CompleteQuest, null, true);
                    yield return Get(ProcessLibrary.BeginConversation, exitArgs);
                    yield return Get(ProcessLibrary.EndConversation, exitArgs);
                    if (exitArgs.Target) {
                        GameObject.Destroy(exitArgs.Target);
                    }
                    QuestUtil.EndQuest(quest.ID);
                } else {
                    yield return Get(UILibrary.NegativeFeedback, null);
                    convArgs.Dialogue = quest.PersonRejected;
                    yield return Get(ProcessLibrary.ConversationSegment, convArgs);
                    yield return Get(ProcessLibrary.EndConversation, convArgs);
                }
            } else {
                convArgs.Dialogue = quest.PersonRejected;
                yield return Get(ProcessLibrary.ConversationSegment, convArgs);
                yield return Get(ProcessLibrary.EndConversation, convArgs);
            }
        }
    }

}