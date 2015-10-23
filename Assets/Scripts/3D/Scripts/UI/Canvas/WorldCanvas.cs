using UnityEngine;
using System.Collections;

[ResourcePath("UI/3DCanvas")]
public class WorldCanvas : MonoBehaviour {

	static WorldCanvas _instance;
    public static WorldCanvas Instance {
        get {
            if (!_instance) {
                _instance = GameObject.FindObjectOfType<WorldCanvas>();
                if (!_instance) {
                    Debug.Log("Producing world canvas.");
                    _instance = GameObjectUtil.GetResourceInstanceFromAttribute<WorldCanvas>();
                }
            }
            return _instance;
        }
    }

}
