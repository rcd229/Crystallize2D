using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialCanvas : MonoBehaviour {

	public static TutorialCanvas main { get; set; }

	void Awake(){
		main = this;
	}

	public Sprite dotImage;
	public GameObject uiDownArrowPrefab;
	public GameObject uiUpArrowPrefab;
	public GameObject uiRightArrowPrefab;
    public GameObject dragBoxPrefab;

	//public IInventoryUI InventoryUI { get; set; }
	//public IObjectiveUI ObjectiveUI { get; set; }
	//public IClientUI ClientUI { get; set; }
	//public IExperienceUI ExperienceUI { get; set; }
	//public IConversationUI ConversationUI { get; set; }
    //public IFullInventoryUI FullInventoryButton { get; set; }

	List<GameObject> uiArrowInstances = new List<GameObject>();
	HashSet<TutorialDragLine> uiLines = new HashSet<TutorialDragLine>();
	HashSet<TutorialWorldLine> worldLines = new HashSet<TutorialWorldLine>();

    Dictionary<string, GameObject> namedTransforms = new Dictionary<string, GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		foreach (var line in uiLines) {
			line.Update();
		}

		//Debug.Log ("World lines: " + worldLines.Count);
		foreach (var line in worldLines) {
			line.Update();
		}
	}

    //public RectTransform GetWordObjective(PhraseSequenceElement word){
    //    if (ObjectiveUI != null) {
    //        return ObjectiveUI.GetObjective(word);
    //    }
    //    return null;	
    //}

    //public RectTransform GetWordEntry(PhraseSequenceElement word){
    //    if (InventoryUI != null) {
    //        return InventoryUI.GetEntry(word);
    //    }
    //    return null;	
    //}

    public void CreateUIDragBox(RectTransform target, string text) {
        var b = Instantiate(dragBoxPrefab) as GameObject;
        //Debug.Log(b);
        b.transform.SetParent(transform);
        b.GetComponent<DragBoxTutorialUI>().Initialize(target, text);
        uiArrowInstances.Add(b);
    }

	public void CreateUILine(RectTransform t1, RectTransform t2){
		uiLines.Add (new TutorialDragLine (t1, t2, GetComponent<RectTransform>(), dotImage));
	}

	public void CreateWorldLine(Transform t1, Transform t2){
		Debug.Log ("Adding world line.");
		worldLines.Add (new TutorialWorldLine (t1, t2, WorldCanvas.Instance.transform, dotImage));
	}

	public void CreateUIDownArrow(RectTransform target, Vector3 offset){
		var arrow = Instantiate (uiDownArrowPrefab) as GameObject;
		arrow.transform.SetParent (transform);
		arrow.GetComponent<LinearOscillate> ().target = target;
		arrow.transform.position = (Vector3)target.rect.center + target.position + offset;
		uiArrowInstances.Add (arrow);
	}

	public void CreateUIDownArrow(Vector3 position){
		var arrow = Instantiate (uiDownArrowPrefab) as GameObject;
		arrow.transform.SetParent (transform);
		arrow.transform.position = position;
		uiArrowInstances.Add (arrow);
	}

	public void CreateUIUpArrow(Vector3 position){
		var arrow = Instantiate (uiUpArrowPrefab) as GameObject;
		arrow.transform.SetParent (transform);
		arrow.transform.position = position;
		uiArrowInstances.Add (arrow);
	}

	public void CreateUIRightArrow(Vector3 position){
		var arrow = Instantiate (uiRightArrowPrefab) as GameObject;
		arrow.transform.SetParent (transform);
		arrow.transform.position = position;
		uiArrowInstances.Add (arrow);
	}

	public void ClearLines(){
		foreach (var line in uiLines) {
			line.Clear();
		}
		uiLines.Clear ();

		foreach (var line in worldLines) {
			line.Clear();
		}
		worldLines.Clear ();
	}

	public void ClearArrows(){
		foreach (var arrow in uiArrowInstances) {
			Destroy(arrow);
		}
		uiArrowInstances.Clear ();
	}

	public void ClearAllIndicators(){
		ClearLines ();
		ClearArrows ();
	}

    public T PlayTutorial<T>() where T : Component{
        var go = new GameObject("Tutorial");
        return go.AddComponent<T>();
    }

    public void RegisterGameObject(string s, GameObject target) {
        if (!namedTransforms.ContainsKey(s)) {
            namedTransforms[s] = target;
        } else {
            namedTransforms[s] = target;
            Debug.LogWarning("Already contains " + s + "!");
        }
    }

    public void UnregisterGameObject(string s) {
        Debug.Log("unregistering " + s);
        if (namedTransforms.ContainsKey(s)) {
            namedTransforms.Remove(s);
        } 
    }

    public GameObject GetRegisteredGameObject(string s) {
        if (namedTransforms.ContainsKey(s)) {
            return namedTransforms[s];
        } else {
            //Debug.LogWarning(s + " not in dictionary!");
            return null;
        }
    }

}
