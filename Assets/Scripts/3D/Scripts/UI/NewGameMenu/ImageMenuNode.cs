using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ImageMenuNode : MenuNodeCompo {
	
	public Sprite Image{get;set;}
	public MenuLayoutOption LayoutOption{get;set;}

	public ImageMenuNode(){
		Image = new Sprite();
		//default layout is grid
		LayoutOption = MenuLayoutOption.Grid;
	}
	

}
