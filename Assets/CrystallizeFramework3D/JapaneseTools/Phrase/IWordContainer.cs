using UnityEngine;
using System.Collections;

public interface IWordContainer {

	GameObject gameObject { get; }
	PhraseSequenceElement Word { get ; }

}
