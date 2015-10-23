using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EveningProcess : TimeSessionProcess<EveningSessionArgs, HomeRef>, IProcess<EveningSessionArgs, HomeRef> {

    public static readonly ProcessFactoryRef<object, TimeSessionArgs> RequestExplore = new ProcessFactoryRef<object, TimeSessionArgs>();

    protected override string TimeName {
        get { return TagLibrary.Evening; }
    }

    public override void Initialize(EveningSessionArgs input) {
        MusicManager.FadeToMusic(MusicType.Night1);
        base.Initialize(input);
    }

    protected override void AfterLoad() {
        SceneAreaManager.Instance.Get(TagLibrary.Area02);

        UILibrary.MoneyState.Get(null);
        var ui = UILibrary.HomeSelectionPanel.Get(null);
        if (Network.connections.Length > 0 || GameSettings.Instance.IsDebug) {
            UILibrary.LeaderBoard.Get(null);
        }
        ui.Complete += ui_Complete;

        if (!PlayerData.Instance.Tutorial.GetTutorialViewed(TagLibrary.Shops)) {
            ProcessLibrary.MessageBox.Get("The people here sell things. You can approach and click on them to interact.", AfterTutorialCallback, this);
            PlayerData.Instance.Tutorial.SetTutorialViewed(TagLibrary.Shops);
        } else {
            BeginExplore();
        }
    }

    void AfterTutorialCallback(object sender, object args) {
        BeginExplore();
    }

    void BeginExplore() {
        ProcessLibrary.Explore.Get(new ExploreInitArgs(null, TagLibrary.Actor), ExploreCallback, this);
    }

    void ExploreCallback(object sender, ExploreResultArgs exploreResult) {
        var interactable = exploreResult.Target.GetInterface<IInteractableSceneObject>();
        if (interactable != null) {
            interactable.BeginInteraction(InteractionComplete, this);
        } else {
            Debug.Log("Nothing valid was found.");
            BeginExplore();
        }
    }

    void InteractionComplete(object obj, object args) {
        BeginExplore();
    }

    void ui_Complete(object sender, EventArgs<HomeRef> e) {
        Exit(e.Data);
    }

    void ReturnHomeCallback(object sender, TimeSessionArgs args) {
        Exit();
    }

}
