using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * Menu Item builder is a visitor that traverses the nested structure of the menu item scriptable object
 * and build a gameobject from it. It constructs gameobjects and components from the data stored in the scriptable object
 * 
 */ 
public class MenuItemBuilder : MenuVisitorBase {

	class BuildProcess{
		public GameObject obj;
		public bool isModified;
		public BuildProcess parent;

		//represents a step in the build process, stores the depth in the item structure, 
		//the current child gameobject and if this gameobject is used
		public BuildProcess(GameObject o, bool b, BuildProcess p){
			obj = o;
			isModified = b;
			parent = p;
		}
	}

	GameObject buildResult;

	BuildProcess current;

	public static MenuItemObjectRef BuildMenuItemObject(MenuAcceptor data){
		MenuItemObjectRef ret = new MenuItemObjectRef(data);
		MenuItemBuilder builder = new MenuItemBuilder();
		ret.Data.Accept(builder);
		ret.GO = builder.buildResult;
		return ret;
	}

	public MenuItemBuilder(){
		buildResult = new GameObject();
		current = new BuildProcess(buildResult, true, null);
	}

	#region implemented abstract members of MenuVisitorBase
	public override bool VisitEnter (ImageMenuNode node)
	{
		//current gameobject will be used
		current.isModified = true;
		GameObject currentObj = current.obj;
		Image image = currentObj.AddComponent<Image>();
		image.sprite = node.Image;
		//create layout group based on layout option specified in the menu item
		LayoutGroup layout;
		switch (node.LayoutOption) 
		{
			case MenuLayoutOption.Horizontal:
				layout = currentObj.AddComponent<HorizontalLayoutGroup>();
				break;
			case MenuLayoutOption.Vertical:
				layout = currentObj.AddComponent<VerticalLayoutGroup>();
				break;
			case MenuLayoutOption.Grid:
				layout = currentObj.AddComponent<GridLayoutGroup>();
				break;
			default:
				throw new System.ArgumentOutOfRangeException ();
		}
		layout.childAlignment = TextAnchor.MiddleCenter;
		var fitter = currentObj.AddComponent<ContentSizeFitter>();
		fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
		fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
		//since image can have children, on entering the image item create the children
		BuildProcess child = new BuildProcess(new GameObject(), false, current); 
		current = child;
		return true;
	}

	public override bool VisitLeave (ImageMenuNode node)
	{
		//only retain this gameobject (children of image) if it has any content/is modified
		if(current.isModified){
			current.obj.transform.SetParent(current.parent.obj.transform);
			current.parent.isModified = true;
		}
		//otherwise we don't need it
		else{
			GameObject.Destroy(current.obj.gameObject);
		}
		current = current.parent;
		return true;
	}
	//for nodes without children, just instantiate the content
	public override bool Visit (TextMenuNode node)
	{
		current.isModified = true;
		GameObject currentObj = current.obj;
		currentObj.AddComponent<Text>().text = node.Text;
		return true;
	}
	public override bool Visit (ValuedMenuNode node)
	{
		current.isModified = true;
		GameObject currentObj = current.obj;
		currentObj.AddComponent<Text>().text = node.Value.ToString();
		return true;
	}
	#endregion
	
}
