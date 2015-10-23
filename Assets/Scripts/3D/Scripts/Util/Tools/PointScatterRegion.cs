using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PointScatterRegion {
    public static List<Vector3> GeneratePositions(int count, Vector3 outerOrigin, float innerRadius, float outerRadius) {
        return GeneratePositions(count, outerOrigin, outerRadius, outerOrigin, innerRadius);
    }

    public static List<Vector3> GeneratePositions(int count, Vector3 outerOrigin, float outerRadius, Vector3 innerOrigin, float innerRadius) {
        return new PointScatterRegion(outerOrigin, outerRadius, innerOrigin, innerRadius).GeneratePositions(count);
    }

    Rect outerRect;
    Rect innerRect;

    public PointScatterRegion(Vector3 outerOrigin, float outerRadius, Vector3 innerOrigin, float innerRadius) {
        outerRect = new Rect(0, 0, 2f * outerRadius, 2f * outerRadius);
        outerRect.center = outerOrigin.XZToVector2();

        innerRect = new Rect(0, 0, 2f * innerRadius, 2f * innerRadius);
        innerRect.center = innerOrigin.XZToVector2();
    }

    public List<Vector3> GeneratePositions(int count) {
        return outerRect.ScatterPoints(count, 50f, innerRect);
    }
}
