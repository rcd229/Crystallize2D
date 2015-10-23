using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

[ResourcePath("UI/ColumnReviewSummary")]
public class ColumnReviewSummaryUI : MonoBehaviour, ITemporaryUI<object, object>, IDebugMethods {

    public GameObject wordPrefab;
    public GameObject phrasePrefab;
    public GameObject experienceEffectPrefab;
    public RectTransform weakParent;
    public RectTransform moderateParent;
    public RectTransform strongParent;

    public event EventHandler<EventArgs<object>> Complete;

    List<GameObject> instances = new List<GameObject>();
    Queue<SessionReviewEntry<PhraseSequence>> reviewQueue = new Queue<SessionReviewEntry<PhraseSequence>>();
    Dictionary<ItemReviewPlayerData<PhraseSequence>, GameObject> itemTargets = new Dictionary<ItemReviewPlayerData<PhraseSequence>, GameObject>();

    public void Initialize(object param1) {
        PlayerData.Instance.Reviews.GetNewReviews();

        var weak = PlayerData.Instance.Reviews.GetWeakReviews();
        UIUtil.GenerateChildren(weak, weakParent, CreateChild);

        var moderate = PlayerData.Instance.Reviews.GetModerateReviews();
        UIUtil.GenerateChildren(moderate, moderateParent, CreateChild);

        var strong = PlayerData.Instance.Reviews.GetStrongReviews();
        UIUtil.GenerateChildren(strong, strongParent, CreateChild);
    }

    public void Close() {
        Complete.Raise(this, new EventArgs<object>(null));
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
        CrystallizeEventManager.Input.OnEnvironmentClick += Input_OnLeftClick;
    }

    void OnDestroy() {
        CrystallizeEventManager.Input.OnEnvironmentClick -= Input_OnLeftClick;
    }

    IEnumerator ExperienceCoroutine() {
        float time = 0.5f;
        if (reviewQueue.Count > 0) {
            time = Mathf.Clamp(1f / reviewQueue.Count, 0.05f, 0.5f);
        }

        var parents = new RectTransform[] { weakParent, moderateParent, strongParent };
        foreach (var revEntry in reviewQueue) {
            var targ = itemTargets[revEntry.Review];
            CreateEffect(targ, revEntry.AfterRank - revEntry.BeforeRank);

            if (revEntry.LevelChanged) {
                yield return new WaitForSeconds(0.25f);
                targ.GetInterface<IInitializable<int>>().Initialize(revEntry.AfterLevel);
                UIUtil.AnimateMoveItemFromList(targ.GetComponent<RectTransform>(), parents[revEntry.AfterLevel]);
            }

            yield return new WaitForSeconds(time);
        }
    }

    void Input_OnLeftClick(object sender, EventArgs e) {
        Close();
    }

    void CreateEffect(GameObject target, int amount) {
        var instance = Instantiate<GameObject>(experienceEffectPrefab);
        instance.GetInterface<IInitializable<int>>().Initialize(amount);
        MainCanvas.main.Add(instance.transform);
        instance.transform.position = target.GetComponent<RectTransform>().GetCenter();
        //Debug.Log(target.GetComponent<RectTransform>().GetCenter());
    }

    #region DEBUG
    public IEnumerable<NamedMethod> GetMethods() {
        return new Func<string, string>[] { 
            MoveRandomItems,
            GenerateRandomReviews
        }.Select((m) => new NamedMethod(m));
    }

    string MoveRandomItems(string s) {
        var parents = new RectTransform[] { weakParent, moderateParent, strongParent };
        var sub = instances.PickN(5);

        foreach(var i in sub){
            var newParentIndex = UnityEngine.Random.Range(0, parents.Length);
            i.GetInterface<IInitializable<int>>().Initialize(newParentIndex);
            UIUtil.AnimateMoveItemFromList(i.GetComponent<RectTransform>(), parents[newParentIndex]);
        }
        
        return "moved items";
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
