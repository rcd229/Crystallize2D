using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameProcess2D : EnumeratorProcess<object, object> {
    static GameObject _statusUI;
    public static ICloseable GetStatusUI() {
        if (!_statusUI) {
            _statusUI = GameObjectUtil.GetResourceInstance("UI/HUD2D");
            MainCanvas.main.Add(_statusUI.transform, CanvasBranch.Persistent);
        }
        return _statusUI.GetInterface<ICloseable>();
    }

    ITemporaryUI modeController;

    public override IEnumerator<SubProcess> Run(object args) {
        PrepareScene();
        BeginPlayMode(null, null);
        yield return Wait();
    }

    void PrepareScene() {
        UISystem.GetInstance();
        SceneInputHandler2D.Instance.OnObjectClicked += OnObjectClicked;

        GetStatusUI();
        GameObjectUtil.GetResourceInstance("DebugListener");

        TileResourceManager.Instance.Refresh();
        ThingResourceManager2D.Initialize();
        TriggerResourceManager2D.Initialize();

        UpdateThings();
        CoroutineManager.Get();
        ResourceCollection.GetOrCreate<SceneThing2D>().OnItemAdded += HandleThingsChanged;
        ResourceCollection.GetOrCreate<SceneThing2D>().OnItemRemoved += HandleThingsChanged;
    }

    void HandleThingsChanged(object sender, EventArgs args) {
        if (CoroutineManager.Alive)
            CoroutineManager.Instance.WaitAndDo(UpdateThings);
    }

    void UpdateThings() {
        foreach (var thing in ResourceCollection.GetOrCreate<SceneThing2D>()) {
            FloatingNameManager2D.Instance.Add(thing.transform, thing.Thing.Name);
        }
    }

    void OnObjectClicked(object sender, EventArgs<GameObject> e) {
        if (e.Data && e.Data.GetInterface<IInteractableSceneObject>() != null) {
            e.Data.GetInterface<IInteractableSceneObject>().BeginInteraction(null, this);
        }
    }

    void BeginPlayMode(object sender, object args) {
        modeController.CloseIfNotNull();
        var newContr = GameObjectUtil.NewWithComponent<PlayModeController2D>();
        newContr.Initialize(null, null, this);
        modeController = newContr;
        MainHUD2D.SetPlay();

        CoroutineManager.Instance.WaitAndDo(() => ListenForModeSwitch(BeginEditMode));
    }

    void BeginEditMode(object sender, object args) {
        modeController.CloseIfNotNull();
        var newContr = GameObjectUtil.GetResourceInstanceFromAttribute<TilePlacerUI2D>();
        newContr.Initialize(null, null, this);
        modeController = newContr;
        MainHUD2D.SetEdit();

        CoroutineManager.Instance.WaitAndDo(() => ListenForModeSwitch(BeginPlayMode));
    }

    void ListenForModeSwitch(ProcessExitCallback<InputListenerArgs> callback) {
        ProcessLibrary.ListenForInput.Get(new KeyInputListenerArgs(KeyCode.BackQuote), callback, this);
    }

}
