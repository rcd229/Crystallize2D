using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// If you want a behaviour to be attached to a type of object, use this attribute
[Object2DBehaviour(typeof(SceneChangeTrigger2D))]
// All behaviours must derive from monobehaviour
public class SceneChangeTrigger2DBehaviour : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            var trigger = GetComponent<Object2DComponent>().Object as SceneChangeTrigger2D;

            var objs = Object2DLoader.LoadAll(trigger.Scene);
            var target = (from o in objs
                          where o.Guid == trigger.Target 
                          select o)
                         .FirstOrDefault();

            if (target != null) {
                other.transform.position = (Vector2)target.Position;
            }
            // load the other scene here
        }
    }
}
