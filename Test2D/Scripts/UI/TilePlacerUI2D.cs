using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Util;

[ResourcePath("UI/TilePlacer")]
public class TilePlacerUI2D : MonoBehaviour, ITemporaryUI<object, object> {

    public Text text;

    bool show = true;
    int selected = 0;

    Vector2int triggerOrigin;
    GameObject triggerObject;

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(object args1) {
        
    }

    public void Close() {
        Destroy(gameObject);
    }

    void Update() {
        //text.gameObject.SetActive(show);
        //if (show) {
        //    var s = "Commands";
        //    s += WrapString(1, "Add Tile", KeyCode.Alpha1, PlaceTile);
        //    s += WrapString(2, "Remove Tile", KeyCode.Alpha2, RemoveTile);
        //    s += WrapString(3, "Place Thing", KeyCode.Alpha3, onMouseDown: PlaceThing);
        //    s += WrapString(4, "Place trigger", KeyCode.Alpha4, UpdatePlaceTrigger, BeginPlaceTrigger, EndPlaceTrigger);
        //    s += WrapString(5, "Refresh", KeyCode.Alpha5, onMouseDown: TileResourceManager.Instance.Refresh);

        //    text.text = s;
        //}

        if (Input.GetMouseButtonDown(1)) {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
            Debug.Log(hit.transform);
            var editor = GetEditor(hit);
            if (editor != null) {
                editor.GetEditor();
            }
        }
    }

    IHasSceneEditor GetEditor(RaycastHit2D hit) {
        if (!hit.transform) return null;
        var e = hit.transform.gameObject.GetInterface<IHasSceneEditor>();
        if (e != null) return e;
        if (!hit.collider.attachedRigidbody) return null;
        return hit.collider.attachedRigidbody.gameObject.GetInterface<IHasSceneEditor>();
    }

    public static void PlaceTile(SpriteLayer layer) {
        var mPos = GetMapPositionFromMousePosition();
        //TileMap2D.pathInstance.SetValue(mPos, 1);
        TileResourceManager.Instance.AddTile(mPos);
    }

    public static void RemoveTile(SpriteLayer layer) {
        var mPos = GetMapPositionFromMousePosition();
        //TileMap2D.pathInstance.SetValue(mPos, 0);
        TileResourceManager.Instance.RemoveTile(mPos);
    }

    public static void PlaceThing() {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        AddThing(pos);
    }

    public static void PlaceThing(ThingTemplate2D template) {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        AddThing(template, pos);
    }

    public static void RemoveThing() {
        var hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
        //Debug.Log(hit.transform);
        foreach (var hit in hits) {
            if (hit.transform && hit.transform.GetComponent<SceneThing2D>()) {
                var t = hit.transform.GetComponent<SceneThing2D>().Thing;
                ThingResourceManager2D.Instance.Remove(t);
            }
        }
    }

    void BeginPlaceTrigger() {
        triggerOrigin = GetMapPositionFromMousePosition();
        var t = GameObjectUtil.GetResourceInstanceFromAttribute<SceneTrigger2D>();
        t.Initialize(new TriggerData2D());
        triggerObject = t.gameObject;
    }

    void UpdatePlaceTrigger() {
        if (triggerOrigin == null) return;

        var triggerEnd = GetMapPositionFromMousePosition();
        var size = 0;
        var orientation = 0;
        var offset = triggerEnd - triggerOrigin;
        if (triggerOrigin != triggerEnd) {
            if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y)) {
                if (offset.x > 0) {
                    orientation = 0;
                } else {
                    orientation = 180;
                }
                size = Mathf.Abs(offset.x) + 1;
                offset = new Vector2int(offset.x, 0);
            } else {
                if (offset.y > 0) {
                    orientation = 90;
                } else {
                    orientation = 270;
                }
                size = Mathf.Abs(offset.y) + 1;
                offset = new Vector2int(0, offset.y);
            }
        }

        triggerObject.transform.position = Vector2.Lerp(GetWorldPositionFromMapPosition(triggerOrigin), GetWorldPositionFromMapPosition(triggerOrigin + offset), 0.5f);
        triggerObject.transform.rotation = Quaternion.Euler(0, 0, orientation);
        triggerObject.transform.localScale = new Vector3(size, 1f, 1f);
    }

    void EndPlaceTrigger() {
        UpdatePlaceTrigger();
        triggerOrigin = default(Vector2int);
        triggerObject = null;
        TriggerLoader2D.Instance.SaveAll();
    }

    static void AddThing(Vector2 position) {
        var thing = new ThingInstance2D();
        thing.Position = position;
        ThingResourceManager2D.Instance.Add(thing);
    }

    static void AddThing(ThingTemplate2D template, Vector2 position) {
        var thing = new ThingInstance2D(template);
        thing.Position = position;
        ThingResourceManager2D.Instance.Add(thing);
    }

    //string WrapString(int index, string s, KeyCode keyCode, Action onMouse = null, Action onMouseDown = null, Action onMouseUp = null) {
    //    if (Input.GetKeyDown(keyCode)) { selected = index; }

    //    if (index == selected) {
    //        //if (Input.GetMouseButton(0)) { onMouse.Raise(); }
    //        //if (Input.GetMouseButtonDown(0)) { onMouseDown.Raise(); }
    //        //if (Input.GetMouseButtonUp(0)) { onMouseUp.Raise(); }
    //        return "\n<b>" + index + ": " + s + "</b>";
    //    } else {
    //        return "\n" + index + ": " + s;
    //    }
    //}

    static Vector2int GetMapPositionFromMousePosition() {
        return TileResourceManager.GetMapPositionFromWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    static Vector2int GetMapPositionFromWorldPosition(Vector2 worldPosition) {
        return TileResourceManager.GetMapPositionFromWorldPosition(worldPosition);
    }

    public static Vector2 GetWorldPositionFromMapPosition(Vector2int mapPosition) {
        return TileResourceManager.GetWorldPositionFromMapPosition(mapPosition);
    }

}
