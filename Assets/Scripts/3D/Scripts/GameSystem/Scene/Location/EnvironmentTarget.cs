using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnvironmentTarget : MonoBehaviour {

    GUIStyle style;
    public EnvironmentTarget() {
//        style = new GUIStyle();
//        style.normal.textColor = Color.white;
//        style.alignment = TextAnchor.MiddleCenter;
//        style.fontSize = 18;
//        style.fontStyle = FontStyle.Bold;
    }

    void Start() {
        gameObject.tag = "Place";
    }

	void OnDrawGizmos(){
		style = new GUIStyle();
		style.normal.textColor = Color.white;
		style.alignment = TextAnchor.MiddleCenter;
		style.fontSize = 18;
		style.fontStyle = FontStyle.Bold;
        Gizmos.color = Color.blue;
		Gizmos.DrawSphere(transform.position, 0.25f);
        #if UNITY_EDITOR
		Handles.Label(transform.position + Vector3.up * .5f, transform.name, style);
#endif
	}
}
