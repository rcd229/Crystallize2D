using UnityEngine;
using System.Collections;
using System.Collections.Generic;   

public class HoverJobMissingWordsPanelUI : MonoBehaviour {

    public GameObject wordPrefab;
    public RectTransform wordParent;

    List<GameObject> instances = new List<GameObject>();

    public void Initialize(IJobRef job) {
        gameObject.SetActive(true);
        var phrases = job.GetUnlearnedWords();//new List<PhraseSequence>();
        //instances.DestroyAndClear();
        UIUtil.GenerateChildren(phrases, instances, wordParent, CreateChild);
    }

    GameObject CreateChild(PhraseSequence phrase) {
        var inst = GameObject.Instantiate<GameObject>(wordPrefab);
        inst.GetComponent<JobWordUI>().Initialize(new PhraseJobRequirementGameData(phrase));
        return inst;
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    IEnumerator Start() {
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }

}