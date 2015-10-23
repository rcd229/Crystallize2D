using UnityEngine;
using System.Collections;

public class ContextDataExpressedEventArgs : System.EventArgs {

	public int SourceWorldID { get; set; }
	public string ContextItemLabel { get; set; }

	public ContextDataExpressedEventArgs(int sourceWorldID, string contextItemLabel){
		SourceWorldID = sourceWorldID;
		ContextItemLabel = contextItemLabel;
	}

}
