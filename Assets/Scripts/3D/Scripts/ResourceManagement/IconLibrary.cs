using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class IconType : ResourceType<Sprite> {

    public static readonly IconType ShoppingCart = new IconType("ShoppingCartIcon");
    public static readonly IconType Briefcase = new IconType("BriefcaseIcon");
    public static readonly IconType Scroll = new IconType("ScrollIcon");
    public static readonly IconType SpeechBubble = new IconType("SpeechBubbleIcon");
    public static readonly IconType ExclamationMark = new IconType("ExclamationPoint");
    public static readonly IconType QuestionMark = new IconType("QuestionMark");
    public static readonly IconType Home = new IconType("HomeIcon");

    public Sprite Image {
        get {
            return Resources.Load<Sprite>(ResourcePath);
        }
    }

    protected override string ResourceDirectory {
        get {
            return "Images/Icons/";
        }
    }

    IconType(string name) : base(name) {    }

}

public class OverheadIcon {
    public virtual IconType Type { get; private set; }
    public virtual Color Color { get; private set; }
    protected OverheadIcon() { }
    public OverheadIcon(IconType type, Color color = default(Color)) {
        Type = type;
        if (color == default(Color)) {
            Color = Color.white;
        } else {
            Color = color;
        }
    }
}