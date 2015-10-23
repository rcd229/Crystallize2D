using UnityEngine;
using System;
using System.Collections;

public interface ISpeechTextSource {

	event EventHandler<PhraseEventArgs> OnSpeechTextChanged;

}
