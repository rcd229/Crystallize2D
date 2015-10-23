using UnityEngine;
using System.Collections;

public interface IPhraseDragHandler {

	bool IsDragging { get; }
	GameObject BeginDrag(PhraseSequenceElement phraseElement, Vector2 mousePosition);

}
