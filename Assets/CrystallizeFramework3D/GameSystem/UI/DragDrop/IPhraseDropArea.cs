using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public interface IDropArea {
    void AcceptDrop(PhraseSequence phrase, GameObject draggedObject);
}