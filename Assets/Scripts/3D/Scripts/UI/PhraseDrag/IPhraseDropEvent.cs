using UnityEngine;
using System;
using System.Collections;

public interface IPhraseDropEvent {

	event EventHandler<PhraseEventArgs> OnPhraseDropped;

}
