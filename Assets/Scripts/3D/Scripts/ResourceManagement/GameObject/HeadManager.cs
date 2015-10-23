using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class HeadExtensions {
    public static Transform GetHead(this Transform t) {
        return HeadManager.Instance.GetHead(t);
    }

    public static Vector3 GetHeadPosition(this Transform t) {
        var p = GetHead(t).position;
        var headOverride = t.gameObject.GetInterface<IHeadOverride>();
        if (headOverride != null) {
            return p + headOverride.HeadPosition;
        } else {
            return p;
        }
    }
}

public class HeadManager : MonoBehaviour {

    static HeadManager _instance;
    public static HeadManager Instance {
        get {
            if (!_instance) {
                _instance = new GameObject("HeadManager").AddComponent<HeadManager>();
            }
            return _instance;
        }
    }

    Dictionary<Transform, Transform> heads = new Dictionary<Transform, Transform>();

    public Transform GetHead(Transform target) {
        if (!heads.ContainsKey(target) || !heads[target]) {
             if (target.gameObject.GetInterface<IHeadOverride>() != null) {
                heads[target] = target;
            } else {
                heads[target] = target.FindBone("Head");
            }
        }
        return heads[target];
    }

}
