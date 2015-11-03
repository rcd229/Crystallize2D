using UnityEngine; 
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public static class GameObjectUtil {

    static GUIStyle style;
    public static void DoTargetGizmo(Transform transform, string name, Color color) {
        style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 18;
        style.fontStyle = FontStyle.Bold;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, 0.25f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + transform.forward * 0.3f, 0.05f);
#if UNITY_EDITOR
        Handles.Label(transform.position + Vector3.up * .5f, name, style);
#endif
    }

    public static GameObject GetResourceInstance(string resourcePath) {
        return GameObject.Instantiate<GameObject>(
            Resources.Load<GameObject>(resourcePath));
    }

    public static C GetResourceInstance<C>(string resourcePath) where C : Component {
        return GameObject.Instantiate<GameObject>(
            Resources.Load<GameObject>(resourcePath))
            .GetComponent<C>();
    }

    public static C GetResourceInstanceFromAttribute<C>() where C : Component {
        var type = typeof(C);
        if (!type.HasAttribute<ResourcePathAttribute>()) {
            Debug.LogError(type + " does not have resource path attribute");
            return null;
        }
        var resourcePath = type.GetAttribute<ResourcePathAttribute>();
        return GameObject.Instantiate<GameObject>(
            Resources.Load<GameObject>(resourcePath.ResourcePath))
            .GetComponent<C>();
    }

	///require both lists to be none-empty
	public static List<GameObject> RandomAssignPrefabToTarget(IEnumerable<string> parentNames, IEnumerable<GameObject> prefabSet){
        return RandomAssignInstancesToTargets(parentNames, prefabSet.Select(p => GameObject.Instantiate<GameObject>(p)));
	}

    public static List<GameObject> RandomAssignInstancesToTargets(IEnumerable<string> parentNames, IEnumerable<GameObject> instanceSet) {
        var parents = parentNames.Randomize();
        var instances = new List<GameObject>(instanceSet);
        if (parents.Count < instances.Count) {
            Debug.LogWarning("Not enough parents were provided.");
        }

        var places = GameObject.FindGameObjectsWithTag("Place");
        var count = Mathf.Min(parents.Count, instances.Count);
        //map the elements
        //instantiate each of the prefabs
        for (int i = 0; i < count; i++) {
            var parentObj = places.Where((g) => g.name == parents[i]).FirstOrDefault();
            var instance = instances[i];
            //Debug.Log("assigning prefab to " + parentObj.name + "; " + instance.name);
            instance.transform.parent = parentObj.transform;
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
            instances.Add(instance);
        }
        return instances;
    }

    public static bool ResourceExists(string resourcePath) {
        return Resources.Load(resourcePath) != null;
    }

    public static void DestroyAndClear(this List<GameObject> objs) {
        foreach (var go in objs) {
            GameObject.Destroy(go);
        }
        objs.Clear();
    }

    public static GameObject FindGameObjectWithNameAndTag(string name, string tag) {
        foreach (var go in GameObject.FindGameObjectsWithTag(tag)) {
            if (go.name == name) {
                return go;
            }
        }
        return null;
    }

	public static C InstantiateAtPlace<C>(string resourcePath, Transform location) where C : Component{
		GameObject go = GetResourceInstance(resourcePath);
		PutAtPlace(go, location);
		return go.GetComponent<C>();
	}

	public static GameObject InstantiateAtPlace(string resourcePath, Transform location) {
		GameObject go = GetResourceInstance(resourcePath);
		PutAtPlace(go, location);
		return go;
	}

    public static void SetTransform(this Transform t, Transform other) {
        t.position = other.position;
        t.rotation = other.rotation;
    }

    public static void SetTransform(this Transform t, Vector3 position, Quaternion rotation) {
        t.position = position;
        t.rotation = rotation;
    }

	public static void PutAtPlace(GameObject go, Transform location){
		go.transform.position = location.position;
		go.transform.rotation = location.rotation;
	}

    public static T NewWithComponent<T>() where T : Component {
        return new GameObject(typeof(T).ToString()).AddComponent<T>();
    }

}
