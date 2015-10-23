using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[SerializedQuestAttribute]
public class PointPlaceQuest01 : PointPlaceQuest1, INPCSpawner {

    float distance = 50f;

    DialogueSetBuilder set = new DialogueSetBuilder("PointPlaceQuest01");

    string contextID = "place";

    public static string IndicatorResourcePath {
        get {
            return "Prop/TaskNotice/Exclaimation";
        }
    }

    public override QuestTypeID ID { get { return QuestTypeID.PointPlaceQuest; } }

    #region implemented abstract members of PointPlaceQuest1
    public override DialogueSequence AskPlace {
        get {
            return set.Get("I want to go to [" + contextID + "]");
        }
    }

    public override INPCSpawner Spawner { get { return this; } }
    protected override IEnumerable<GameObject> GeneratePlaceIndicators() {

        var generatedInstance = this.GetGeneratedDataInstance();

        var targets = (GameObject.FindObjectsOfType(typeof(PlaceEnvironmentPhrase)) as PlaceEnvironmentPhrase[])
            .Where(t => Vector3.Distance(t.gameObject.transform.position, generatedInstance.SceneTargetPos) < distance);

        var targetGos = new List<GameObject>();

        foreach (var target in targets) {
            //things like the exclaimation mark
            var placeGO = GameObjectUtil.InstantiateAtPlace(IndicatorResourcePath, target.gameObject.transform);
            MaterialFadeIn.Get(placeGO);
            var sceneObject = placeGO.AddComponent<SceneInteractableObject>();
            var correct = PhraseSequence.PhrasesEquivalent(generatedInstance.correctPhrase, target.phrase.Get());
            sceneObject.HandleInteraction = (s, e) => HandleInteraction(correct, s, e);
            placeGO.tag = BaseFreeExploreProcess.consumerTag;

            targetGos.Add(placeGO);
        }
        Debug.Log("generate indicators " + targetGos.Count);
        return targetGos;
    }
    #endregion

    #region INPCSpawner implementation
    public bool CanSpawn(SpawnNPCContext context) {
        if (context.CurrentNPCs.Where(s => s.GetComponent<QuestNPC>()).Select(s => s.GetComponent<QuestNPC>())
           .Where(s => s.NPC.ID == NPCID.LostPerson).Count() > 0) {
            return false;
        }

        var targets = GameObject.FindObjectsOfType<PlaceEnvironmentPhrase>()
            .Where(t => Vector3.Distance(t.gameObject.transform.position, context.Point) < distance);

        return targets.Count() >= 2;
    }

    public GameObject SpawnNPC(SpawnNPCContext context) {
        var askerNPC = NPCManager.Instance.SpawnNPC(NPCID.LostPerson, context.Point, false);

        var phraseContext = new ContextData();
        var targets = GameObject.FindObjectsOfType<PlaceEnvironmentPhrase>()
            .Where(t => Vector3.Distance(t.gameObject.transform.position, context.Point) < distance).ToList();

        var correctPhrase = targets[UnityEngine.Random.Range(0, targets.Count)].phrase.Get();

        this.SetGeneratedQuestDatInstance(new PointPlaceGeneratedQuestData("", context.Point, correctPhrase));
        phraseContext.Set(contextID, correctPhrase);

        askerNPC.GetOrAddComponent<DialogueActor>().GetOrCreateRandomContext().Add(phraseContext);

        return askerNPC;
    }

    #endregion

    void HandleInteraction(bool correct, ProcessExitCallback<object> callback, IProcess process) {
        new SelectPlaceSubProcess(this, correct).Run(null, callback, process);
    }

    class SelectPlaceSubProcess : EnumeratorProcess<object, object> {
        PointPlaceQuest1 quest;
        bool correct;

        public SelectPlaceSubProcess(PointPlaceQuest1 quest, bool correct) {
            this.quest = quest;
            this.correct = correct;
        }

        public override IEnumerator<SubProcess> Run(object obj) {
            if (correct) {
                yield return Get(UILibrary.PositiveFeedback, "");
                NPCManager.Instance.RemoveNPC(NPCManager.Instance.GetNPC(NPCID.LostPerson));
                QuestUtil.EndQuest(quest.ID);
            } else {
                yield return Get(UILibrary.NegativeFeedback, "");
                PlayerDataConnector.AddConfidence(-1);
            }
        }
    }
}
