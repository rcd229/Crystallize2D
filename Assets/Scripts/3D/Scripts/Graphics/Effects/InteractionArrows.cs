using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractionArrows : MonoBehaviour {

    //public GameObject _self;
    //public GameObject _ally;

    public GameObject npcGroundCirclePrefab;

    List<GameObject> npcGroundCircleInstances = new List<GameObject>();

    public Transform selfCircle;
    public Transform allyCircle;

    public Transform selfRect;
    public Transform selfHead;

    public Transform allyRect;
    public Transform allyHead;

    public Color allyColor;
    public Color npcColor;

    Material[] materials;

	// Use this for initialization
	void Start () {
        allyCircle.SetParent(null);

        materials = new Material[2];
        materials[0] = allyRect.GetComponentInChildren<Renderer>().material;
        materials[1] = allyHead.GetComponentInChildren<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        //if(!NetworkMessageManager.IsConnected){
        //    return;
        //}

        var pl1 = GameObject.FindGameObjectWithTag("Player");
        var pl2 = GameObject.FindGameObjectWithTag("OtherPlayer");

        if(!(pl1 && pl2)){
            return;
        }

        var p1 = pl1.transform.position; //_self.transform.position;
        var p2 = pl2.transform.position; //_ally.transform.position;
        
        if (PlayerManager.Instance.PlayerGameObject != pl1) {
            p1 = pl2.transform.position;
            p2 = pl1.transform.position;
        }

        UpdateAllyCircle(p2);

        //var it = InteractionManager.GetInteractionTarget();
        //if (it) { 
        //    transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, 5f * Time.deltaTime);
        //    p2 = it.transform.position;
        //    if (it.transform.IsHumanControlled()) {
        //        SetArrowColor(allyColor);
        //    } else {
        //        SetArrowColor(npcColor);
        //    }
        //} else {
        //    //transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, 5f * Time.deltaTime);
        //    transform.localScale = Vector3.zero;
        //}

        UpdateArrows(p1, p2);
        selfCircle.position = p1;

        SetNPCCircles();
	}

    void UpdateAllyCircle(Vector3 p2) {
        allyCircle.position = p2;
    }

    void UpdateArrows(Vector3 p1, Vector3 p2) {
        var midPoint = Vector3.Lerp(p1, p2, 0.5f);
        var offset = p2 - p1;
        var dir = offset.normalized;
        var dist = offset.magnitude;

        transform.position = midPoint;

        if (dist < 2.5f) {
            selfRect.gameObject.SetActive(false);
            allyRect.gameObject.SetActive(false);
        } else {
            selfRect.gameObject.SetActive(true);
            allyRect.gameObject.SetActive(true);

            selfRect.localScale = new Vector3(1f, 1f, dist - 2.5f);
            allyRect.localScale = new Vector3(1f, 1f, dist - 2.5f);

            selfRect.forward = dir;
            allyRect.forward = dir;

            selfRect.position = midPoint + selfRect.right * 0.2f;
            allyRect.position = midPoint - selfRect.right * 0.2f;
        }

        selfHead.forward = dir;
        allyHead.forward = -dir;

        selfHead.position = midPoint + selfHead.right * 0.2f + selfHead.forward * (dist * 0.5f - 1f);
        allyHead.position = midPoint + allyHead.right * 0.2f + allyHead.forward * (dist * 0.5f - 1f);
    }

    void SetArrowColor(Color c) {
        foreach (var m in materials) {
            m.SetColor("_TintColor", c);
        }
    }

    void SetNPCCircles() {
        var interactionPoints = new List<IInteractionPoint>();
        foreach (var a in ActorTracker.Actors) {
            var ip = a.GetInterface<IInteractionPoint>();
            if (ip != null) {
                if (ip.GetInteractionEnabled()) {
                    interactionPoints.Add(ip);
                }
            }
        }

        while (npcGroundCircleInstances.Count < interactionPoints.Count) {
            var go = Instantiate<GameObject>(npcGroundCirclePrefab);
            npcGroundCircleInstances.Add(go);
        }

        int i = 0;
        for (i = 0; i < interactionPoints.Count; i++) {
            npcGroundCircleInstances[i].SetActive(true);
            npcGroundCircleInstances[i].transform.position = interactionPoints[i].GetPosition();
            npcGroundCircleInstances[i].transform.localScale = interactionPoints[i].GetRadius() * Vector3.one;
        }

        while (i < npcGroundCircleInstances.Count) {
            npcGroundCircleInstances[i].SetActive(false);
            i++;
        }
    }

}
