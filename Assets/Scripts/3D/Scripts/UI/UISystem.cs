using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public enum UIMode
{
    Exploring,
    Speaking,
    FixedPhraseInput
}

public class UISystem : MonoBehaviour {

    const string ResourcePath = "UI/UISystem";

	public static UISystem main { 
        get{
            return GetInstance();
        }
    }

    static UISystem _instance;
    public static UISystem GetInstance() {
        if (EventSystem.current == null) {
            GameObjectUtil.GetResourceInstance("UI/EventSystem");
        }

        if (!_instance) {
            _instance = GameObjectUtil.GetResourceInstance<UISystem>(ResourcePath);
        }
        return _instance;
    }

    public static bool MouseOverUI() {
        if (main.PhraseDragHandler.IsDragging) {
            return true;
        }

        return UIUtil.MouseOverUI();
    }

	public IPhraseDragHandler PhraseDragHandler { get; set; }

	[SerializeField]
	GameObject phraseDragHandlerObject;

    HashSet<object> centerPanels = new HashSet<object>();
    bool mainMenuOpen = false;

    public IEnumerable<object> CenterPanels {
        get {
            return centerPanels;
        }
    }

    public event EventHandler<UIInputEventArgs> OnInputEvent;

    KeyCode[] UIKeys = { KeyCode.Space, KeyCode.Return };

	// Use this for initialization
	void Awake() {
        if (_instance) {
            Debug.LogWarning("Already have a UISystem!");
        }

		PhraseDragHandler = phraseDragHandlerObject.GetInterface<IPhraseDragHandler> ();
	}

    void Update() {
        if (mainMenuOpen) {
            return;
        }

        foreach (var uiKey in UIKeys) {
            if (Input.GetKeyDown(uiKey)) {
                if (OnInputEvent != null) {
                    OnInputEvent(this, new UIInputEventArgs(uiKey));
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            OpenMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            CrystallizeEventManager.Input.RaiseEnvironmentClick(this, System.EventArgs.Empty);
        }

        if (Input.GetMouseButtonDown(0)) {
            CrystallizeEventManager.Input.RaiseLeftClick(this, System.EventArgs.Empty);
            if (!UISystem.MouseOverUI()) {
                CrystallizeEventManager.Input.RaiseEnvironmentClick(this, System.EventArgs.Empty);
            }
        }

        if (Input.GetMouseButtonUp(1)) {
            CrystallizeEventManager.Input.RaiseRightClick(this, System.EventArgs.Empty);
        }
    }

    public void OpenMainMenu() {
        mainMenuOpen = true;
        UILibrary.MainMenu.Get(null, MainMenuClosed);
    }

    void MainMenuClosed(object obj, object arg) {
        mainMenuOpen = false;
    }

    public void AddCenterPanel(object panel) {
        Debug.Log("Adding " + panel);
        if (!centerPanels.Contains(panel)) {
            centerPanels.Add(panel);
        }
    }

    public void RemoveCenterPanel(object panel) {
        Debug.Log("Removing " + panel);
        if (centerPanels.Contains(panel)) {
            centerPanels.Remove(panel);
        }
    }

    public bool ContainsCenterPanel() {
        return centerPanels.Count > 0;
    }

}
