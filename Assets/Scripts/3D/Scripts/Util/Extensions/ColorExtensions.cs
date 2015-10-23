using UnityEngine;
using System.Collections;

public static class ColorExtensions {

    public static Color Lighten(this Color c, float amount) {
        return Color.Lerp(c, Color.white, amount);
    }

    public static Color Darken(this Color c, float amount) {
        return Color.Lerp(c, Color.black, amount);
    }

    public static Color SetTransparency(this Color c, float a) {
        c.a = a;
        return c;
    }

}