using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[XmlExtraType]
public class GetToKnowNPCGeneratedQuestData : GeneratedQuestData {
    public int CurrentNPCIndex { get; set; }
    public string TargetContext { get; set; }
    public List<GetToKnowNPCInstanceData> NPCs { get; set; }
    public GetToKnowNPCInstanceData CurrentNPC {
        get { return NPCs.GetSafely(CurrentNPCIndex); }
    }

    public GetToKnowNPCGeneratedQuestData() {
        NPCs = new List<GetToKnowNPCInstanceData>();
    }

    public void UpdateInstanceContext(GetToKnowNPCInstanceData instance) {
        foreach (var c in ContextPhraseResources.GetRepeatableContext()) {
            var phrases = ContextPhraseResources.GetAvailableForContext(c);
            var key = phrases.Select(p => p.Translation).GetNewRandom(GetExistingContexts(c));
            var phrase = phrases.Where(p => p.Translation == key).FirstOrDefault();
            instance.Context.Set(c, phrase);
            if (phrase == null) {
                Debug.Log(key + " resulted in null phrase");
            }
            Debug.Log("Context set to: " + key + "; " + phrase.GetText());

            if (c == ContextPhraseResources.Hobby) {
                instance.Context.Set("doinghobby", HobbyPhraseResources.GetPhraseForHobby(key));
            }
        }
    }

    public IEnumerable<string> GetExistingContexts(string context) {
        return from npc in NPCs
               where npc.Context.Get(context) != null
               select npc.Context.Get(context).Data.Translation;
    }
}

public class GetToKnowNPCInstanceData {
    public Guid guid { get; set; }
    public NPCCharacterData CharacterData { get; set; }
    public ContextData Context { get; set; }
    public HashSet<string> KnownContext { get; set; }
    public HashSet<int> GivenParts { get; set; }
    public int FriendshipScore { get; set; }
    public DateTime LastSeen { get; set; }

    public GetToKnowNPCInstanceData() {
        guid = Guid.NewGuid();
        CharacterData = NPCCharacterData.GetRandom();
        GivenParts = new HashSet<int>();
        KnownContext = new HashSet<string>();
        Context = new ContextData();
        LastSeen = DateTime.Now;

        var name = "";
        if (CharacterData.Appearance.Gender == 0) {
            name = RandomNameGenerator.GetRandomMaleName();
        } else {
            name = RandomNameGenerator.GetRandomFemaleName();
        }
        Context.Set("name", new PhraseSequence(name));
    }
}

public abstract class BaseGetToKnowNPCQuest : BaseQuestGameData,
    INPCSpawner, IHasNPCSpawner, IQuestStateMachine, IGeneratedQuest<GetToKnowNPCGeneratedQuestData> {
    const float NewNPCProbability = 0.4f;

    public override QuestTypeID ID { get { return new QuestTypeID("07835d0b523048d29748b549384e5e62"); } }
    public override string QuestName { get { return "Get to know people"; } }
    public override bool IsRepeatable { get { return false; } }
    public override IQuestStateMachine StateMachine { get { return this; } }
    public string FirstState { get { return "Root"; } }
    public INPCSpawner Spawner { get { return this; } }

    public abstract DialogueSequence FirstTimeDialogue { get; }

    public abstract DialogueSequence GetQuizDialogueForContext(string context);
    public abstract DialogueSequence GetNewQuestionDialogue(string targetContext);
    public abstract NPCGroup GetSupportGroupForContext(string targetContext);

    public string GetNextState(string state, string transition) {
        return transition;
    }

    public void UpdateSceneForState(QuestRef quest) { }

    public bool CanSpawn(SpawnNPCContext context) {
        var genData = GetGeneratedData();
        if (genData == null || genData.NPCs == null || genData.NPCs.Count == 0) {
            return true;
        }

        var idHash = new HashSet<Guid>(
            context.CurrentNPCs.Where(npc => npc.GetComponent<QuestNPC>()).Select(npc => npc.GetComponent<QuestNPC>().NPC.ID.guid)
        );

        foreach (var npc in genData.NPCs) {
            if (idHash.Contains(npc.guid)) {
                return false;
            }
        }
        return true;
    }

    public GameObject SpawnNPC(SpawnNPCContext context) {
        AddNewNPC();
        var data = GetGeneratedData();
        data.CurrentNPCIndex = UnityEngine.Random.Range(0, data.NPCs.Count);
        Debug.Log("NPC count: " + data.NPCs.Count + "; " + data.CurrentNPCIndex);
        var genData = GetGeneratedData().CurrentNPC;

        var go = DialogueActorUtil.GetNewActor(genData.CharacterData);
        var npc = new QuestNPCItemData();
        npc.ID = new NPCID(genData.guid);
        npc.QuestID = ID;
        npc.CharacterData = genData.CharacterData;
        npc.CharacterData.Name = "Friendly person";
        npc.OverrideOverheadName = "Friendly person";

        if (genData.KnownContext.Count == 0) {
            npc.EntryDialogue = FirstTimeDialogue;
            var support = SpawnSupportNPCs(context.Point, GetSupportGroupForContext("name"));
            go.GetOrAddComponent<DestroyEvent>().Destroyed += (s, e) => GameObject.Destroy(support);
        } else if (UnityEngine.Random.value < 0.5f && genData.KnownContext.Count < 5) {
            var targetContext = ContextPhraseResources.GetRepeatableContext().GetNewRandom(genData.KnownContext);
            npc.EntryDialogue = GetNewQuestionDialogue(targetContext);
            var support = SpawnSupportNPCs(context.Point, GetSupportGroupForContext(targetContext));
            go.GetOrAddComponent<DestroyEvent>().Destroyed += (s, e) => GameObject.Destroy(support);
        } else {
            npc.EntryDialogue = GetQuizDialogueForContext(genData.KnownContext.GetRandomFromEnumerable());
        }

        go.GetOrAddComponent<QuestNPC>().Initialize(npc);
        go.GetOrAddComponent<DialogueActorContext>().Add(genData.Context);
        go.GetOrAddComponent<DestroyEvent>().Destroyed += (s, e) => CleanNPCs();

        return go;
    }

    GameObject SpawnSupportNPCs(Vector3 position, NPCGroup group) {
        var p = PointScatterRegion.GeneratePositions(1, position, 5f, 10f);
        //Debug.Log("spawning support: " + p.Count + "; " + p.GetSafely(0));
        return NPCManager.Instance.SpawnNPCGroup(p.GetSafely(0), group);
    }

    void AddNewNPC() {
        var genData = GetGeneratedData();
        if (genData.NPCs.Count > 0 && UnityEngine.Random.value > NewNPCProbability) {
            return;
        }
        var newNPC = new GetToKnowNPCInstanceData();
        genData.UpdateInstanceContext(newNPC);
        genData.NPCs.Add(newNPC);
    }

    void CleanNPCs() {
        var data = GetGeneratedData().NPCs;
        for (int i = 0; i < data.Count; i++) {
            if (data[i].KnownContext.Count == 0) {
                data.RemoveAt(i);
                i--;
            }
        }
    }

    public void LearnContext(string context) {
        GetGeneratedData().CurrentNPC.KnownContext.Add(context);
    }

    public void AddFriendship(int amount) {
        var score = GetGeneratedData().CurrentNPC.FriendshipScore;
        if (score == 0) {
            PlayerDataConnector.QuestCompleted = "-Made new friend!";
        } else {
            PlayerDataConnector.QuestCompleted = "-Friendship increased!";
            if (score == 1) {
                PlayerDataConnector.QuestReward = QuestReward.Money(500);
            } else if (score == 3) {
                var available = BuyableFurniture.GetRandomAvailable();
                if (available == null) {
                    PlayerDataConnector.QuestReward = QuestReward.Money(2000);
                } else {
                    PlayerDataConnector.QuestReward = QuestReward.Furniture(available);
                }
            } else if (score == 6) {
                GiveClothes();
            } else if (score == 10) {
                GiveClothes();
            }
        }
        GetGeneratedData().CurrentNPC.FriendshipScore += amount;
    }

    void GiveClothes() {
        var target = GetGeneratedData().CurrentNPC;
        var availableParts = new HashSet<int>(new int[] { 0, 1 });
        availableParts.ExceptWith(target.GivenParts);
        var part = (BodyPartType)availableParts.GetRandomFromEnumerable();
        target.GivenParts.Add((int)part);

        int mat, type;
        target.CharacterData.Appearance.GetPartParameters(part, out mat, out type);
        var available = BuyableClothes.GetItemWithParams(part, type, mat);
        if (available == null) {
            PlayerDataConnector.QuestReward = QuestReward.Money(2000);
        } else {
            PlayerDataConnector.QuestReward = QuestReward.Clothing(available);
        }
    }

    public void ClearSceneNPC() {
        var sceneNPC = QuestNPC.Get(GetGeneratedData().CurrentNPC.guid);
        if (sceneNPC) {
            Debug.Log("adding event");
            sceneNPC.gameObject.GetOrAddComponent<OnExitDialogueEvent>().OnExitDialogue += (s, e) => GameObject.Destroy(sceneNPC.gameObject);
        } else {
            Debug.Log("unable to find NPC");
        }
    }

    protected GetToKnowNPCGeneratedQuestData GetGeneratedData() {
        var genData = this.GetGeneratedDataInstance();
        if (genData == null) {
            genData = new GetToKnowNPCGeneratedQuestData();
            this.SetGeneratedQuestDatInstance(genData);
        }
        return genData;
    }

}