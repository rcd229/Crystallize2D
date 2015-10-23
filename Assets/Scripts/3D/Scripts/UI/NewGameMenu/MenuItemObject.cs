using UnityEngine;
using System.Collections;


/**
 * A GameObject that stores data in hierarchical form
 */ 
public class MenuItemObjectRef {
	public GameObject GO {get;set;}
	MenuAcceptor data;
	public MenuAcceptor Data{get{return data;}}
	public MenuItemObjectRef(MenuAcceptor data){
		this.data = data;
	}
}
