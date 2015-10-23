using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class MusicType : ResourceType<AudioClip> {

    public static readonly MusicType Night1 = new MusicType("M5_Deserted Stability");
    public static readonly MusicType Day1 = new MusicType("sport_04_loop");
    public static readonly MusicType Promotion1 = new MusicType("sport_03_loop");
    public static readonly MusicType Morning1 = new MusicType("M2 Brewing Coffee");

    protected override string ResourceDirectory { get { return "Music/"; } }

    MusicType(string name) : base(name) { }

}
