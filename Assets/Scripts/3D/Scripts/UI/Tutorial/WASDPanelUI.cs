using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ResourcePath("UI/Tutorial/WASDPanel")]
public class WASDPanelUI : BaseTemporaryUI<object, object> {

    public List<WASDButtonUI> buttons = new List<WASDButtonUI>();

    // Use this for initialization
    void Start() {
        transform.position = new Vector2(Screen.width * 0.5f, 300f);
    }

    // Update is called once per frame
    void Update() {
        foreach (var button in buttons) {
            button.SetState(false);
        }

        int activeButton = ((int)Time.time) % 4;
        buttons[activeButton].SetState(true);
    }
}
