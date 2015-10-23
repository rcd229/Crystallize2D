using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class CoroutineManager : MonoBehaviour {

    static CoroutineManager _instance;

    public static bool Alive {
        get {
            return _instance;
        }
    }

    public static CoroutineManager Instance {
        get {
            if (!_instance) {
                _instance = new GameObject("Coroutines").AddComponent<CoroutineManager>();
            }
            return _instance;
        }
    }

    public static CoroutineManager Get() { return Instance; }

	public void WaitAndDo(Action action, object obj = null){
		StartCoroutine(WaitAndDoCoroutine(action, obj));
	}

	IEnumerator WaitAndDoCoroutine(Action action, object obj){
		yield return obj;

		if(action != null){
			action();
		}
	}

}
