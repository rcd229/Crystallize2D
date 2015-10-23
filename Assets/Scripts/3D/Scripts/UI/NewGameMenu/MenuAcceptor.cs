using UnityEngine;
using System.Collections;

public abstract class MenuAcceptor : GameMenuItem, IMenuAcceptor{
	public abstract bool Accept(MenuVisitor visitor);
}
