using UnityEngine;
using System.Collections;

public class PersonAnimationEventArgs : System.EventArgs {

    public GameObject TargetObject { get; set; }
    public PersonAnimationType AnimationType { get; set; }

    public PersonAnimationEventArgs(GameObject targetObject, PersonAnimationType type) {
        TargetObject = targetObject;
        AnimationType = type;
    }

}
