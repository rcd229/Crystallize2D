using UnityEngine;
using System;
using System.Collections;

public class TextEventArgs : EventArgs {

	public static new TextEventArgs Empty { get; private set; }

	static TextEventArgs (){
		Empty = new TextEventArgs ();
	}

	public string Text { get; set; }

	TextEventArgs (){
	}

	public TextEventArgs (string text){
		Text = text;
	}

}
