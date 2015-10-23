using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class ActorParent {
    public static GameObject Get(GameObject target) {
        if (target == null || target.transform.parent == null) { return target; }
        if (target.transform.parent.gameObject.GetInterface<IActorParent>() != null) { return target.transform.parent.gameObject; }
        return target;
    }
}

public interface IActorParent { }
