using UnityEngine;
using System.Collections;

public abstract class MenuVisitorBase : MenuVisitor {
	public abstract bool VisitEnter (ImageMenuNode node);
	
	public abstract bool VisitLeave (ImageMenuNode node);
	
	public abstract bool Visit (TextMenuNode node);
	
	public abstract bool Visit (ValuedMenuNode node);
	
	

	public bool Visit (MenuNodeLeaf node)
	{
		throw new System.NotImplementedException ();
	}

	public bool VisitEnter (MenuNodeCompo node)
	{
		throw new System.NotImplementedException ();
	}

	public bool VisitLeave (MenuNodeCompo node)
	{
		throw new System.NotImplementedException ();
	}





}
