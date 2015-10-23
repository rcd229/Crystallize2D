using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MapIndicator {
    public virtual MapResourceType Type { get; private set; }
    public virtual Color Color { get; private set; }

    protected MapIndicator() { }

    public MapIndicator(MapResourceType type, Color color = default(Color)) {
        Color = color;
        if (color == default(Color)) {
            Color = Color.white;
        }
        Type = type;
    }
}

public class MapResourceType : ResourceType<GameObject> {
    public static readonly MapResourceType QuestNPC = new MapResourceType("MapQuestNPC");
    public static readonly MapResourceType ShopNPC = new MapResourceType("MapShopNPC");
    public static readonly MapResourceType BossNPC = new MapResourceType("MapBossNPC");
    public static readonly MapResourceType Standard = new MapResourceType("StandardNPC");
    public static readonly MapResourceType Player = new MapResourceType("MapPlayer");

    protected override string ResourceDirectory { get { return "UI/Map/"; } }

    MapResourceType(string path) : base(path) { }

    public static MapIndicator GetTypeForGameObject(GameObject gameObject) {
        if (gameObject.GetComponent<QuestNPC>()) {
            if (gameObject.GetComponent<QuestNPC>().HasQuest) {
                return new MapIndicator(QuestNPC);
            } else {
                return new MapIndicator(Standard, gameObject.GetComponent<QuestNPC>().color);
            }
        } else if (gameObject.GetComponent<SceneShop>()) {
            return new MapIndicator(ShopNPC);
        } else if (gameObject.GetComponent<SceneBoss>()) {
            return new MapIndicator(BossNPC);
        } else if (gameObject.GetComponent<IndicatorComponent>()) {
            return gameObject.GetComponent<IndicatorComponent>().MapIndicator;
        } else {
            return new MapIndicator(Player, Color.blue.Lighten(0.75f));
        }
    }
}

public class MapManager : MonoBehaviour {

    static MapManager _instance;

    public static bool Alive {
        get {
            return _instance;
        }
    }

    public static MapManager Instance {
        get {
            if (!_instance) {
                _instance = new GameObject("MapManager").AddComponent<MapManager>();
            }
            return _instance;
        }
    }


    public Dictionary<Transform, MapIndicator> targets { get; private set; }

    public event EventHandler OnElementsChanged;

    bool changed = false;

    void Awake() {
        targets = new Dictionary<Transform, MapIndicator>();
    }

    void Update() {
        if (changed) {
            OnElementsChanged.Raise(this, EventArgs.Empty);
            changed = false;
        }
    }

    void HandleOnQuestNPCCreated(object sender, GameObjectArgs e) {
        AddMapElement(e.Target.transform, MapResourceType.GetTypeForGameObject(e.Target));
    }

    public void RemoveMapElement(GameObject target) {
        if (targets.ContainsKey(target.transform)) {
            targets.Remove(target.transform);
            changed = true;
        }
    }

    public void AddMapElement(Transform mapElementTarget) {
        AddMapElement(mapElementTarget, MapResourceType.GetTypeForGameObject(mapElementTarget.gameObject));
    }

    public void AddPlayerMapElement(string targetName) {
        AddMapElement(GameObject.Find(targetName).transform, MapResourceType.GetTypeForGameObject(gameObject));
    }

    public void AddMapElement(Transform mapElementTarget, MapIndicator type) {
        if (targets.ContainsKey(mapElementTarget)) {
            DestroyEvent.Get(mapElementTarget.gameObject).Destroyed += MapManager_Destroyed;
        }
        targets[mapElementTarget] = type;
        changed = true;
    }

    void MapManager_Destroyed(object sender, GameObjectArgs e) {
        RemoveMapElement(e.Target);
    }

}
