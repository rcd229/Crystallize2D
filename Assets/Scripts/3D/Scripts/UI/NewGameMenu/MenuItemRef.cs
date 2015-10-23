using UnityEngine;
using System.Collections;

public class MenuItemRef {

	MenuAcceptor data;
	public MenuAcceptor Data {get{return data;}}
	public MenuItemRef(MenuAcceptor data){
		this.data = data;
	}
}
