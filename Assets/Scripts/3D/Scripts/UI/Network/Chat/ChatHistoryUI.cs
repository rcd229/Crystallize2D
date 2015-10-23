using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChatHistoryUI : MonoBehaviour {

    public GameObject chatLinePrefab;
    public RectTransform chatLineParent;
    public RectTransform textArea;
    public Scrollbar scroll;

    public Color selfColor = Color.white;
    public Color otherColor = Color.white;
    public Color npcColor = Color.white;

	// Use this for initialization
	IEnumerator Start () {
        //if (!LevelSettings.main.isMultiplayer) {
        //    gameObject.SetActive(false);
        //    yield break;
        //}
        yield return null;

        CrystallizeEventManager.UI.OnSpeechBubbleRequested += HandleSpeechBubbleRequested;
	}
	
	// Update is called once per frame
	void Update () {
        var height = textArea.rect.height;
        var overflowHeight = chatLineParent.rect.height - textArea.rect.height;
        if(overflowHeight < 0){
            scroll.size = 1f;
        } else {
            scroll.size = height / (height + overflowHeight);
        }
        chatLineParent.localPosition = -1f * Vector2.up * scroll.value * overflowHeight;
	}

    void HandleSpeechBubbleRequested(object sender, SpeechBubbleRequestedEventArgs e) {
        if (e.Phrase != null) {
            var name = ActorTracker.GetName(e.Target.gameObject);
            var text = e.Phrase.GetText(JapaneseTools.JapaneseScriptType.Romaji);
            var type = ActorTracker.GetActorType(e.Target.gameObject);
            AddLine(name, text, GetColorForType(type));
        }
    }

    void AddLine(string name, string text, Color color) {
        var newLine = Instantiate<GameObject>(chatLinePrefab);
        newLine.transform.SetParent(chatLineParent);
        newLine.GetComponent<Text>().text = string.Format("{0}: {1}", name, text);
        newLine.GetComponent<Text>().color = color;
    }

    Color GetColorForType(ActorType type) {
        switch (type) {
            case ActorType.Self:
                return selfColor;
            case ActorType.Other:
                return otherColor;
            default:
                return npcColor;
        }
    }

}
