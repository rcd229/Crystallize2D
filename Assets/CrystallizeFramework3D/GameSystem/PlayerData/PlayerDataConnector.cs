using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JapaneseTools;

public class PlayerDataConnector {

    //public static string QuestCompleted { get; set; }
    //public static IQuestReward QuestReward { get; set; }

    public static int CollectedWordCount {
        get {
            int count = 0;
            foreach (var p in PlayerData.Instance.Session.TodaysCollectedWords) {
                if (p.IsWord) {
                    count++;
                }
            }
            return count;
        }
    }

    public static int RemainingWordCount {
        get {
            return PlayerData.Instance.Proficiency.Words - CollectedWordCount;
        }
    }

    public static int CollectedPhraseCount {
        get {
            int count = 0;
            foreach (var p in PlayerData.Instance.Session.TodaysCollectedWords) {
                if (!p.IsWord) {
                    count++;
                }
            }
            return count;
        }
    }

    public static int RemainingPhraseCount {
        get {
            return PlayerData.Instance.Proficiency.Phrases - CollectedPhraseCount;
        }
    }

    //public static void RevealJob(IJobRef job) {
    //    var i = PlayerData.Instance.Jobs.GetOrCreateItem(job.GameDataInstance.ID);
    //    i.Shown = true;
    //}

    //public static void UnlockJob(IJobRef job) {
    //    var i = PlayerData.Instance.Jobs.GetOrCreateItem(job.GameDataInstance.ID);
    //    i.Unlocked = true;
    //}

    //public static void PromoteJob(IJobRef job) {
    //    var i = PlayerData.Instance.Jobs.GetOrCreateItem(job.GameDataInstance.ID);
    //    i.Promoted = true;
    //}

    //public static void UpdateShownJobs() {
    //    foreach (var j in GameData.Instance.Jobs.Items) {
    //        var r = new IDJobRef(j.ID);
    //        //r.PlayerDataInstance.Unlocked = false;

    //        if (j.GetJobRequirements().IsFulfilled() && !j.Hide) {
    //            r.PlayerDataInstance.Shown = true;
    //        }

    //        //if (r.PlayerDataInstance.Shown && j.GetPhraseRequirements().IsFulfilled()) {
    //        //    new IDJobRef(j.ID).PlayerDataInstance.Unlocked = true;
    //        //}
    //    }
    //}

    //public static void UpdateShownJobs(IEnumerable<string> jobs) {
    //    foreach (var j in GameData.Instance.Jobs.Items) {
    //        var r = new IDJobRef(j.ID);
    //        if (jobs.Contains(r.GameDataInstance.Name)) {
    //            r.PlayerDataInstance.Shown = true;
    //        }

    //        if (r.PlayerDataInstance.Shown && j.GetPhraseRequirements().IsFulfilled()) {
    //            new IDJobRef(j.ID).PlayerDataInstance.Unlocked = true;
    //        }
    //    }
    //}

    //public static void AddRepetitionToJob(IJobRef job, JobTaskRef task) {
    //    //Debug.Log("Adding rep to " + task.Job.GameDataInstance.Name);
    //    job.PlayerDataInstance.AddTask(task);
    //}

    //public static void AddDayToJob(IJobRef job) {
    //    job.PlayerDataInstance.Days++;
    //}

    //public static void UnlockHome(HomeRef home) {
    //    PlayerData.Instance.Homes.GetOrCreateItem(home.ID).Unlocked = true;
    //    //.AddItem(new HomePlayerData(home.ID, true));
    //    CrystallizeEventManager.PlayerState.RaiseHomesChanged(null, null);
    //}

    //public static void AddMoney(int amount) {
    //    PlayerData.Instance.Money += amount;
    //    CrystallizeEventManager.PlayerState.RaiseMoneyChanged(null, null);
    //}

    public static bool ContainsLearnedItem(PhraseSequenceElement word) {
        return PlayerData.Instance.WordCollection.ContainsFoundWord(word);
    }

    public static bool ContainsLearnedItem(PhraseSequence phrase) {
        if (phrase.IsWord) {
            //Debug.Log("w: " + phrase.GetText() + ": " + PlayerData.Instance.WordStorage.ContainsFoundWord(phrase.Word));
            return PlayerData.Instance.WordCollection.ContainsFoundWord(phrase.Word);
        } else {
            //Debug.Log("p: " + phrase.GetText() + ": " + PlayerData.Instance.PhraseStorage.ContainsPhrase(phrase));
            return PlayerData.Instance.PhraseStorage.ContainsPhrase(phrase);
        }
    }

    public static bool ContainsUncollectedItem(PhraseSequence phrase) {
        foreach (var w in phrase.PhraseElements) {
            if (CanLearn(new PhraseSequence(w))) {
                return true;
            }
        }
        return false;
    }

    public static bool CanLearn(PhraseSequence phrase) {
        var s = "";
        return CanLearn(phrase, out s);
    }

    public static bool CanLearn(PhraseSequence phrase, out string message) {
        //Debug.Log("Knows phrase?" + phrase.GetText() + "; " + PlayerDataConnector.ContainsLearnedItem(phrase));
        if (PlayerDataConnector.ContainsLearnedItem(phrase)) {
            message = "You already have that word!";
            return false;
        }

        bool hasDictItem = false;
        foreach (var w in phrase.PhraseElements) {
            if (w.IsDictionaryWord) {
                hasDictItem = true;
                break;
            }
        }

        if (!hasDictItem) {
            message = "You can't learn that.";
            return false;
        }

        if (phrase.IsWord) {
            if (CollectedWordCount >= PlayerData.Instance.Proficiency.Words) {
                message = "You can't learn any more words today. Review your words to earn more slots.";
                return false;
            }
        } else {
            if (CollectedPhraseCount >= PlayerData.Instance.Proficiency.Phrases) {
                message = "You can't learn any more sentences today. Review your words to earn more slots.";
                return false;
            }
        }

        message = "You can learn that.";
        return true;
    }

    public static int ConfidenceCost(PhraseSequence phrase) {
        var amount = 0;
        foreach (var word in phrase.PhraseElements) {
            if (CanLearn(new PhraseSequence(word))) {
                amount++;
            }
        }
        return amount;
    }

    public static bool TryCollectItem(PhraseSequence item) {
        if (CanLearn(item)) {
            CollectItem(item);
            return true;
        } else {
            return false;
        }
    }

    public static void CollectPhrase(PhraseSequence phrase) {
        CollectItem(phrase);
    }

    public static void CollectWord(PhraseSequenceElement word) {
        CollectItem(new PhraseSequence(word));
    }

    public static void CollectItem(PhraseSequence item, bool addToCollected = true) {
        if (item == null) {
            return;
        }

        if (item.IsWord) {
            var word = item.Word;
            if (!word.IsDictionaryWord || PlayerData.Instance.WordCollection.ContainsFoundWord(word)) {
                //Debug.Log("Word cannot be collected: " + word.GetText());
                return;
            }
            PlayerData.Instance.WordCollection.AddFoundWord(word);
            //Debug.Log("collected word: " + item.GetText());
            AddConfidence(1);
        } else {
            if (PlayerData.Instance.PhraseStorage.ContainsPhrase(item)) {
                return;
            }
            PlayerData.Instance.PhraseStorage.AddPhrase(item);
            //Debug.Log("collected phrase: " + item.GetText());
            AddConfidence(1);
        }

        //TODO: put this someplace
        //SoundEffectManager.Play(SoundEffectType.Pop);

        if (addToCollected) {
            PlayerData.Instance.Session.TodaysCollectedWords.Add(item);
        }
        PlayerData.Instance.Reviews.AddReview(item);

        //CrystallizeEventManager.PlayerState.RaiseWordCollected(null, new PhraseEventArgs(item));
        //CrystallizeEventManager.PlayerState.RaiseAvailableReviewsChanged(null, new ReviewStateArgs());
    }

    /// <summary>
    /// Will return a minimum of 10 choices
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<PhraseSequence> GetWordChoices(PhraseSequence ignoreWord) {
        var toIgnore = new HashSet<int>(ignoreWord.PhraseElements.Select((e) => e.WordID));
        var choices = PlayerData.Instance.WordCollection.FoundWords
            .Where((w) => !toIgnore.Contains(w))
            .Select((w) => new PhraseSequence(new PhraseSequenceElement(w, 0)))
            .ToList();

        return choices;
    }

    public static IEnumerable<PhraseSequence> BufferWithExtraWords(IEnumerable<PhraseSequence> words, int minCount) {
        var choices = words.ToList();
        if (choices.Count < 10) {
            var extras = PhraseSetCollectionGameData.Default.Phrases;
            while (choices.Count < 10) {
                var eles = extras[UnityEngine.Random.Range(0, extras.Count)].PhraseElements;
                if (eles.Count > 0) {
                    var w = eles[UnityEngine.Random.Range(0, eles.Count)];
                    if (w.IsDictionaryWord) {
                        choices.Add(new PhraseSequence(w));
                    }
                }
            }
        }
        return choices;
    }

    public static int GetLearnedWordsCount() {
        return (from review in PlayerData.Instance.Reviews.Reviews
                where review.Rank >= 2
                select review).Count();
    }

    public static bool CanSelectPhrase(PhraseSequence phrase) {
        return GetNeededWords(phrase).Count == 0;
    }

    public static bool IsConstructed(PhraseSequence phrase) {
        if (phrase.IsWord) return false;
        if (phrase.ComparableElementCount == 0) return false;
        return true;
    }

    public static bool NeedToConstructPhrase(PhraseSequence phrase) {
        if (phrase.IsWord) {
            return false;
        }

        if (PlayerData.Instance.Reviews.ContainsReview(phrase)) {
            return PlayerData.Instance.Reviews.GetReview(phrase).Rank < 4;
        }
        return true;
    }

    public static List<PhraseSequence> GetNeededWords(PhraseSequence phrase) {
        var needed = new List<PhraseSequence>();
        foreach (var word in phrase.PhraseElements) {
            if (!word.IsDictionaryWord) {
                continue;
            }

            if (!ContainsLearnedItem(word)) {
                needed.Add(new PhraseSequence(word));
            }
        }
        return needed;
    }

    public static int ExtraWordCount() {
        return (PlayerData.Instance.Proficiency.GetReviewLevel() - 1) * 2;
    }

    static HashSet<string> knownKana = new HashSet<string>();
    public static string GetText(PhraseSequence basePhrase, bool usePlayerContext = false) {
        if (basePhrase == null || basePhrase.PhraseElements == null) {
            return "";
        }

        knownKana = PlayerData.Instance.KanaReviews.GetKnownKana();
        var phrase = basePhrase;
        if (usePlayerContext) {
            phrase = basePhrase.InsertContext(PlayerData.Instance.PersonalData.Context);
        }

        if (PlayerData.Instance.ScriptType == JapaneseTools.JapaneseScriptType.Romaji) {
            var outText = "";
            foreach (var word in phrase.PhraseElements) {
                var text = KanaConverter.Instance.ConvertToHiragana(word.GetText(JapaneseScriptType.Kana));
                if (AllKanaKnown(text)) {
                    outText += text;
                } else {
                    outText += KanaConverter.Instance.ConvertToRomaji(text) + " ";
                }
            }
            return outText;
        } else {
            return phrase.GetText(PlayerData.Instance.ScriptType);
        }
    }

    public static string GetText(PhraseSequenceElement word) {
        if (PlayerData.Instance.ScriptType == JapaneseTools.JapaneseScriptType.Romaji) {
            knownKana = PlayerData.Instance.KanaReviews.GetKnownKana();

            var text = KanaConverter.Instance.ConvertToHiragana(word.GetText(JapaneseScriptType.Kana));
            if (AllKanaKnown(text)) {
                return text;
            } else {
                return KanaConverter.Instance.ConvertToRomaji(text);
            }
        } else {
            return word.GetText(PlayerData.Instance.ScriptType);
        }
    }

    public static string GetTranslation(PhraseSequence phrase, bool usePlayerContext = true) {
        if (usePlayerContext) {
            return phrase.InsertContext(PlayerData.Instance.PersonalData.Context).Translation;
        } else {
            return phrase.Translation;
        }
    }

    static bool AllKanaKnown(string hiragana) {
        for (int i = 0; i < hiragana.Length; i++) {
            if (i + 1 < hiragana.Length) {
                if (knownKana.Contains(hiragana.Substring(i, 2))) {
                    i++;
                    continue;
                }
            }

            if (!knownKana.Contains(hiragana.Substring(i, 1))) {
                return false;
            }
        }
        return true;
    }

    //public static float GetKanaMultiplier() {
    //    return 2f * ((float)PlayerData.Instance.KanaReviews.GetKnownKana().Count / KanaTableUI.HiraganaCount()) + 1f;
    //}

    //public static float GetPromotionMultiplier() {
    //    if (PlayerData.Instance.Session.isPromotion) {
    //        return 2f;
    //    } else {
    //        return 1f;
    //    }
    //}

    //public static int GetVersatility() {
    //    int versatility = 0;
    //    foreach (var i in BuyableClothes.GetValues()) {
    //        if (PlayerData.Instance.Session.ChestItem == i.Name
    //            || PlayerData.Instance.Session.LegsItem == i.Name) {
    //            versatility += i.Versatility;
    //        }
    //    }
    //    return versatility;
    //}

    //public static float GetVersatilityMultiplier() {
    //    return (float)GetVersatility() * 0.1f + 1f;
    //}

    //public static int GetFormality() {
    //    int formality = 0;
    //    foreach (var i in BuyableClothes.GetValues()) {
    //        if (PlayerData.Instance.Session.ChestItem == i.Name
    //            || PlayerData.Instance.Session.LegsItem == i.Name) {
    //            formality += i.Formality;
    //        }
    //    }
    //    return formality;
    //}

    //public static float GetFormalityMultiplier() {
    //    return (float)GetFormality() * 0.1f + 1f;
    //}

    //public static int GetComfort() {
    //    int comfort = 0;
    //    foreach (var i in BuyableFurniture.GetValues()) {
    //        if (i.Availability == BuyableAvailability.Purchased) {
    //            //Debug.Log("Adding comfort for " + i.Name);
    //            comfort += i.Comfort;
    //        }
    //    }
    //    return comfort;
    //}

    //public static float GetComfortMultiplier() {
    //    return (float)GetComfort() * 0.1f + 1f;
    //}

    public static void AddReview<T>(ItemReviewPlayerData<T> review, int amount) {
        AddConfidence(amount);
        if (typeof(T) == typeof(PhraseSequence)) {
            var p = review.Item as PhraseSequence;
            if (p != null && PlayerData.Instance.Session.TodaysCollectedWords.ContainsEquivalentPhrase(p, out p)) {
                PlayerData.Instance.Session.TodaysCollectedWords.Remove(p);
            }
        }

        var maxConf = PlayerData.Instance.Proficiency.Confidence;
        PlayerData.Instance.Proficiency.RecalculateExperience();
        var newMaxConf = PlayerData.Instance.Proficiency.Confidence;
        AddConfidence(newMaxConf - maxConf);

        //CrystallizeEventManager.PlayerState.RaiseAvailableReviewsChanged(null, new ReviewStateArgs());
    }

    public static void SetConfidence(int amount) {
        var change = amount - PlayerData.Instance.Session.Confidence;
        AddConfidence(change);
    }

    public static void AddConfidence(int amount) {
        if (amount != 0) {
            var max = PlayerData.Instance.Proficiency.Confidence;
            if (PlayerData.Instance.Session.Confidence + amount > max) {
                var overflow = PlayerData.Instance.Session.Confidence + amount - max;
                PlayerData.Instance.Session.Confidence = Mathf.Max(max, 0);
                PlayerData.Instance.Proficiency.ReserveConfidence += overflow;
            } else {
                PlayerData.Instance.Session.Confidence = Mathf.Max(0, PlayerData.Instance.Session.Confidence + amount);
            }
            //CrystallizeEventManager.PlayerState.RaiseConfidenceChanged(null, new EventArgs<int>(amount));
        }
    }

    //public static List<bool> GetMissingWords(PhraseSequence phrase) {
    //    return PlayerData.Instance.Tutorial.GetMissingWords(phrase);
    //}

    //public static void SetQuestProgress(QuestTypeID questID, string progress) {
    //    QuestProgress prog = new QuestProgress();
    //    prog.TypeID = questID;
    //    prog.State = progress;
    //    PlayerData.Instance.QuestData.Set(prog);
    //}

    //public static void SetQuestFinished(QuestTypeID questID) {
    //    PlayerData.Instance.QuestData.GetOrCreateItem(questID).FinishedTimes++;
    //}

    //public static void SetQuestViewed(QuestTypeID questID) {
    //    PlayerData.Instance.QuestData.GetOrCreateItem(questID).Viewed = true;
    //}

    //public static void SetFlags(params Guid[] flags) {
    //    foreach (var flag in flags) {
    //        PlayerData.Instance.QuestData.AddFlag(flag);
    //    }
    //    CrystallizeEventManager.PlayerState.RaiseQuestFlagChanged(null, null);
    //}

    //public static void UnsetFlags(params Guid[] flags) {
    //    foreach (var flag in flags) {
    //        PlayerData.Instance.QuestData.RemoveFlag(flag);
    //    }
    //    CrystallizeEventManager.PlayerState.RaiseQuestFlagChanged(null, null);
    //}

    //public static PhraseConstructorArgs GetConstructorArgsForPhrase(PhraseSequence phrase) {
    //    var rev = PlayerData.Instance.Reviews.GetReview(phrase);
    //    var revLevel = 0;
    //    if (rev != null) {
    //        revLevel = rev.Rank;
    //    }
    //    var words = GetConstructorWordsForPhrase(phrase, ExtraWordCount());
    //    foreach (var w in words) {
    //        if (w == null) {
    //            Debug.Log("null word");
    //        }
    //    }
    //    var pca = new PhraseConstructorArgs(phrase, words, 2 + revLevel / 2);
    //    pca.Context = PlayerData.Instance.PersonalData.Context;
    //    return pca;
    //}

    public static List<PhraseSequence> GetConstructorWordsForPhrase(PhraseSequence phrase, int extraChoiceCount = 0, bool usePlayerContext = true) {
        var totalCount = phrase.ComparableElementCount + extraChoiceCount;
        var allChoices = BufferWithExtraWords(GetWordChoices(phrase), totalCount);
        var comparable = from w in phrase.PhraseElements
                         where w.IsDictionaryWord
                         select new PhraseSequence(w);
        if (usePlayerContext) {
            comparable = comparable.Concat(phrase.GetContextWords(PlayerData.Instance.PersonalData.Context));
        }

        return CollectionExtensions.RandomSubsetWithValues(allChoices, comparable, totalCount);
    }

    public static bool GetTutorialViewed(string tutorial) {
        return PlayerData.Instance.Tutorial.GetTutorialViewed(tutorial);
    }

    public static void SetTutorialViewed(string tutorial) {
        PlayerData.Instance.Tutorial.SetTutorialViewed(tutorial);
    }

    //public static bool MapOpenStatus {
    //    get {
    //        return PlayerData.Instance.UIData.MapOpen;
    //    }
    //    set {
    //        PlayerData.Instance.UIData.MapOpen = value;
    //    }
    //}
    //public static bool ChatBoxOpenStatus {
    //    get {
    //        return PlayerData.Instance.UIData.ChatBoxOpen;
    //    }
    //    set {
    //        PlayerData.Instance.UIData.ChatBoxOpen = value;
    //    }
    //}
    //public static bool QuestStatusPanelOpenStatus {
    //    get {
    //        return PlayerData.Instance.UIData.QuestStatusOpen;
    //    }
    //    set {
    //        PlayerData.Instance.UIData.QuestStatusOpen = value;
    //    }
    //}

    //public static bool GetHUDPartEnabled(HUDPartType hudPart) {
    //    return PlayerData.Instance.UIData.GetPartEnabled(hudPart);
    //}

    //public static void SetHUDPartEnabled(HUDPartType hudPart, bool val) {
    //    var oldVal = GetHUDPartEnabled(hudPart);
    //    //if (val != oldVal) {
    //    PlayerData.Instance.UIData.SetPartEnabled(hudPart, val);
    //    SetHUDPartActive(hudPart, val);
    //    //Debug.Log(hudPart + "; " + val + "; " + GetHUDPartActive(hudPart));
    //    CrystallizeEventManager.UI.RaiseHUDPartStateChanged(null, new HUDPartArgs(hudPart, val));
    //    //}
    //}

    //public static bool GetHUDPartActive(HUDPartType hudPart) {
    //    return PlayerData.Instance.UIData.GetPartActive(hudPart);
    //}

    //public static void SetHUDPartActive(HUDPartType hudPart, bool val) {
    //    var oldVal = GetHUDPartActive(hudPart);
    //    if (val != oldVal) {
    //        PlayerData.Instance.UIData.SetPartActive(hudPart, val);
    //    }
    //}

}
