using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ExploreTutorialProcess : ExploreProcess {

    ExploreInitArgs args;
    ITemporaryUI ui;
    ITemporaryUI contextStatus;

    public override void Initialize(ExploreInitArgs args) {
        this.args = args;
        CoroutineManager.Instance.StartCoroutine(WaitForFadeIn());
    }

    protected override void BeforeExit() {
        contextStatus.CloseIfNotNull();
    }

    IEnumerator WaitForFadeIn() {
        yield return null;
        yield return null;
        while (BlackScreenUI.Instance && BlackScreenUI.Instance.canvasGroup.alpha > 0) {
            yield return null;
        }
        DoCameraTutorial();
    }

    void DoCameraTutorial() {
        MainCanvas.main.PushLayer();
        var instance = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("UI/Tutorial/CameraTutorial"));
        MainCanvas.main.Add(instance.transform);
        ProcessLibrary.ListenForInput.Get(
            new InputListenerArgs(InputType.RightClick),
            (s, e) => {
                CoroutineManager.Instance.StartCoroutine(MoveCoroutine());
                GameObject.Destroy(instance);
            },
            this);
        PlayerData.Instance.Tutorial.SetTutorialViewed("Camera");
    }

    IEnumerator MoveCoroutine() {
        BeginMovement(args);

        yield return null;

        ui = UILibrary.MovementTutorial.Get(null);
        var p = PlayerManager.Instance.PlayerGameObject.transform;
        var start = p.position;
        float totalDist = 0;

        while (Vector3.Distance(p.position, start) < 2f && totalDist < 4f) {
            var pos = p.position;
            yield return null;
            totalDist += Vector3.Distance(p.position, pos);
        }

        ui.CloseIfNotNull();
        MainCanvas.main.PopLayer();

        BeginAction(args);
        contextStatus = UILibrary.ContextActionStatus.Get(new ContextActionArgs("Click on the door to continue", true, false));
    }
}

//public class MovementTutorialProcess : IProcess<object, object> {

//    public ProcessExitCallback OnExit { get; set; }

//    ITemporaryUI ui;

//    public void ForceExit() {
//        Exit();
//    }

//    public void Initialize(object param1) {
//        ui = UILibrary.MovementTutorial.Get(null);
//        MainCanvas.main.PushLayer();
//        CoroutineManager.Instance.StartCoroutine(MoveCoroutine());
//    }

//    IEnumerator MoveCoroutine() {
//        yield return null;

//        var instance = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("UI/Tutorial/CameraTutorial"));
//        MainCanvas.main.Add(instance.transform);
//        ProcessLibrary.ListenForInput.Get(
//            new InputListenerArgs(InputType.RightClick),
//            (s, e) => {
//                BeginMovement();
//                GameObject.Destroy(instance);
//            },
//            this);
//        PlayerData.Instance.Tutorial.SetTutorialViewed("Camera");

//        var p = PlayerManager.Instance.PlayerGameObject.transform;
//        var start = p.position;

//        while (Vector3.Distance(p.position, start) < 2f) {
//            yield return null;
//        }

//        ui.CloseIfNotNull();
//        Exit();
//    }

//    void Exit() {
//        MainCanvas.main.PopLayer();
//        OnExit.Raise(this, null);
//    }

//}
