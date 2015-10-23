using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class MenuNodeCompo : MenuAcceptor {
	
	protected List<MenuAcceptor> children;

	public MenuNodeCompo(){
		children = new List<MenuAcceptor>();
	}
	

	public override bool Accept (MenuVisitor visitor)
	{
		if(visitor.VisitEnter(this)){
			foreach(var child in children){
				if(!child.Accept(visitor))
					break;
			}
		}

		return visitor.VisitLeave(this);
	}
	

}
