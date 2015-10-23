using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ExploreInitArgs {

    public static ExploreInitArgs ActorArgs() {
        return new ExploreInitArgs(null, "Actor", "click to interact", "you can't interact with that");
    }

    public string ActiveItem { get; set; }
    public string RequiredTag { get; set; }
	public string ValidNotification{get; set;}
	public string InvalidNotification{get; set;}

    public ExploreInitArgs() {    }

    public ExploreInitArgs(string activeItem) {
        ActiveItem = activeItem;
        RequiredTag = "Place";
    }

    public ExploreInitArgs(string activeItem, string tag) : this(activeItem) {
        RequiredTag = tag;
    }

	public ExploreInitArgs(string activeItem, string valid, string invalid) : this(activeItem) {
		ValidNotification = valid;
		InvalidNotification = invalid;
	}

	public ExploreInitArgs(string activeItem, string tag, string valid, string invalid) : this(activeItem, valid, invalid){
		
        RequiredTag = tag;
	}

}

public class ExploreResultArgs {
    public GameObject Target { get; set; }

    public ExploreResultArgs(GameObject target) {
        this.Target = target;
    }
}
