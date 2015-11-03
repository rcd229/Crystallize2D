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

public static class PointScatterRegionExtensions {
    const string blockedLayer = "Blocked";

    public static bool ScatterPoint(this Rect rect, float height, out Vector3 point) {
        point = Vector3.zero;
        int maxTimes = 50;
        while (maxTimes >= 0) {
            var y = height;
            var x = UnityEngine.Random.Range(rect.xMin, rect.xMax);
            var z = UnityEngine.Random.Range(rect.yMin, rect.yMax);
            var newP = new Vector3(x, y, z);
            if (CanAdd(newP, out newP)) {
                point = newP;
                return true;
            }

            maxTimes--;
        }
        return false;
    }

    static bool CanAdd(Vector3 point, out Vector3 hitPoint) {
        hitPoint = Vector3.zero;

        var ray = new Ray(point, Vector3.down);
        var hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 100f)) {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer(blockedLayer)) {
                return false;
            } else if (hit.rigidbody) {
                return false;
            } else {
                hitPoint = hit.point;
                return true;
            }
        } else {
            //Debug.Log("nothing hit");
            return false;
        }
    }

    public static List<Vector3> ScatterPoints(this Rect rect, int count, float height, Rect notRect = new Rect()) {
        var current = new List<Vector3>();
        int maxTimes = 50;
        while (maxTimes >= 0 && current.Count < count) {
            var y = height;
            var x = UnityEngine.Random.Range(rect.xMin, rect.xMax);
            var z = UnityEngine.Random.Range(rect.yMin, rect.yMax);
            var newP = new Vector3(x, y, z);

            if (!notRect.Contains(newP.XZToVector2()) && CanAdd(current, 4f, newP, out newP)) {
                current.Add(newP);
            }

            maxTimes--;
        }
        return current;
    }

    static bool CanAdd(IEnumerable<Vector3> currentPoints, float minDistance, Vector3 point, out Vector3 hitPoint) {
        hitPoint = Vector3.zero;

        var sqrDist = minDistance * minDistance;
        foreach (var p in currentPoints) {
            if (Vector2.SqrMagnitude(point.XZToVector2() - p.XZToVector2()) < sqrDist) {
                return false;
            }
        }

        var ray = new Ray(point, Vector3.down);
        var hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 100f)) {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer(blockedLayer)) {
                return false;
            } else {
                hitPoint = hit.point;
                return true;
            }
        } else {
            return false;
        }
    }

}