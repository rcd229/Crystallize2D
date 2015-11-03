using UnityEngine;
using System.Collections;

public class Object2DBehaviourMap : TypeAttributeMap<Object2D, MonoBehaviour, Object2DBehaviourAttribute> {
    public static readonly Object2DBehaviourMap Instance = new Object2DBehaviourMap();
}
