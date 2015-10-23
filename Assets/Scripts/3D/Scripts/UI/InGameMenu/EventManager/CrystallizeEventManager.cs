using UnityEngine;
using System;

public partial class CrystallizeEventManager : MonoBehaviour {
	
	public static event EventHandler<MenuItemEventArg> MenuSelectionEvent;
	//TODO fired sender upstream instead of this. Should it be done this way?
	public static void SelectFromMenu(object sender, MenuItemEventArg args) { MenuSelectionEvent(sender, args); }
	
}

