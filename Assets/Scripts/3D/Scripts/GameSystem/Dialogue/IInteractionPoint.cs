using UnityEngine;
using System.Collections;

public interface IInteractionPoint {

    bool GetInteractionEnabled();
    Vector3 GetPosition();
    float GetRadius();

}
