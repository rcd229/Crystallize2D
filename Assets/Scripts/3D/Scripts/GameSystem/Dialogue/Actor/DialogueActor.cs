using UnityEngine;
using System;
using System.Collections;

public class DialogueActor : MonoBehaviour, ISpeechTextSource {

    public string actorName = "???";
    [NonSerialized]
    public bool inGroup = false;
    public float distanceToPlayer = 2f;
    public bool canReduceConfidence = true;

    public PhraseSequence CurrentPhrase { get; set; }

    public event EventHandler<PhraseEventArgs> OnSpeechTextChanged;

    void Start() {
        //FloatingNameUI.main.SetName(this, name);
    }

    void OnEnable() {
        if (!transform.IsHumanControlled()) {
            ActorTracker.AddActor(gameObject);
        }
    }

    void OnDisable() {
        ActorTracker.RemoveActor(gameObject);
    }

    public void SetPhrase(PhraseSequence phrase, bool reduceConfidence = true, Action<GameObject> speechBubbleCallback = null) {
        CurrentPhrase = phrase;
        // TODO: remove
        if (OnSpeechTextChanged != null) {
            OnSpeechTextChanged(this, new PhraseEventArgs(phrase));
        }
        var speechArgs = new SpeechBubbleRequestedEventArgs(transform, phrase, reduceConfidence && canReduceConfidence, speechBubbleCallback);
        CrystallizeEventManager.UI.RaiseSpeechBubbleRequested(this, speechArgs);
        CrystallizeEventManager.PlayerState.RaiseGameEvent(this, speechArgs);

        if (phrase != null && phrase.ComparableElementCount > 0) {
            bool isMale = IsMale();

            var av = transform.Find("AvatarLoader");
            if (av && av.GetChild(0) && av.GetChild(0).name.Contains("Female")) {
                isMale = false;
            }

            //Debug.Log("phrasetext is: " + phrase.GetText(JapaneseTools.JapaneseScriptType.Kanji) + "; " + isMale);
            var audioText = "";
            if (phrase.IsWord) {
                audioText = phrase.GetText(JapaneseTools.JapaneseScriptType.Kana);
            } else {
                audioText = phrase.GetText(JapaneseTools.JapaneseScriptType.Kanji);
            }
            AudioLoader.PlayAudio(audioText, isMale);
        }
    }

    public void SetPhrase(PhraseSequence phrase, ContextData context) {
        SetPhrase(phrase.InsertContext(context));
    }

    public void SetLine(DialogueActorLine line, ContextData context = null, bool reduceConfidence = true, Action<GameObject> speechBubbleCallback = null) {
        if (line == null) {
            SetPhrase(null);
        } else {
            if (context != null) {
                SetPhrase(line.Phrase.InsertContext(context), reduceConfidence, speechBubbleCallback);
            } else {
                SetPhrase(line.Phrase, reduceConfidence, speechBubbleCallback);
            }
        }
    }

    public bool IsMale() {
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).name.Contains("Female")) {
                return false;
            }
        }
        return true;
    }

    //AudioClip GetAudioClip(DialogueActorLine line) {
    //    var ac = line.GetAudioClip();

    //    var overrideAudio = GetComponent<NPCDialogueOverrideAudio>();
    //    if (overrideAudio) {
    //        if (overrideAudio.GetAudioClip(line)) {
    //            ac = overrideAudio.GetAudioClip(line);
    //        }
    //    }

    //    return ac;
    //}

    void PlayAudio(AudioClip clip) {
        if (!clip) {
            return;
        }

        if (!GetComponent<AudioSource>()) {
            gameObject.AddComponent<AudioSource>();
        }
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().time = 0;
        GetComponent<AudioSource>().Play();
    }

    public void HandleTriggerEntered(object sender, TriggerEventArgs args) {
        if (args.Collider.IsPlayer()) {
            CrystallizeEventManager.Environment.RaiseActorApproached(this, EventArgs.Empty);
        }
    }

    public void HandleTriggerExited(object sender, TriggerEventArgs args) {
        if (args.Collider.IsPlayer()) {
            SetPhrase(null);
            CrystallizeEventManager.Environment.RaiseActorDeparted(this, EventArgs.Empty);
        }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public float GetRadius() {
        return 1f;
    }

    void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, 0.25f);
        Gizmos.DrawWireSphere(transform.position + transform.forward * distanceToPlayer, 0.25f);
    }
}
