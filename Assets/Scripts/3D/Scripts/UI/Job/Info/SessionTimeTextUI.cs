using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ResourcePath("UI/Element/SessionTime")]
public class SessionTimeTextUI : UIMonoBehaviour, ITemporaryUI<string,object> {

    public void Close() {
       CrystallizeEventManager.Input.OnLeftClick -= Input_OnLeftClick;
       Complete.Raise(this, null);
       Complete = null;
        Destroy(gameObject);
    }

    public void Initialize(string param1) {
        GetComponent<Text>().text = param1;
        StartCoroutine(UIUtil.FadeInAndOutRoutine(canvasGroup, 1f, 5f, 1f));
        CrystallizeEventManager.Input.OnLeftClick += Input_OnLeftClick;
    }

    void Input_OnLeftClick(object sender, System.EventArgs e) {
        Close();
    }

    public event System.EventHandler<EventArgs<object>> Complete;

    IEnumerator Start() {
        yield return new WaitForSeconds(6.5f);
        Close();
    }

}