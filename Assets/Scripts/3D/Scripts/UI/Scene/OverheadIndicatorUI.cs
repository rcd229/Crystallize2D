using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

[ResourcePath("UI/OverheadIndicator")]
public class OverheadIndicatorUI : MonoBehaviour {

    static OverheadIndicatorUI _instance;
    public static OverheadIndicatorUI Instance {
        get {
            if (!_instance) {
                _instance = GameObjectUtil.GetResourceInstanceFromAttribute<OverheadIndicatorUI>();
            }
            return _instance;
        }
    }

    public GameObject indicatorPrefab;

    //Vector3 offset = 2f * Vector3.up;
    Dictionary<Transform, Transform> indicators = new Dictionary<Transform, Transform>();

    void Start() {
        //MainCanvas.main.Add(transform);
        transform.SetParent(WorldCanvas.Instance.transform, false);
        transform.position = Vector3.zero;
        //offset = transform.GetHead().position + Vector3.up * 0.38f;
    }

    void Update() {
        // TODO: combine with floating name
        if (!PlayerManager.Instance.PlayerGameObject) {
            return;
        }

        var player = PlayerManager.Instance.PlayerGameObject.transform;
        var cameraFront = new Plane(Camera.main.transform.forward, Camera.main.transform.position);
        //var sine = Mathf.Sin(Time.time * 3f);
        foreach (var h in indicators.Keys) {
            var pos = h.transform.GetHeadPosition() + Vector3.up * (0.625f);// + 0.03f * sine);//0.38f;
            indicators[h].position = pos;
            indicators[h].forward = Camera.main.transform.forward;
            //if (cameraFront.GetSide(pos)) {
            //    var d = Vector3.Distance(player.position, h.transform.position);
            //    if (d < 20f) {
            //        indicators[h].GetComponent<CanvasGroup>().alpha = 1f - ((d - 5f) / 15f);
            //        indicators[h].position = Camera.main.WorldToScreenPoint(pos);
            //        indicators[h].localScale = (1f - (d / 25f)) * Vector3.one;
            //    } else {
            //        indicators[h].GetComponent<CanvasGroup>().alpha = 0;
            //    }
            //} else {
            //    indicators[h].GetComponent<CanvasGroup>().alpha = 0;
            //}
        }
    }

    public void SetIndicator(Transform target, Sprite sprite, Color color, bool isNew = false) {
        if (indicators.ContainsKey(target)) {
            Destroy(indicators[target].gameObject);
        }

        var instance = Instantiate<GameObject>(indicatorPrefab);
        instance.GetComponent<Image>().sprite = sprite;
        instance.GetComponent<Image>().color = color;
        instance.transform.localScale = 0.006f * Vector3.one;
        instance.transform.SetParent(transform, false);
        instance.transform.GetChild(0).gameObject.SetActive(isNew);
        DestroyEvent.Get(target.gameObject).Destroyed += (s, e) => OverheadIndicatorUI_Destroyed(target);
        indicators[target] = instance.transform;
    }

    void OverheadIndicatorUI_Destroyed(Transform target) {
        if (indicators.ContainsKey(target)) {
            if (indicators[target]) {
                Destroy(indicators[target].gameObject);
                indicators.Remove(target);
            }
        }
    }

}
