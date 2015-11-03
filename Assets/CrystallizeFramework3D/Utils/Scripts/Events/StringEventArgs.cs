using UnityEngine;
using System;
using System.Collections;

public class StringEventArgs : EventArgs {

	public string Message { get; private set; }

	public StringEventArgs(string message){
		Message = message;
	}

}
