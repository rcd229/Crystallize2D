using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using JapaneseTools;

public enum PointerType {
    Normal,
    Phone
}

public class SpeechBubbleUI : MonoBehaviour, IPointerClickHandler {

    const float MinY = 120f;
    const float MinZ = 1f;
    

    public static SpeechBubbleUI LastSpeechBubble { get; private set; }

    public GameObject editButton;
    public GameObject bookmarkButton;
    // TODO: probably want to reference GUIPallet
    public Color wrongColor;

    public Text translationText;
    public GameObject wordButtonPrefab;
    public GameObject wordsPanel;
    public Transform target;
    public PhraseSequence phrase;
    //public PhraseSegmentData phrase;

    Transform baseTarget;
    PointerType pointerType;
    bool flipped = true;
    int uiLayer;
    Vector2 offset;
    bool offscreen = false;
    bool isRectTransform = false;

    public RectTransform rectTransform { get; private set; }
    public Vector2 RootPosition { get; set; }
    public Vector2 TargetVerticalOffset { get; set; }
    public Vector2 HorizontalOffset { get; set; }

    public Transform Speaker { get; set; }

    public bool Flipped {
        get {
            return flipped;
        }
        set {
            if (flipped != value) {
                flipped = value;
                UpdateBackground();
            }
        }
    }

    public PointerType PointerType {
        get {
            return pointerType;
        }
        set {
            if (pointerType != value) {
                pointerType = value;
                UpdateBackground();
            }
        }
    }

    public Vector2 FlipOffset {
        get {
            if (Flipped) {
                return Vector2.zero;
            } else {
                return -rectTransform.rect.width * Vector2.right;
            }
        }
    }

    public Rect DoubleRect {
        get {
            var r = rectTransform.rect;
            r.width *= 2f;
            r.center = RootPosition;
            return r;
        }
    }

    public GameObject BookmarkButtonInstance { get; set; }

    public void Initialize(Transform target, PhraseSequence phrase, PointerType pointerType) {
        Speaker = target;

        this.target = target.GetSpeechBubbleTarget();

        if (Speaker.CompareTag(TagLibrary.Player)) {
            var ptarg = GameObject.FindGameObjectWithTag(TagLibrary.PlayerSpeechBubbleTarget);
            if (ptarg) {
                this.target = ptarg.transform;
                isRectTransform = true;
            }
        }

        this.baseTarget = target;
        this.phrase = phrase;
        this.PointerType = pointerType;
    }

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start() {
        LastSpeechBubble = this;
        uiLayer = LayerMask.NameToLayer("UI");

        if (!phrase.IsWord && PlayerDataConnector.CanLearn(phrase, false)) {
            var btn = Instantiate(bookmarkButton) as GameObject;
            btn.GetInterface<IInitializable<PhraseSequence>>().Initialize(phrase);
            btn.transform.SetParent(wordsPanel.transform);
        }

        foreach (var word in phrase.PhraseElements) {
            var go = Instantiate(wordButtonPrefab) as GameObject;
            go.transform.SetParent(wordsPanel.transform, false);
            go.GetComponent<SpeechBubbleWordUI>().Initialize(word);
        }

        CrystallizeEventManager.Environment.AfterCameraMove += AfterCameraMove;


        if (translationText.text == "") {
            translationText.gameObject.SetActive(false);
        }

        UpdateBackground();
    }

    void HandleEditButtonClicked(object sender, System.EventArgs e) {
        CrystallizeEventManager.UI.RaiseUIRequest(this, new DragDropFreeInputUIRequestEventArgs(gameObject, phrase));
        CrystallizeEventManager.UI.RaiseSpeechBubbleRequested(this, new SpeechBubbleRequestedEventArgs(baseTarget));
    }

    void OnDestroy() {
        CrystallizeEventManager.Environment.AfterCameraMove -= AfterCameraMove;
    }

    void AfterCameraMove(object sender, System.EventArgs e) {
        offset = Vector2.MoveTowards(offset, TargetVerticalOffset + HorizontalOffset, 100f * Time.deltaTime);

        if (target) {
            if (isRectTransform) {
                rectTransform.position = target.position;
            } else {
                bool nowOffscreen = false;
                var t = Camera.main.WorldToScreenPoint(target.position);
                var z = t.z;
                var rectBounds = rectTransform.rect;
                var screenRect = new Rect(0, MinY, Screen.width - rectBounds.width, Screen.height - MinY - rectBounds.height);
                if (!screenRect.Contains((Vector2)t) || t.z < MinZ) {
                    var direction = Camera.main.transform.InverseTransformPoint(target.position);
                    var origin = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                    var hit = screenRect.Raycast(origin, direction);

                    t = hit + origin;
                    t.z = z;

                    if (z < 0) {
                        t.y = MinY;
                    } else if (z < MinZ) {
                        t.y = Mathf.Lerp(t.y, MinY, 1f - (z / MinZ));
                    }
                    nowOffscreen = true;
                }

                RootPosition = t;
                rectTransform.position = RootPosition + offset + FlipOffset;

                if (nowOffscreen != offscreen) {
                    offscreen = nowOffscreen;
                    UpdateBackground();
                }
            }
        }
    }

    public void SetTranslation(string text) {
        translationText.text = text;
    }

    void UpdateBackground() {
        if (offscreen) {
            GetComponent<Image>().sprite = GUIPallet.Instance.offscreenSpeechBubble;
        } else {
            switch (pointerType) {
                case PointerType.Normal:
                    if (Flipped) {
                        GetComponent<Image>().sprite = GUIPallet.Instance.leftSpeechBubble;
                    } else {
                        GetComponent<Image>().sprite = GUIPallet.Instance.rightSpeechBubble;
                    };
                    break;
                case PointerType.Phone:
                    if (Flipped) {
                        GetComponent<Image>().sprite = GUIPallet.Instance.leftPhoneSpeechBubble;
                    } else {
                        GetComponent<Image>().sprite = GUIPallet.Instance.rightPhoneSpeechBubble;
                    }
                    break;
            }
        }
    }

    // TODO: ew....
    public RectTransform GetWord(int wordID) {
        foreach (var w in GetComponentsInChildren<Transform>()) {
            if (w.GetComponent<SpeechBubbleWordUI>()) {
                if (w.GetComponent<SpeechBubbleWordUI>().Word.WordID == wordID) {
                    return w.GetComponent<RectTransform>();
                }
            }
        }
        return null;
    }

    public RectTransform GetWord(PhraseSequenceElement word) {
        foreach (var w in GetComponentsInChildren<Transform>()) {
            if (w.GetComponent<SpeechBubbleWordUI>()) {
                if (PhraseSequenceElement.IsEqual(w.GetComponent<SpeechBubbleWordUI>().Word, word)) {
                    return w.GetComponent<RectTransform>();
                }
            }
        }
        return null;
    }


    public void OnPointerClick(PointerEventData eventData) {
        CrystallizeEventManager.Input.RaiseEnvironmentClick(this, EventArgs.Empty);
    }
}
