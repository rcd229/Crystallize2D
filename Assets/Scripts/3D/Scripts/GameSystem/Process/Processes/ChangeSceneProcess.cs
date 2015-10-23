using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ChangeSceneProcess : EnumeratorProcess<SceneData, object> {

    public override IEnumerator<SubProcess> Run(SceneData args) {
        BlackScreenUI.Instance.FadeOut(0.5f, ContinueCallback);
        yield return Wait();

        Application.LoadLevel(args.SceneID);
        SceneChangeEvent.Instance.SceneChanged += Continue;
        yield return Wait();
        SceneChangeEvent.Instance.SceneChanged -= Continue;
        BlackScreenUI.Instance.FadeIn(1f, ContinueCallback);

        var sceneTarget = SceneConnectionTarget.Get(args);
        if (sceneTarget == null) {
            Debug.LogWarning("No scene connection for " + args.Name);
        } else {
            var r = OmniscientCamera.main.transform.rotation.eulerAngles;
            r.y = sceneTarget.transform.rotation.eulerAngles.y;
            OmniscientCamera.main.transform.rotation = Quaternion.Euler(r);
            OmniscientCamera.main.SetY(r.y);
            PlayerManager.Instance.PlayerGameObject.GetComponent<Rigidbody>().position = sceneTarget.transform.position;
        }

        yield return Wait();
    }

}
