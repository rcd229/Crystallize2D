using UnityEngine;
using System.Collections;

public abstract class MenuNodeLeaf : MenuAcceptor {
	#region MenuAcceptor implementation

	public override bool Accept (MenuVisitor visitor)
	{
		return visitor.Visit(this);
	}

	#endregion
	

}
