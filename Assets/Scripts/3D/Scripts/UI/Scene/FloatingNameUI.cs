using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[ResourcePath("UI/FloatingNames")]
public class FloatingNameUI : MonoBehaviour {

    static FloatingNameUI _instance;
    public static FloatingNameUI GetInstance() {
        if (!_instance) {
            _instance = GameObjectUtil.GetResourceInstanceFromAttribute<FloatingNameUI>();
            MainCanvas.main.Add(_instance.transform);
        }
        return _instance;
    }

    public static void Initialize() { GetInstance(); }

    public GameObject namePrefab;
    public Color impossibleColor = Color.red.Lighten(0.5f);
    public Color hardColor = Color.yellow.Lighten(0.5f);
    public Color normalColor = Color.white;
    public Color easyColor = Color.cyan.Lighten(0.5f);

    Dictionary<Transform, RectTransform> holderNames = new Dictionary<Transform, RectTransform>();
    Dictionary<Transform, Color> colors = new Dictionary<Transform, Color>();
    
    //Dictionary<Transform, Color> holderColors = new Dictionary<Transform, Color>();

	void Awake(){
		CrystallizeEventManager.UI.OnFloatingNameRequested += UI_OnFloatingNameRequested;
	}

    void Start() {
        transform.SetParent(WorldCanvas.Instance.transform, false);
    }

    void OnEnable() {
        foreach (var holder in holderNames.Keys) {
            holderNames[holder].GetComponentInChildren<Text>().text = holderNames[holder].name;
            holderNames[holder].GetComponentInChildren<Text>().color = colors[holder];
        }
    }

    void OnDestroy() {
        CrystallizeEventManager.UI.OnFloatingNameRequested -= UI_OnFloatingNameRequested;
    }

    // Update is called once per frame
    void Update() {
        if (!PlayerManager.Instance.PlayerGameObject) {
            return;
        }

        var player = PlayerManager.Instance.PlayerGameObject.transform;
        var cameraFront = new Plane(Camera.main.transform.forward, Camera.main.transform.position);
        foreach (var h in holderNames.Keys) {
            var pos = h.transform.GetHeadPosition() + Vector3.up * (0.35f);// + 0.03f * sine);//0.38f;
            holderNames[h].position = pos;
            holderNames[h].forward = Camera.main.transform.forward;
        }
    }

    void UI_OnFloatingNameRequested(object sender, FloatingNameEventArgs e) {
        SetName(e.Holder, e.Name);
        SetColor(e.Holder, e.Color);
    }

    public void SetName(Transform holder, string name) {
        if (!holderNames.ContainsKey(holder)) {
            var instance = Instantiate(namePrefab) as GameObject;
            instance.transform.SetParent(transform, false);
            instance.transform.localScale = 0.0065f * Vector3.one;
            DestroyEvent.Get(holder.gameObject).Destroyed += (s, e) => Holder_Destroyed(holder);
            holderNames[holder] = instance.GetComponent<RectTransform>();
        }

        holderNames[holder].name = name;
        if (gameObject.activeInHierarchy) {
            holderNames[holder].GetComponentInChildren<Text>().text = name;
        }
    }

    void Holder_Destroyed(Transform holder) {
        if (holderNames[holder]) {
            Destroy(holderNames[holder].gameObject);
            holderNames.Remove(holder);
        }
    }

    public RectTransform GetName(Transform holder) {
        if (holderNames.ContainsKey(holder)) {
            return holderNames[holder];
        }
        return null;
    }

    public void SetColor(Transform holder, Color color) {
        if (!holderNames.ContainsKey(holder)) {
            SetName(holder, "???");
        }

        colors[holder] = color;
        if (holderNames[holder].gameObject.activeInHierarchy) {
            holderNames[holder].GetComponentInChildren<Text>().color = color;
        }
        //holderColors[]
    }

}
