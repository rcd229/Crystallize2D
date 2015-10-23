using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class AnimatedCollectedItemArgs {

    public PhraseSequence Phrase { get; set; }
    public Vector2 TargetPosition { get; set; }
    public bool IsDragging { get; set; }
    public bool SetPos { get; set; }

    public AnimatedCollectedItemArgs(Vector2 target) {
        Phrase = null;
        TargetPosition = target;
        IsDragging = false;
        SetPos = true;
    }

    public AnimatedCollectedItemArgs(PhraseSequence phrase) {
        Phrase = phrase;
        TargetPosition = Vector2.zero;
        IsDragging = true;
        SetPos = true;
    }

    public AnimatedCollectedItemArgs(PhraseSequence phrase, Vector2 target) {
        Phrase = phrase;
        TargetPosition = target;
        IsDragging = false;
        SetPos = true;
    }

    public AnimatedCollectedItemArgs(PhraseSequence phrase, Vector2 target, bool isDragging) {
        Phrase = phrase;
        TargetPosition = target;
        IsDragging = isDragging;
        SetPos = true;
    }

}
