using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SoundEffectType : ResourceType<AudioClip> {

    public static readonly SoundEffectType PositiveFeedback = new SoundEffectType("PianoSmallSuccess");
    public static readonly SoundEffectType NegativeFeedback = new SoundEffectType("Block");
    public static readonly SoundEffectType Pop = new SoundEffectType("Pop");
    public static readonly SoundEffectType Buy = new SoundEffectType("buy_02");
    public static readonly SoundEffectType Invalid = new SoundEffectType("MenuFail");

    protected override string ResourceDirectory { get { return "SoundEffect/"; } }

    SoundEffectType(string name) : base(name) {  }

}
