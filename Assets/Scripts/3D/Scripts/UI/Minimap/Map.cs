using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

[ResourcePath("UI/Map/Map")]
public class Map : UIMonoBehaviour, ITemporaryUI<string, object> {

    Transform mapTarget;

    public float zoomLevel;

    static Map _instance;

    public Slider sizeAdjuster;
    public UIButton ZoomButton;
    public RectTransform childParent;
    bool isMax;


    float baseZoom = 1f;

    float minSize { get { return 150f; } }
    float maxSize { get { return Screen.height * 0.9f; } }

    Dictionary<Transform, GameObject> targetElementMapping;

    public static Map Instance {
        get {
            return _instance;
        }
    }

    void Awake() {
        targetElementMapping = new Dictionary<Transform, GameObject>();
    }

    void OnDestroy() {
        if (MapManager.Alive) {
            MapManager.Instance.OnElementsChanged -= Instance_OnElementsChanged;
        }
    }

    //TODO load map terrain based on scene
    public void Initialize(string param1) {
        //Debug.Log("Opening map: " + Camera.main + " scene: " + Application.loadedLevelName);
        if (!OmniscientCamera.main) {
            Destroy(gameObject);
            return;
        }

        mapTarget = OmniscientCamera.main.transform; 
        _instance = this;

        sizeAdjuster.onValueChanged.AddListener(ChangeSize);
        ZoomButton.OnClicked += HandleOnZoomClicked;
        MapManager.Instance.OnElementsChanged += Instance_OnElementsChanged;
    }

    void Instance_OnElementsChanged(object sender, EventArgs e) {
        Refresh();
    }

    void HandleOnZoomClicked(object sender, EventArgs e) {
        if (isMax) {
            transform.SetParent(null, false);
            MainCanvas.main.PopLayer();
            MainCanvas.main.Add(transform);

            childParent.GetComponent<LayoutElement>().preferredWidth = minSize;
            childParent.GetComponent<LayoutElement>().preferredHeight = minSize;
            rectTransform.pivot = new Vector2(1f, 1f);
            rectTransform.anchoredPosition = new Vector2(-6f, -6f);

            isMax = false;
        } else {
            transform.SetParent(null, false);
            MainCanvas.main.PushLayer();
            MainCanvas.main.Add(transform);

            childParent.GetComponent<LayoutElement>().preferredWidth = maxSize;
            childParent.GetComponent<LayoutElement>().preferredHeight = maxSize;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.position = .5f * new Vector2(Screen.width, Screen.height);

            isMax = true;
        }

        foreach (var go in targetElementMapping.Values) {
            go.GetComponent<MapElement>().SetTextActive(isMax);
        }
    }

    void ChangeSize(float size) {
        zoomLevel = baseZoom * size;
    }

    void Refresh() {
        foreach (var go in targetElementMapping.Values) {
            GameObject.Destroy(go);
        }
        targetElementMapping.Clear();

        foreach (var elem in MapManager.Instance.targets.Keys) {
            var type = MapManager.Instance.targets[elem];
            GameObject go = type.Type.GetInstance();
            var mapEle = go.GetComponent<MapElement>();
            mapEle.target = elem;
            CoroutineManager.Instance.WaitAndDo(() => mapEle.SetTextActive(isMax));
            go.GetComponent<Image>().color = type.Color;
            if (type.Type == MapResourceType.Player) {
                go.GetComponent<MapElement>().rotate = true;
            }
            targetElementMapping[elem] = go;
        }
    }


    public event EventHandler<EventArgs<object>> Complete;

    public void Close() {
        PlayerDataConnector.SetHUDPartActive(HUDPartType.Map, false);
        if (this) {
            GameObject.Destroy(gameObject);
        }
    }

    /**
     * calculate the 2D position of an gameobject with 3D position [otherPos]
     * XRotation is the X axis of the rotated Vector2
     * YRotation is the Y axis of the rotated Vector2
     **/
    public Vector2 Position(Vector3 otherPos) {
        var offset = (otherPos - mapTarget.position);
        offset = new Vector2(offset.x, offset.z);
        var newPosition = Quaternion.AngleAxis(mapTarget.rotation.eulerAngles.y, Vector3.forward) * offset;
        return zoomLevel * newPosition;
    }

    public Quaternion Rotation(Quaternion otherRotation) {
        var newRotation = otherRotation.eulerAngles.y - mapTarget.rotation.eulerAngles.y;
        return Quaternion.Euler(0, 0, 180f - newRotation);
    }
}
