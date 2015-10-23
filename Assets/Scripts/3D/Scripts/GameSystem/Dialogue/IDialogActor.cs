using UnityEngine;
using System;
using System.Collections;

public interface IDialogActor {

	event EventHandler<PhraseEventArgs> OnOpenDialog;
	event EventHandler<PhraseEventArgs> OnExitDialog;
	event EventHandler<PhraseEventArgs> OnDialogueSuccess;

	PointerType SpeechBubblePointerType { get; }

}
