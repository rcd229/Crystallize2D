using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Object2DBehaviour(typeof(SceneChangeTrigger2D))]
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
