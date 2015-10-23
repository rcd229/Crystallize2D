using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EmoteChatBoxUI : MonoBehaviour {

    public const string LockString = "EmoteChatUI";

    static string chatHistory = "";

    public Text chatText;
    public RectTransform textBackground;
    public Scrollbar scrollbar;

    // Use this for initialization
    IEnumerator Start() {
        if (GameSettings.GetFlag(LockString)) {
            Destroy(gameObject);
            yield break;
        }

        yield return null;

        MainCanvas.main.Add(transform);
        CrystallizeEventManager.Network.OnEnglishLineInput += HandleEnglishLineInput;
        chatText.text = chatHistory;
    }

    /*void HandleEventManagerInitialized(object sender, System.EventArgs e){
        CrystallizeEventManager.main.OnEnglishLineInput += HandleEnglishLineInput;
        CrystallizeEventManager.main.BeforeSceneChange += HandleBeforeSceneChange;

        transform.SetParent (MainCanvas.main.transform);
        transform.position = new Vector2 (-446f, 10f);
        transform.localPosition = new Vector2 (-446f, transform.localPosition.y);
    }*/

    void HandleEnglishLineInput(object sender, TextEventArgs e) {
        DataLogger.LogTimestampedData("Chat", e.Text);
        chatHistory += "\n" + e.Text;
        chatText.text = chatHistory;
    }

    // Update is called once per frame
    void Update() {
        var textRect = chatText.GetComponent<RectTransform>();
        var size = textRect.rect.height;
        var backSize = textBackground.rect.height - 8f;
        var scrollDist = size - backSize;
        if (scrollDist > 0) {
            scrollbar.size = (size - scrollDist) / size;
            textRect.localPosition = -scrollbar.value * scrollDist * Vector2.up + new Vector2(8f, 4f);
        } else {
            scrollbar.size = 1f;
            textRect.localPosition = new Vector2(8f, 4f);
        }
    }

}
