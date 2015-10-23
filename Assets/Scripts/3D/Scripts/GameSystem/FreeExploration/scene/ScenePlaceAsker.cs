using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ScenePlaceAsker : MonoBehaviour {

	public List<PlaceEnvironmentPhrase> Targets{get;set;}

	static float distance = 20f;

	void Start(){
		Targets = (GameObject.FindObjectsOfType(typeof(PlaceEnvironmentPhrase)) as PlaceEnvironmentPhrase[])
			.Where(t => Vector3.Distance(t.gameObject.transform.position, transform.position) < distance)
			.ToList();
	}
}
