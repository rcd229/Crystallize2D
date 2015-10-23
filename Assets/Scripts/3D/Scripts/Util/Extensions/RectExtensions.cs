using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class RectExtensions {
    /// <summary>
    /// Creates a new Rect shifted out by the amount indicated.
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="border"></param>
    /// <returns></returns>
    public static Rect Border(this Rect rect, float border) {
        return new Rect(rect.x - border, rect.y - border, rect.width + border * 2, rect.height + border * 2);
    }

    public static Vector2 Raycast(this Rect rect, Vector2 origin, Vector2 direction) {
        float w = 0;
        float h = 0;
        if (direction.x > 0) {
            w = rect.xMax - origin.x;
        } else {
            w = rect.xMin - origin.x;
        }

        if (direction.y > 0) {
            h = rect.yMax - origin.y;
        } else {
            h = rect.yMin - origin.y;
        }

        var projectedX = direction * (w / direction.x);
        var projectedY = direction * (h / direction.y);
        if (Mathf.Abs(projectedX.x) < Mathf.Abs(projectedY.x)) {
            return projectedX;
        } else {
            return projectedY;
        }
    }
}
