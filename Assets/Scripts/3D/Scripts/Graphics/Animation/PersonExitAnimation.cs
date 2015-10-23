using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class PersonExitAnimation : MonoBehaviour {

    float speed = 3f;

    IEnumerator Start() {
        if (GetComponent<DialogueActor>()) {
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        if(GetComponent<Rigidbody>()){
            GetComponent<Rigidbody>().isKinematic = true;
            foreach (var c in GetComponentsInChildren<Collider>()) {
                c.enabled = false;
            }
        }

        var exits = GameObject.FindGameObjectsWithTag("Exit");
        Vector3 exit = Vector3.zero;
        if (exits.Length > 0) {
            exit = gameObject.Nearest<Transform>(exits.Select((e) => e.transform)).position;
        }
        exit.y = transform.position.y;

        while (Vector3.Distance(transform.position, exit) > 1f) {
            transform.position = Vector3.MoveTowards(transform.position, exit, speed * Time.deltaTime);
            //transform.forward = exit - transform.position;
            yield return null;
        }

        Destroy(gameObject);
    }

}
