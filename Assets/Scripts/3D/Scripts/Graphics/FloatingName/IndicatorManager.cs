using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class IndicatorManager : MonoBehaviour {

    static IndicatorManager _instance;
    public static IndicatorManager Instance {
        get {
            if (!_instance) {
                _instance = new GameObject("IndicatorManager").AddComponent<IndicatorManager>();
            }
            return _instance;
        }
    }

    public static void SetIndicatorsChanged() {
        Instance.indicatorsChanged = true;
    }

    bool indicatorsChanged = false;

    void Start() {
        UILibrary.InitializeFloatingNames();
        PlayerProximity.Instance.ProximityChanged += Instance_ProximityChanged;
    }

    void OnDestroy() {
        if (PlayerProximity.Alive) {
            PlayerProximity.Instance.ProximityChanged -= Instance_ProximityChanged;
        }
    }

    void Update() {
        if (indicatorsChanged) {
            AssignIndicators();
            indicatorsChanged = false;
        }
    }

    void Instance_ProximityChanged(object sender, ProximityArgs e) {
        SetIndicatorsChanged();
    }

    public void AssignIndicators() {
        //var npcs = GameObject.FindObjectsOfType<QuestNPC>();
        //foreach (var npc in npcs) {
        //    AssignIndicator(npc);
        //    MapManager.Instance.AddMapElement(npc.transform);
        //    CrystallizeEventManager.Environment.RaiseQuestNPCCreated(this, new GameObjectArgs(npc.gameObject));
        //}

        //foreach (var boss in GameObject.FindObjectsOfType<SceneBoss>()) {
        //    CrystallizeEventManager.UI.RaiseFloatingNameRequested(this,
        //        new FloatingNameEventArgs(boss.transform, , Color.white));
        //    MapManager.Instance.AddMapElement(boss.transform);
        //    // TODO: change whether new stuff is available
        //    OverheadIndicatorUI.Instance.SetIndicator(boss.transform, IconType.Briefcase.LoadResource(), boss.hasNew());
        //}

        //foreach (var shop in GameObject.FindObjectsOfType<SceneShop>()) {
        //    // TODO: change whether new stuff is available
        //    CrystallizeEventManager.UI.RaiseFloatingNameRequested(this,
        //         new FloatingNameEventArgs(shop.transform, shop.Shop.GetInitArgs().Title, Color.white));
        //    MapManager.Instance.AddMapElement(shop.transform);
        //    OverheadIndicatorUI.Instance.SetIndicator(shop.transform, IconType.ShoppingCart.LoadResource(), shop.HasNew());
        //}

        foreach (var npc in IndicatorComponent.Indicators) {
            if (!npc.Name.IsEmptyOrNull()) {
                CrystallizeEventManager.UI.RaiseFloatingNameRequested(this,
                    new FloatingNameEventArgs(npc.transform, npc.Name, Color.white));
            }

            if (npc.Icon != null) {
                MapManager.Instance.AddMapElement(npc.transform, npc.MapIndicator);
                OverheadIndicatorUI.Instance.SetIndicator(npc.transform, npc.Icon.Type.LoadResource(), npc.Icon.Color, false);
            }
        }
    }

    // TODO: move this to the UI
    Color GetColor(Transform npc) {
        var d = npc.GetComponent<QuestNPC>().NPC.EntryDialogue;
        var words = d.AggregateNPCWords();
        var unknownCount = 0;
        foreach (var word in words) {
            if (!PlayerDataConnector.ContainsLearnedItem(word)) {
                unknownCount++;
            }
        }
        //Debug.Log("Unknown: " + unknownCount);
        if (unknownCount == 0) {
            return Color.blue.Lighten(0.8f);
        } else if (unknownCount < (PlayerData.Instance.Proficiency.Confidence / 2)) {
            return Color.white;
        } else if (unknownCount <= PlayerData.Instance.Proficiency.Confidence) {
            return Color.yellow.Lighten(0.5f);
        } else {
            return Color.red.Lighten(0.5f);
        }
    }

}
