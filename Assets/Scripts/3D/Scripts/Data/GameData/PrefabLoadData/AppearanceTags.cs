using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AppearanceTags : MonoBehaviour {

	private List<string> tags = new List<string>();
	public IEnumerable<string> Tags {
		get{
			return tags;
		}
	}

	public void AddTag(string tag){
		tags.Add(tag);
	}

	public void AddTagList(IEnumerable<string> taglist){
		tags.AddRange(taglist);
	}
}
