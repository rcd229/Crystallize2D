using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[XmlExtraType]
public class FindNamedPersonGeneratedQuestData : SearchPersonGeneratedQuestData {
    public string Name { get; set; }
    public bool IsMale { get; set; }

    public FindNamedPersonGeneratedQuestData() {
        Name = "";
        IsMale = true;
    }

    public FindNamedPersonGeneratedQuestData(ContextData context, List<Vector3> points, string name, bool isMale)
        : base(context, points) {
        Name = name;
        IsMale = isMale;
    }
}

public abstract class FindNamedPersonQuest : SearchPersonQuest<FindNamedPersonGeneratedQuestData> {

    public const string TargetName = "targetname";

    public override string QuestName { get { return "Find person"; } }

    public FindNamedPersonGeneratedQuestData GenerateNewQuestData(Vector3 client) {
        var male = UnityEngine.Random.Range(0, 2) == 0;
        var name = RandomNameGenerator.GetRandomCommonName(male);
        var context = new ContextData();
        context.Set(TargetName, new PhraseSequence(name));
        var points = PointScatterRegion.GeneratePositions(5, client, 5f, 10f);
        return new FindNamedPersonGeneratedQuestData(context, points, name, male);
    }

    public GameObject SpawnNPC(SpawnNPCContext context) {
        var go = DialogueActorUtil.GetNewActor(CharacterData);

        var charData = NPCCharacterData.GetRandom();
        var npc = new QuestNPCItemData();
        npc.CharacterData = charData;
        npc.TargetName = charData.Name;
        npc.EntryDialogue = Introduction;
        npc.ID = NPCID;
        go.AddComponent<QuestNPC>().Initialize(npc);

        CoroutineManager.Instance.WaitAndDo(
            () => {
                var genData = GenerateNewQuestData(go.transform.position);
                this.SetGeneratedQuestDatInstance(genData);
                go.GetComponent<DialogueActor>().GetOrCreateRandomContext().Add(genData.Context);
            });

        return go;
    }

    public override void UpdateSceneForState(QuestRef quest) {
        if (quest.PlayerDataInstance.State.IsEmptyOrNull()) {
            SceneResourceManager.Instance.SetResources(quest.ID.guid, null);
        } else {
            SceneResourceManager.Instance.SetResources(quest.ID.guid, GenerateActors());
        }
    }

    IEnumerable<GameObject> GenerateActors() {
        var genData = this.GetGeneratedDataInstance();
        var actors = DialogueActorUtil.GenerateActorsForTargets(genData.Points.Take(genData.Points.Count - 1));
        foreach (var a in actors) {
            var actor = a;
            MaterialFadeIn.Get(actor);
            actor.GetOrAddComponent<IndicatorComponent>().Initialize(
                "???", new OverheadIcon(IconType.QuestionMark, Color.yellow), new MapIndicator(MapResourceType.Standard, Color.yellow), false);
            actor.AddComponent<SceneInteractableObject>().HandleInteraction = (s, e) => HandleInteraction(actor, true, s, e);
        }
        actors.Add(NPCManager.Instance.SpawnNPCGroup(genData.Points.Last(), StandardNPCGroup.IntroductionGroup1));
        return actors;
    }

    void HandleInteraction(GameObject target, bool correct, ProcessExitCallback<object> callback, IProcess process) { }
}
