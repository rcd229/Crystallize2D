using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CollectInitArgs {
    public static readonly CollectInitArgs Default = new CollectInitArgs();

    public List<PhraseSequence> Phrases { get; set; }
    public bool IsFilled { get; set; }
    public CanvasLayer Layer { get; set; }

    public CollectInitArgs() {
        Phrases = new List<PhraseSequence>();
        IsFilled = false;
        Layer = CanvasLayer.None;
    }

    public CollectInitArgs(List<PhraseSequence> phrases, bool isFilled)
        : this() {
        this.Phrases = phrases;
        this.IsFilled = isFilled;
    }

}

public class CollectUI : UIMonoBehaviour, ITemporaryUI<CollectInitArgs, object>, IDebugMethods {

    const string ResourcePath = "UI/Collect";
    public static CollectUI GetInstance() {
        return GetInstance(CollectInitArgs.Default);
    }

    public static CollectUI GetInstance(CollectInitArgs args) {
        var sceneInstance = GameObject.FindGameObjectWithTag(TagLibrary.CollectUI);
        if (sceneInstance) {
            return sceneInstance.GetComponent<CollectUI>();
        } else {
            var i = GameObjectUtil.GetResourceInstance<CollectUI>(ResourcePath);
            i.Initialize(args);
            MainCanvas.main.Add(i.transform);
            //MainCanvas.main.Add(i.transform, args.Layer);
            return i;
        }
    }

    static List<PhraseSequence> GetUnknownPhrases(List<PhraseSequence> phrases) {
        var list = new List<PhraseSequence>();
        foreach (var item in list) {
            if (!PlayerDataConnector.ContainsLearnedItem(item)) {
                list.Add(item);
            }
        }
        return list;
    }

    public GameObject wordPrefab;
    public GameObject phrasePrefab;
    public GameObject targetWordPrefab;
    public GameObject targetPhrasePrefab;
    public GameObject animatedWordPrefab;
    public GameObject animatedPhrasePrefab;
    public GameObject animatedWordSlotPrefab;
    public GameObject emptyWordPrefab;
    public GameObject emptyPhrasePrefab;
    public Scrollbar scrollBar;
    public RectTransform mask;
    public RectTransform collectionParent;
    public RectTransform fillerTransform;
    public RectTransform container;

    List<PhraseSequence> targetPhrases = new List<PhraseSequence>();
    List<GameObject> instances = new List<GameObject>();
    List<GameObject> emptyInstances = new List<GameObject>();
    List<GameObject> addedInstances = new List<GameObject>();
    float maxWidth = 0;
    float width = 0;
    bool isFilled = false;
    bool started = false;

    float lastWidth = 0;

    List<NamedMethod> debugMethods = new List<NamedMethod>();

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(CollectInitArgs args) {// List<PhraseSequence> targetPhrases) {
        this.targetPhrases = args.Phrases;// targetPhrases;
        this.isFilled = args.IsFilled;
    }

    public void Close() {
        Destroy(gameObject);
    }

    void Start() {
        if (isFilled) {
            //countPanel.gameObject.SetActive(false);
            foreach (var p in targetPhrases) {
                AddItem(p, false);
            }
            RefreshScroll();
        } else {
            if (targetPhrases.Count == 0) {
                var pdh = GetComponentInChildren<CollectBackgroundUI>();
                if (pdh) {
                    pdh.OnPhraseDropped += HandleBackgroundPhraseDropped;
                }

                //CrystallizeEventManager.UI.BeforeWordClicked += UI_OnWordClicked;
                //CrystallizeEventManager.UI.BeforePhraseClicked += UI_OnPhraseClicked;
            } else {
                //countPanel.gameObject.SetActive(false);
            }

            Refresh();
        }

        started = true;
    }

    void OnEnable() {
        if (started) {
            Refresh();
        }

        CrystallizeEventManager.PlayerState.OnWordCollected += PlayerState_OnWordCollected;
        CrystallizeEventManager.PlayerState.OnPhraseCollected += PlayerState_OnPhraseCollected;
        CrystallizeEventManager.PlayerState.AvailableReviewsChanged += PlayerState_AvailableReviewsChanged;

        CrystallizeEventManager.UI.BeforeWordClicked += UI_OnWordClicked;
        CrystallizeEventManager.UI.BeforePhraseClicked += UI_OnPhraseClicked;
        CrystallizeEventManager.UI.OnWordDragged += UI_OnWordDragged;
        CrystallizeEventManager.UI.OnPhraseDragged += UI_OnPhraseDragged;

        debugMethods = new List<NamedMethod>(GetMethods());
        DebugMenuListener.AddContextMethods(debugMethods);

        CoroutineManager.Instance.WaitAndDo(Refresh, new WaitForSeconds(0.05f));
    }

    void OnDisable() {
        if (CrystallizeEventManager.Alive) {
            CrystallizeEventManager.PlayerState.OnWordCollected -= PlayerState_OnWordCollected;
            CrystallizeEventManager.PlayerState.OnPhraseCollected -= PlayerState_OnPhraseCollected;
            CrystallizeEventManager.PlayerState.AvailableReviewsChanged -= PlayerState_AvailableReviewsChanged;

            CrystallizeEventManager.UI.BeforeWordClicked -= UI_OnWordClicked;
            CrystallizeEventManager.UI.BeforePhraseClicked -= UI_OnPhraseClicked;
            CrystallizeEventManager.UI.OnWordDragged -= UI_OnWordDragged;
            CrystallizeEventManager.UI.OnPhraseDragged -= UI_OnPhraseDragged;
        }

        DebugMenuListener.RemoveContextMethods(debugMethods);
    }

    IEnumerator SlowUpdate() {
        while (true) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 2f));
            Refresh();
        }
    }

    void PlayerState_AvailableReviewsChanged(object sender, ReviewStateArgs e) {
        Refresh();
    }

    void Update() {
        var overflow = width - maxWidth;
        if (overflow > 0 && rectTransform.GetScreenRect().Contains(Input.mousePosition)) {
            var scrollForce = Input.GetAxis("Mouse ScrollWheel");
            scrollBar.value -= scrollForce * 1000f / overflow;
        }

        if (lastWidth != rectTransform.rect.width) {
            lastWidth = rectTransform.rect.width;
            Refresh();
        }
    }

    GameObject CreateChild(PhraseSequence phrase) {
        GameObject prefab = null;
        if (phrase.IsWord) {
            if (PlayerDataConnector.ContainsLearnedItem(phrase)) {
                prefab = wordPrefab;
            } else {
                prefab = targetWordPrefab;
            }
        } else {
            if (PlayerDataConnector.ContainsLearnedItem(phrase)) {
                prefab = phrasePrefab;
            } else {
                prefab = targetPhrasePrefab;
            }
        }

        var instance = Instantiate<GameObject>(prefab);
        instance.GetComponent<IInitializable<PhraseSequence>>().Initialize(phrase);
        instances.Add(instance);
        return instance;
    }

    void HandleBackgroundPhraseDropped(object sender, PhraseEventArgs args) {
        if (PlayerDataConnector.CanLearn(args.Phrase)) {
            AddItem(args.Phrase);
        }
    }

    void UI_OnPhraseDragged(object sender, PhraseClickedEventArgs e) {
        var item = animatedPhrasePrefab;
        CreateAnimatedItem(item, new PhraseSequence(e.Phrase), true);
    }

    void UI_OnPhraseClicked(object sender, PhraseClickedEventArgs e) {
       if (PlayerDataConnector.TryCollectItem(e.Phrase)) {
            var item = animatedPhrasePrefab;
            CreateAnimatedItem(item, new PhraseSequence(e.Phrase), false);
        }
    }

    void UI_OnWordDragged(object sender, PhraseClickedEventArgs e) {
        var item = animatedWordPrefab;
        CreateAnimatedItem(item, new PhraseSequence(e.Phrase), true);
    }

    void UI_OnWordClicked(object sender, PhraseClickedEventArgs e) {
        if (PlayerDataConnector.TryCollectItem(e.Phrase)) {
            var item = animatedWordPrefab;
            CreateAnimatedItem(item, new PhraseSequence(e.Phrase), false);
        }
    }

    void PlayerState_OnWordCollected(object sender, PhraseEventArgs e) {
        Refresh();
    }

    void PlayerState_OnPhraseCollected(object sender, PhraseEventArgs e) {
        Refresh();
    }

    void Refresh() {
        //if (targetPhrases.Count == 0) {
            //RefreshFree();
            //CoroutineManager.Instance.WaitAndDo(RefreshFree);
        //} else {
            RefreshPhrases();
        //}

        CoroutineManager.Instance.WaitAndDo(RefreshScroll);
    }

    void RefreshPhrases() {
        UIUtil.GenerateChildren(targetPhrases, instances, collectionParent, CreateChild);

        foreach (var i in emptyInstances) {
            Destroy(i);
        }
        emptyInstances.Clear();

        var extraLearned = from tp in PlayerData.Instance.Session.TodaysCollectedWords
                           where !targetPhrases.ContainsEquivalentPhrase(tp)
                           select tp;

        foreach (var tp in extraLearned) {
            //Debug.Log("trying to add: " + tp.GetText());
            AddItem(tp, false);
        }

        var unfoundTargetWords = from tp in targetPhrases
                                 where !PlayerDataConnector.ContainsLearnedItem(tp)
                                 select tp;

        var remainingWords = PlayerData.Instance.Proficiency.Words - unfoundTargetWords.Count() - PlayerDataConnector.CollectedWordCount;
        while (remainingWords > 0) {
            //Debug.Log("trying to add empty word");
            AddEmpty(emptyWordPrefab);
            remainingWords--;
        }

        var remainingPhrases = PlayerData.Instance.Proficiency.Phrases - PlayerDataConnector.CollectedPhraseCount;
        while (remainingPhrases > 0) {
            //Debug.Log("trying to add empty phrase");
            AddEmpty(emptyPhrasePrefab);
            remainingPhrases--;
        }

        //RefreshScroll();
    }

    void RefreshScroll() {
        //Debug.Log("refreshingscroll");
        maxWidth = Screen.width - transform.position.x - 24f;
        if (container) {
            maxWidth = container.rect.width - 24f;
        }

        width = collectionParent.rect.width + 8f;
        if (width > maxWidth) {
            scrollBar.gameObject.SetActive(true);
            scrollBar.size = maxWidth / collectionParent.rect.width;
            mask.GetComponent<LayoutElement>().preferredWidth = maxWidth;
        } else {
            scrollBar.gameObject.SetActive(false);
            mask.GetComponent<LayoutElement>().preferredWidth = width;
        }

        mask.GetComponent<LayoutElement>().preferredHeight = collectionParent.rect.height + 4f;
        UpdateScrollLocation();
    }

    //void RefreshFree() {
    //    foreach (var i in emptyInstances) {
    //        Destroy(i);
    //    }
    //    emptyInstances.Clear();

    //    var remainingWords = PlayerData.Instance.Proficiency.Words - ResourceLearnEventHandler.GetWords();
    //    while (remainingWords > 0) {
    //        AddEmpty(emptyWordPrefab);
    //        remainingWords--;
    //    }

    //    var remainingPhrases = PlayerData.Instance.Proficiency.Phrases - ResourceLearnEventHandler.GetPhrases();
    //    while (remainingPhrases > 0) {
    //        AddEmpty(emptyPhrasePrefab);
    //        remainingPhrases--;
    //    }
    //}

    void CreateAnimatedItem(GameObject animatedItemPrefab, PhraseSequence phrase, bool dragging) {
        var instance = Instantiate<GameObject>(animatedItemPrefab);
        var args = new AnimatedCollectedItemArgs(phrase, GetTarget(phrase), dragging);
        instance.GetComponent<AnimatedCollectedItemUI>().Initialize(args);
        instance.GetComponent<AnimatedCollectedItemUI>().Complete += AnimatedItem_Complete;
    }

    public void CreateAnimatedSlotItem(RectTransform target) {
        var instance = Instantiate<GameObject>(animatedWordSlotPrefab);
        var args = new AnimatedCollectedItemArgs(null, GetTarget(null), false);
        args.SetPos = false;
        instance.GetComponent<AnimatedCollectedItemUI>().Initialize(args);
        instance.transform.position = target.GetCenter();
        instance.GetComponent<AnimatedCollectedItemUI>().Complete += (s, e) => {
            AddEmpty(wordPrefab);
            CoroutineManager.Instance.WaitAndDo(RefreshScroll);
        };
    }

    public AnimatedCollectedItemUI CreateAnimatedItemAndRemove(RectTransform target) {
        if (addedInstances.Count == 0) {
            return null;
        }

        var removed = addedInstances[0];
        addedInstances.RemoveAt(0);
        //Debug.Log(removed);
        PhraseSequence removedPhrase = removed.GetComponent<DataContainer>().Retrieve<PhraseSequence>();
        Destroy(removed);
        RefreshScroll();

        GameObject animatedItemPrefab = null;
        if (removedPhrase.IsWord) {
            animatedItemPrefab = animatedWordPrefab;
        } else {
            animatedItemPrefab = animatedPhrasePrefab;
        }
        var instance = Instantiate<GameObject>(animatedItemPrefab);
        var args = new AnimatedCollectedItemArgs(removedPhrase, target.GetCenter(), false);
        args.SetPos = false;
        instance.GetComponent<AnimatedCollectedItemUI>().Initialize(args);
        instance.transform.position = Vector3.zero;
        return instance.GetComponent<AnimatedCollectedItemUI>();
    }

    void AnimatedItem_Complete(object sender, EventArgs<PhraseSequence> e) {
        if (targetPhrases.Count > 0) {
            Refresh();
        } else {
            AddItem(e.Data);
        }
    }

    void AddItem(PhraseSequence item, bool refresh = true) {
        if (item.IsWord) {
            AddItem(item, wordPrefab, false);
        } else {
            AddItem(item, phrasePrefab, false);
        }

        if (!isFilled && refresh) {
            Refresh();
        }
    }

    void AddItem(PhraseSequence item, GameObject prefab, bool refresh) {
        var instance = Instantiate<GameObject>(prefab);
        instance.GetInterface<IInitializable<PhraseSequence>>().Initialize(item);
        instance.transform.SetParent(collectionParent, false);
        instance.AddComponent<DataContainer>().Store<PhraseSequence>(item);
        instances.Add(instance);
        addedInstances.Add(instance);

        collectionParent.transform.localPosition = new Vector2(2f, 2f);

        if (!isFilled && refresh) {
            Refresh();
        }
    }

    Vector2 GetTarget(PhraseSequence phrase) {
        return collectionParent.GetCenter();
    }

    void AddEmpty(GameObject prefab) {
        var instance = Instantiate<GameObject>(prefab);
        instance.transform.SetParent(collectionParent.transform, false);
        emptyInstances.Add(instance);
    }

    public void UpdateScrollLocation() {
        var offset = (maxWidth - width) * scrollBar.value;
        collectionParent.transform.localPosition = new Vector2(offset + 2f, 2f);
    }

    #region DEBUG
    public IEnumerable<NamedMethod> GetMethods() {
        return NamedMethod.Collection(AddEmptyWord, AddEmptyPhrase);
    }

    public string AddEmptyWord(string s) {
        PlayerData.Instance.Proficiency.Words++;
        Refresh();
        return "adding word to inv";
    }

    public string AddEmptyPhrase(string s) {
        PlayerData.Instance.Proficiency.Phrases++;
        Refresh();
        return "adding phrase to inv";
    }

    #endregion
}