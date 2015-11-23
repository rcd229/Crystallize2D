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
        var rigidbody = other.GetComponent<Rigidbody2D>();
        if (other.tag == "Player" && rigidbody != null & rigidbody.velocity.y > 0) {

            var trigger = GetComponent<Object2DComponent>().Object as SceneChangeTrigger2D;
            Scene2DTransitions.Instance.TransitionToScene(trigger);

        }
    }
}
