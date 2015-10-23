using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class FollowPlayer : MonoBehaviour {

    Rigidbody target;
    float speed = 5f;

    void OnEnabled() {
        if (GetComponent<Rigidbody>()) {
            target = GetComponent<Rigidbody>();
            target.isKinematic = false;
        }
    }

    void FixedUpdate() {
        var force = Vector3.zero;

        if (Vector3.Distance(transform.position, PlayerManager.Instance.PlayerGameObject.transform.position) > 3f){
            var dir = PlayerManager.Instance.PlayerGameObject.transform.position - transform.position;
            var inputDirection = dir.normalized;

            force = inputDirection * speed;
        }

        var vy = target.velocity.y;
        target.velocity = Vector3.MoveTowards(target.velocity, force, speed * 10f * Time.fixedDeltaTime);
        target.velocity = new Vector3(target.velocity.x, vy, target.velocity.z);
    }

}
