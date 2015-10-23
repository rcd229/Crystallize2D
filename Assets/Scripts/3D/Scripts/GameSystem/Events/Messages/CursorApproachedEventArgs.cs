using UnityEngine;
using System.Collections;

public class CursorApproachedEventArgs : System.EventArgs {

	public int ActorPlayerID { get; set; }
	public int CursorPlayerID { get; set; }

	public CursorApproachedEventArgs(int actorPlayerID, int cursorPlayerID){
		ActorPlayerID = actorPlayerID;
		CursorPlayerID = cursorPlayerID;
	}

}
