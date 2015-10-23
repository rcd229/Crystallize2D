using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HoverJobMissingWordsUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public IJobRef job;
    public bool mastered;
    HoverJobMissingWordsPanelUI panel;

    public void Initialize(IJobRef job) {
        this.job = job;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (job != null) {
            var words = job.GetUnlearnedWords();
            if (words.Count > 0 && words.Count <= 5){
                panel.transform.position = transform.position;
                panel.Initialize(job);
            }
        }
    }

    IEnumerator Start() {
        yield return null;
        this.panel = GameObject.FindObjectOfType<HoverJobMissingWordsPanelUI>();
    }

    public void OnPointerExit(PointerEventData eventData) {
        panel.Close();
    }
}