using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

[ResourcePath("UI/ReviewSummary")]
public class ReviewSummaryUI : MonoBehaviour, ITemporaryUI<PhraseReviewSessionResultArgs, PhraseReviewSessionResultArgs>, IDebugMethods {

    public GameObject wordPrefab;
    public GameObject phrasePrefab;
    public GameObject experienceEffectPrefab;
    public RectTransform reviewParent;

    public event EventHandler<EventArgs<PhraseReviewSessionResultArgs>> Complete;

    List<GameObject> instances = new List<GameObject>();
    Queue<SessionReviewEntry<PhraseSequence>> reviewQueue = new Queue<SessionReviewEntry<PhraseSequence>>();
    Dictionary<ItemReviewPlayerData<PhraseSequence>, GameObject> itemTargets = new Dictionary<ItemReviewPlayerData<PhraseSequence>, GameObject>();

    public virtual void Initialize(PhraseReviewSessionResultArgs param1) {
        PlayerData.Instance.Reviews.GetNewReviews();
//        var reviews = PlayerData.Instance.Reviews.Reviews;

        var phrases = PlayerData.Instance.Reviews.Reviews;
        UIUtil.GenerateChildren(phrases, reviewParent, CreateChild);

        if (param1 != null) {
            reviewQueue = new Queue<SessionReviewEntry<PhraseSequence>>(param1.SessionReviews);
            StartCoroutine(ExperienceCoroutine());
        }
    }

    public void Close() {
        Complete.Raise(this, new EventArgs<PhraseReviewSessionResultArgs>(null));
        Destroy(gameObject);
    }

    GameObject CreateChild(PhraseItemReviewPlayerData review) {
        GameObject instance = null;
        if (review.Item.IsWord) {
            instance = Instantiate<GameObject>(wordPrefab);
            instance.GetInterface<IInitializable<Color>>().Initialize(GUIPallet.Instance.GetColorForWordCategory(review.Item.Word.GetPhraseCategory()));
        } else {
            instance = Instantiate<GameObject>(phrasePrefab);
        }
        instance.GetInterface<IInitializable<int>>().Initialize(review.GetLevel());
        instance.GetComponentInChildren<Text>().text = PlayerDataConnector.GetText(review.Item);

        instances.Add(instance);
        itemTargets[review] = instance;

        return instance;
    }

    void Start() {
        CrystallizeEventManager.Input.OnLeftClick += Input_OnLeftClick;
    }

    void OnDestroy() {
        CrystallizeEventManager.Input.OnLeftClick -= Input_OnLeftClick;
    }

    protected virtual IEnumerator ExperienceCoroutine() {
        yield return new WaitForSeconds(0.1f);

        float time = 0.5f;
        if (reviewQueue.Count > 0) {
            time = Mathf.Clamp(1f / reviewQueue.Count, 0.05f, 0.5f);
        }

        foreach (var revEntry in reviewQueue) {
            var targ = itemTargets[revEntry.Review];
            CreateEffect(targ, revEntry.AfterRank - revEntry.BeforeRank);

            if (revEntry.LevelChanged) {
                targ.GetInterface<IInitializable<int>>().Initialize(revEntry.AfterLevel);
            }

            yield return new WaitForSeconds(time);
        }
    }

    void Input_OnLeftClick(object sender, EventArgs e) {
        Close();
    }

    void CreateEffect(GameObject target, int amount) {
        var instance = MainCanvas.main.AddNew(experienceEffectPrefab);
        instance.GetInterface<IInitializable<int>>().Initialize(amount);
        instance.transform.position = target.GetComponent<RectTransform>().GetCenter();
    }

    #region DEBUG
    public IEnumerable<NamedMethod> GetMethods() {
        return new Func<string, string>[] { 
            GenerateRandomReviews
        }.Select((m) => new NamedMethod(m));
    }

    string GenerateRandomReviews(string s) {
        var reviews = PlayerData.Instance.Reviews.Reviews.PickN(10);
        var results = reviews.Select((r) => new SessionReviewEntry<PhraseSequence>(r)).ToList();
        foreach (var r in results) {
            r.BeforeRank = UnityEngine.Random.Range(0, 8);
            r.AfterRank = UnityEngine.Random.Range(0, 8);
            Debug.Log(r.BeforeRank + "; " + r.AfterRank);
        }

        reviewQueue = new Queue<SessionReviewEntry<PhraseSequence>>(results);

        StartCoroutine(ExperienceCoroutine());

        return "moving items";
    }
    #endregion

}
