using UnityEngine;
using System.Collections;

public class DefaultWordDictionary : MonoBehaviour {
	PrefixTree<PhraseSequence> dictionary;
	static DefaultWordDictionary _instance;
	bool done{get; set;}
	public static bool Done{
		get{
			return _instance.done;
		}
	}
	// Use this for initialization
	void Awake(){
		done = false;
		_instance = this;
		StartCoroutine(load ());
	}

	IEnumerator load(){
		dictionary = new PrefixTree<PhraseSequence>(PhraseSetCollectionGameData.Default.Phrases);
		done = true;
		yield return null;
	}

	public static PrefixTree<PhraseSequence> Dict{
		get{
			return _instance.dictionary;
		}
	}

}
