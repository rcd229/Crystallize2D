using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class SceneAreaManager : MonoBehaviour {

    static SceneAreaManager _instance;
    public static SceneAreaManager Instance {
        get {
            if (!_instance) {
                _instance = new GameObject("SceneAreaManager").AddComponent<SceneAreaManager>();
            }
            return _instance;
        }
    }

    HashSet<GameObject> areas = new HashSet<GameObject>();

    void Awake() {
        var areas = GameObject.FindGameObjectsWithTag(TagLibrary.SceneArea);
        foreach (var a in areas) {
            Add(a);
        }
    }

    /// <summary>
    /// Add and disable the area if it exists
    /// </summary>
    /// <param name="area"></param>
    public void Add(GameObject area) {
        if (!areas.Contains(area)) {
            //Debug.Log("adding area:" + area);
            areas.Add(area);
            area.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Get and enable the area if it exists
    /// </summary>
    /// <param name="areaName"></param>
    /// <returns></returns>
    public GameObject Get(string areaName) {
        var area =  areas.Where(a => a.name == areaName);//.FirstOrDefault();
        if(area.Count() > 0){
            //Debug.Log("not found");
            area.First().gameObject.SetActive(true);
        }
        return area.FirstOrDefault();
    }

	public IEnumerable<GameObject> GetAll(IEnumerable<string> names){
		List<GameObject> ret = new List<GameObject>();;
		foreach(var name in names){
			ret.Add(Get(name));
		}
		return ret;
	}

	public void ActivateAll(){
		foreach(var area in areas){
			area.gameObject.SetActive(true);
		}
	}

}
