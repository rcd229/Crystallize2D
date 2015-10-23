using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class SpeechPanelExtensions {
    const string SpeechBubbleTargetTag = "SpeechBubbleTarget";
    static Vector3 DefaultOffset = new Vector3(0, 1.8f, 0);

    public static Transform GetSpeechBubbleTarget(this Transform transform) {
        var target = transform.FindChildWithTag(SpeechBubbleTargetTag);
        if (target == null) {
            target = new GameObject(SpeechBubbleTargetTag).transform;
            target.SetParent(transform, false);
            target.localPosition = DefaultOffset;
            target.tag = SpeechBubbleTargetTag;
        }
        return target;
    }
}

public class SpeechPanelUI : MonoBehaviour {

    const string ResourcePath = "UI/SpeechPanel";
	const float Padding = 2f;

    static SpeechPanelUI _instance;
	public static SpeechPanelUI Instance {
        get {
            if (!_instance) {
                _instance = GameObjectUtil.GetResourceInstance<SpeechPanelUI>(ResourcePath);
            }
            return _instance;
        }
    }

    public static SpeechPanelUI GetInstance() {
        return Instance;
    }

	public GameObject normalSpeechBubblePrefab;
	public GameObject playerSpeechBubblePrefab;

    Dictionary<Transform, GameObject> speechBubbleInstances = new Dictionary<Transform, GameObject>();

	void Awake(){
		_instance = this;
	}

    void Start() {
        MainCanvas.main.Add(transform, CanvasBranch.None);
        CrystallizeEventManager.UI.OnSpeechBubbleRequested += HandleSpeechBubbleRequested;
    }

    void HandleSpeechBubbleRequested(object sender, SpeechBubbleRequestedEventArgs e) {
        if (e.Phrase == null) {
            RemoveSpeechBubble(e.Target);
        } else {
            var go = GetSpeechBubble(e.Target, e.Phrase, e.SpeechBubblePointerType);
            e.Callback.Raise(go);
            CoroutineManager.Instance.WaitAndDo(() => CrystallizeEventManager.UI.RaiseSpeechBubbleOpen(go, e));
        }
    }

    public GameObject GetSpeechBubble(Transform target, PhraseSequence phrase, PointerType pointerType) {
        var go = Instantiate(normalSpeechBubblePrefab) as GameObject;
        go.transform.SetParent(transform);
        go.GetComponent<SpeechBubbleUI>().Initialize(target, phrase, pointerType);

        if (speechBubbleInstances.ContainsKey(target)) {
            Destroy(speechBubbleInstances[target]);
        }
        speechBubbleInstances[target] = go;

        return go;
    }

    public void RemoveSpeechBubble(Transform target) {
        if (speechBubbleInstances.ContainsKey(target)) {
            Destroy(speechBubbleInstances[target]);
            speechBubbleInstances.Remove(target);
        }
    }

	void Update(){
		AdjustBubbles ();
	}

	void AdjustBubbles(){
		var bubbles = new HashSet<SpeechBubbleUI> (GetComponentsInChildren<SpeechBubbleUI>());
		if (bubbles.Count <= 1) {
			return;
		}

        if (Overlaps(bubbles)) {
            SetBubblesOutward(bubbles);
        } else {
            SetBubblesInward(bubbles);
        }

		//var sortedBubbles = new List<SpeechBubbleUI> ();
		SpeechBubbleUI previous = null;
		while(bubbles.Count > 0){
			var lowest = bubbles.First();
			foreach(var b in bubbles){
				if(b.rectTransform.position.y < lowest.rectTransform.position.y){
					lowest = b;
				}
			}
			bubbles.Remove(lowest);

			if(previous != null){
				var r1 = previous.rectTransform.rect;
				r1.position += previous.RootPosition + previous.FlipOffset;
				var r2 = lowest.rectTransform.rect;
				r2.position += lowest.RootPosition + lowest.FlipOffset;

				if(r1.Overlaps(r2)){
					var top = previous.rectTransform.position.y + previous.rectTransform.rect.height + Padding;
					var diff = top - lowest.RootPosition.y;
					lowest.TargetVerticalOffset = diff * Vector2.up;
				} else {
					lowest.TargetVerticalOffset = Vector2.zero;
				}
			} else {
				lowest.TargetVerticalOffset = Vector2.zero;
			}
			previous = lowest;
		}
	}

    bool Overlaps(HashSet<SpeechBubbleUI> bubbleSet) {
        if (bubbleSet.Count <= 1) {
            return false;
        }

        var bubbles = new Queue<SpeechBubbleUI>(bubbleSet);
        while (bubbles.Count > 1) {
            var b1 = bubbles.Dequeue();
            foreach (var b2 in bubbles) {
                if (b1.DoubleRect.Overlaps(b2.DoubleRect)) {
                    return true;
                }
            }
        }
        return false;
    }

    void SetBubblesOutward(IEnumerable<SpeechBubbleUI> bubbles) {
        var minBubble = bubbles.First();
        foreach (var bubble in bubbles) {
            bubble.Flipped = true;
            if (bubble.RootPosition.x < minBubble.RootPosition.x) {
                minBubble = bubble;
            }
        }
        minBubble.Flipped = false;
    }

    void SetBubblesInward(IEnumerable<SpeechBubbleUI> bubbles) {
        var minBubble = bubbles.First();
        foreach (var bubble in bubbles) {
            bubble.Flipped = false;
            if (bubble.RootPosition.x < minBubble.RootPosition.x) {
                minBubble = bubble;
            }
        }
        minBubble.Flipped = true;
    }

}
