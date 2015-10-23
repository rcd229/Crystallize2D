using UnityEngine;
using System.Collections;

public class EndConversationProcess : ConversationProcessPart, IProcess<ConversationArgs, object> {

    ConversationArgs args;

    public ProcessExitCallback OnExit { get; set; }

    public void Initialize(ConversationArgs args) {
        this.args = args;
        CoroutineManager.Instance.StartCoroutine(EndCoroutine());
    }

    IEnumerator EndCoroutine() {
        foreach (var t in args.Targets) {
            if (t.GetComponent<DialogueActor>()) {
                t.GetComponent<DialogueActor>().SetPhrase(null);
            }
        }

        //if (args.ClearOnClose) {
        //    foreach (var a in args.Dialogue.GetActors()) {
        //        a.GetTarget(args.ActorMap).GetComponentInSelfOrChild<DialogueActor>().SetPhrase(null);
        //    }
        //}
        PlayerManager.Instance.PlayerGameObject.GetComponent<DialogueActor>().SetPhrase(null);
        if (StopCamera()) {
            yield return new WaitForSeconds(0.2f);
            foreach (var t in args.Targets) {
                OnExitDialogueEvent.Raise(t);
            }
            yield return new WaitForSeconds(0.8f);
        } else {
            yield return new WaitForSeconds(0.1f);
            foreach (var t in args.Targets) {
                OnExitDialogueEvent.Raise(t);
            }

        }

        Exit();
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }
}