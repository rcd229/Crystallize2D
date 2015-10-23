using UnityEngine;
using System.Collections;

public class SlidingDoorTrigger : MonoBehaviour {

    public Transform door;
    public Transform target;
    bool isNear = false;

    Vector3 originalPosition;

    // Use this for initialization
    void Start() {
        originalPosition = door.transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (isNear) {
            door.transform.position = Vector3.MoveTowards(door.transform.position, target.position, 3f * Time.deltaTime);
        } else {
            door.transform.position = Vector3.MoveTowards(door.transform.position, originalPosition, 3f * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.IsPlayer()) {
            isNear = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.IsPlayer()) {
            isNear = false;
        }
    }
}
