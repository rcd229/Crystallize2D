using UnityEngine;
using System.Collections;

public class FloatingNameHolder : MonoBehaviour, IFloatingNameHolder {

    // Use this for initialization
    void Start() {
        SetName("???");
    }

    public void SetName(string name) {
        FloatingNameUI.GetInstance().SetName(transform, name);
    }

}
