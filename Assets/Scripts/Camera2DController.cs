using UnityEngine;
using System.Collections;
using Util;

public class Camera2DController : MonoBehaviour {

    const int HeightUnits = 11;
    const int WidthUnits = 23;

    Transform player;

    // Use this for initialization
    void Start() {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate() {
        var pos = player.position - Vector3.forward * 10f;

        var camBounds = GetCameraBounds(pos);
        var screenBounds = GetScreenBounds(pos);

        if (camBounds.xMin < screenBounds.xMin) 
            pos.x = screenBounds.xMin + camBounds.width * 0.5f;
        if (camBounds.xMax > screenBounds.xMax) 
            pos.x = screenBounds.xMax - camBounds.width * 0.5f;
        if (camBounds.yMin < screenBounds.yMin) 
            pos.y = screenBounds.yMin + camBounds.height * 0.5f;
        if (camBounds.yMax > screenBounds.yMax)
            pos.y = screenBounds.yMax - camBounds.height * 0.5f;

        transform.position = pos;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        //var p0 = -0.5f * WidthUnits * Vector3.right + -0.5f * HeightUnits * Vector3.up;
        var p0 = (Vector3)GetScreenBounds(transform.position).position;
        var p1 = p0 + WidthUnits * Vector3.right;
        var p2 = p1 + HeightUnits * Vector3.up;
        var p3 = p0 + HeightUnits * Vector3.up;
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p0);

        Gizmos.color = Color.red;
        var c0 = transform.position - 0.5f * CameraWidth() * Vector3.right - 0.5f * CameraHeight() * Vector3.up;
        var c1 = c0 + CameraWidth() * Vector3.right;
        Gizmos.DrawLine(c0, c1);
    }

    float CameraWidth() {
        return 2f * GetComponent<Camera>().aspect * GetComponent<Camera>().orthographicSize;
    }

    float CameraHeight() {
        return 2f * GetComponent<Camera>().orthographicSize;
    }

    Vector2 RegionOffset() {
        return -0.5f * WidthUnits * Vector3.right + -0.5f * HeightUnits * Vector3.up;
    }

    Rect GetCurrentRegion() {
        var player = GameObject.Find("Player").transform;
        var offset = RegionOffset();
        var regionCenter = new Vector2(
            player.position.x - offset.x,
            player.position.y - offset.y
            );
        return new Rect();
    }

    Rect GetCameraBounds(Vector3 center) {
        var p0 = center - 0.5f * CameraWidth() * Vector3.right - 0.5f * CameraHeight() * Vector3.up;
        var p1 = p0 + CameraWidth() * Vector3.right + CameraHeight() * Vector3.up;
        return new Rect(p0, p1 - p0);
    }

    Rect GetScreenBounds() {
        return GetScreenBounds(player.position);
    }

    Rect GetScreenBounds(Vector3 center) { 
        var p0 = -0.5f * WidthUnits * Vector3.right + -0.5f * HeightUnits * Vector3.up;
        var playerPos = center - p0;
        playerPos.x = Mathf.Floor(playerPos.x / WidthUnits);
        playerPos.y = Mathf.Floor(playerPos.y / HeightUnits);
        p0 = p0 + playerPos.x * WidthUnits * Vector3.right + playerPos.y * HeightUnits * Vector3.up;
        var p1 = p0 + WidthUnits * Vector3.right + HeightUnits * Vector3.up;
        return new Rect(p0, p1 - p0);
    }

}
