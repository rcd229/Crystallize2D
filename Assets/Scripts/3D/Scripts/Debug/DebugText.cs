using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// This is used for any output that you want to be visible in real time. Can also be seen in the build.
/// By default, this is shown using the insert key.
/// </summary>
public class DebugText : MonoBehaviour {

    // Use this for initialization
    void Start() {
        CrystallizeEventManager.Debug.OnDebugTextRequested += HandleDebugTextRequested;

        gameObject.SetActive(false);
    }

    void HandleDebugTextRequested(object sender, System.EventArgs args) {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    // Update is called once per frame
    void Update() {
        var s = "";
        s = AddLine(s, "Center panels:");
        foreach (var p in UISystem.main.CenterPanels) {
            s = AddLine(s, "\t" + p.ToString());
        }
        GetComponent<Text>().text = s;
    }

    string AddLine(string s1, string s2) {
        return s1 + "\n" + s2;
    }

}
