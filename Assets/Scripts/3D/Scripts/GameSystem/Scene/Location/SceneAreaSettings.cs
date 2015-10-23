using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public static class SceneAreaUtil {

    const string blockedLayer = "Blocked";

    static List<GameObject> targets = new List<GameObject>();

    public static List<string> ScatterTargets(string areaName, int count, Vector3 awayFromTarget) {
        foreach(var t in targets){
            if(t){
                GameObject.Destroy(t);
            }
        }
        targets.Clear();

        var areaGO = SceneAreaManager.Instance.Get(areaName);//GameObject.Find(areaName);
        if (!areaGO) {
            Debug.LogError("Unable to find " + areaName);
            return null;
        }

        var names = new List<string>();
        var area = SceneAreaSettings.Get(areaGO);
        int index = 0;
        foreach (var p in area.ScatterPoints(count, awayFromTarget)) {
            var go = GameObjectUtil.GetResourceInstance("Target01");
            go.name = string.Format("GeneratedTarget{0:D2}", index);
            go.transform.position = p;
            targets.Add(go);
            names.Add(go.name);
            index++;
        }
        return names;
    }

	public static List<string> ClearAndScatterTargets(string areaName, List<GameObject> gos, Vector3 awayFromTarget) {
		foreach(var t in targets){
			if(t){
				GameObject.Destroy(t);
			}
		}
		targets.Clear();
		
		return ScatterTargets(areaName, gos, awayFromTarget);
	}

	public static List<string> ScatterTargets(string areaName, List<GameObject> gos, Vector3 awayFromTarget) {
		var areaGO = SceneAreaManager.Instance.Get(areaName);//GameObject.Find(areaName);
		if (!areaGO) {
			Debug.LogError("Unable to find " + areaName);
			return null;
		}
		
		var names = new List<string>();
		var area = SceneAreaSettings.Get(areaGO);
		int index = 0;
		foreach (var p in area.ScatterPoints(gos.Count, awayFromTarget)) {
			if(gos.Count <= index){
				break;
			}
			var go = gos[index];
			go.transform.position = p;
			targets.Add(go);
			names.Add(go.name);
			index++;
		}
		return names;
	}


    public static bool ScatterPoint(this Rect rect, float height, out Vector3 point) {
        point = Vector3.zero;
        int maxTimes = 50;
        while (maxTimes >= 0) {
            var y = height;
            var x = UnityEngine.Random.Range(rect.xMin, rect.xMax);
            var z = UnityEngine.Random.Range(rect.yMin, rect.yMax);
            var newP = new Vector3(x, y, z);
            if (CanAdd(newP, out newP)) {
                point = newP;
                return true;
            }

            maxTimes--;
        }
        return false;
    }

    static bool CanAdd(Vector3 point, out Vector3 hitPoint) {
        hitPoint = Vector3.zero;

        var ray = new Ray(point, Vector3.down);
        var hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 100f)) {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer(blockedLayer)) {
                return false;
            } else if (hit.rigidbody) {
                return false;
            } else {
                hitPoint = hit.point;
                return true;
            }
        } else {
            //Debug.Log("nothing hit");
            return false;
        }
    }

    public static List<Vector3> ScatterPoints(this Rect rect, int count, float height, Rect notRect = new Rect()) {
        var current = new List<Vector3>();
        int maxTimes = 50;
        while (maxTimes >= 0 && current.Count < count) {
            var y = height;
            var x = UnityEngine.Random.Range(rect.xMin, rect.xMax);
            var z = UnityEngine.Random.Range(rect.yMin, rect.yMax);
            var newP = new Vector3(x, y, z);

            if (!notRect.Contains(newP.XZToVector2()) && CanAdd(current, 4f, newP, out newP)) {
                current.Add(newP);
            }

            maxTimes--;
        }
        return current;
    }

    static bool CanAdd(IEnumerable<Vector3> currentPoints, float minDistance, Vector3 point, out Vector3 hitPoint) {
        hitPoint = Vector3.zero;

        var sqrDist = minDistance * minDistance;
        foreach (var p in currentPoints) {
            if (Vector2.SqrMagnitude(point.XZToVector2() - p.XZToVector2()) < sqrDist) {
                return false;
            }
        }
        
        var ray = new Ray(point, Vector3.down);
        var hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 100f)) {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer(blockedLayer)) {
                return false;
            } else {
                hitPoint = hit.point;
                return true;
            }
        } else {
            return false;
        }
    }

}

public class SceneAreaSettings : MonoBehaviour {

    public static SceneAreaSettings Get(GameObject obj) {
        var s = obj.GetComponent<SceneAreaSettings>();
        if (!s) {
            s = obj.AddComponent<SceneAreaSettings>();
        }
        return s;
    }

    float Height {
        get {
            return transform.localScale.y * 2f;
        }
    }

    string blockedLayer = "Blocked";

    void Start() {
        UILibrary.Message.Set(UILibrary.GetMessageInstance);
        GenerateBoundary();
        SceneAreaManager.Instance.Add(gameObject);
    }

    void GenerateBoundary() {
        var b = Corners();
        var line = GameObjectUtil.GetResourceInstance<LineRenderer>(EffectLibrary.AreaLine);
        line.transform.parent = transform;
        for (int i = 0; i < 5; i++) {
            line.SetPosition(i, b[i % 4] + Vector3.up *0.5f);
        }
    }

    Vector3[] Corners() {
        var y = transform.position.y + 0.1f;// -transform.localScale.y;
        var x = transform.position.x;
        var xExt = transform.localScale.x;
        var z = transform.position.z;
        var zExt = transform.localScale.z;
        return new Vector3[]{
            new Vector3(x + xExt, y, z + zExt),
            new Vector3(x + xExt, y, z - zExt),
            new Vector3(x - xExt, y, z - zExt),
            new Vector3(x - xExt, y, z + zExt)
        };
    }

    Vector3[] CornersWithEdge(float edge) {
        var y = transform.position.y + 0.1f;// -transform.localScale.y;
        var x = transform.position.x;
        var xExt = transform.localScale.x;
        var z = transform.position.z;
        var zExt = transform.localScale.z;
        return new Vector3[]{
            new Vector3(x + xExt - edge, y, z + zExt - edge),
            new Vector3(x + xExt - edge, y, z - zExt + edge),
            new Vector3(x - xExt + edge, y, z - zExt + edge),
            new Vector3(x - xExt + edge, y, z + zExt - edge)
        };
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.IsPlayer()) {
            UILibrary.Message.Get("You probably shouldn't go that way now.");
        }
    }

    public IEnumerable<Vector3> ScatterPoints(int count, Vector3 awayFromTarget = default(Vector3)) {
        var scatterPoints = new List<Vector3>();
        float minDistance = 3f;
        float edge = 3f;
        int maxTimes = 50;
        while (scatterPoints.Count < count) {
            var y = transform.position.y + transform.localScale.y;
            var x = UnityEngine.Random.Range(
                transform.position.x - transform.localScale.x + edge, 
                transform.position.x + transform.localScale.x - edge);
            var z = UnityEngine.Random.Range(
                transform.position.z - transform.localScale.z + edge, 
                transform.position.z + transform.localScale.z - edge);
            var newP = new Vector3(x, y, z);
            var awayDist = 5f;
            if (awayFromTarget == default(Vector3)) {
                awayDist = 0;
            }
            if (CanAdd(scatterPoints, newP, minDistance, awayFromTarget, awayDist, out newP)) {
                scatterPoints.Add(newP);
            } else {
                maxTimes--;
            }

            if (maxTimes <= 0) {
                Debug.Log("Too many tries. Try increasing the area size.");
                break;
            }
        }
        return scatterPoints;
    }

    bool CanAdd(List<Vector3> points, Vector3 point, float minDistance, Vector3 awayFromTarget, float awayDist, out Vector3 hitPoint) {
        hitPoint = Vector3.zero;
        if (Vector3.Distance(awayFromTarget.ToXZ(), point.ToXZ()) < awayDist) {
            return false;
        }
        
        foreach (var p in points) {
            if (Vector3.Distance(p.ToXZ(), point.ToXZ()) < minDistance) {
                //Debug.Log("too close: " + p + "; " + point);
                return false;
            }
        }

        var ray = new Ray(point, Vector3.down);
        var hit = new RaycastHit(); 
        if (Physics.Raycast(ray, out hit, Height)) {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer(blockedLayer)) {
                return false;
            } else {
                hitPoint = hit.point;
                return true;
            }
        } else {
            return false;
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = new Color(0, 1f, 0, 0.5F);
        Gizmos.DrawCube(transform.position, transform.localScale * 2f);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = new Color(1f, 1f, 0, 0.75f);
        foreach (var p in CornersWithEdge(3f)) {
            Gizmos.DrawSphere(p + Vector3.up * 0.5f, 0.125f);
        }
    }

}
