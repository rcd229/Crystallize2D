using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OmniscientCamera : MonoBehaviour {

    const int MouseButton = 1;

    static OmniscientCamera _main;
    public static OmniscientCamera main {
        get {
            if (!_main && Camera.main) {
                _main = Camera.main.GetComponent<OmniscientCamera>();
            }
            return _main;
        }
    }

    //List<GameObject> motionFilterObjects = new List<GameObject>();
    List<ICameraMotionFilter> motionFilters = new List<ICameraMotionFilter>();

    private Vector3 anchorMouse;
    private Vector3 anchorCamera;

    public Vector3 target;
    public Transform player;
    //private Transform player = null;
    public float distance = 10.0f;

    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float keyMoveSpeed = 5f;
    public float keyZoomSpeed = 5f;
    public float keyRotateSpeed = 50f;

    float x = 0.0f;
    float y = 0.0f;

    public bool lockedOn = true;
    public bool enableKeyMovement = true;
    public Vector3 offset = new Vector3(0, 1.5f, 0);
    public bool centerOnIdle = true;
    public float idleDuration = 5f;

    bool initialized = false;

    bool copyTransform = true;
    float minSpeed = 7f;
    Transform finalTransform;

    //float idleRotateSpeed = 0;
    float idleTime = 0;
    bool isIdle = true;

    bool suspended = false;
    Vector3 nextPosition;
    Quaternion nextRotation;

    HashSet<object> controlLocks = new HashSet<object>();

    float minDist = 2f;
    float maxDist = 5f;

    public bool ControlLocked {
        get {
            return controlLocks.Count > 0;
        }
    }

    void Awake() {
        _main = this;
    }

    void OnDestroy() {
        _main = null;
    }

    // Use this for initialization
    IEnumerator Start() {
        initialized = true;

        if (!player) {
            player = PlayerManager.Instance.PlayerGameObject.transform;
        }

        if (player) {
            var targ = player.FindChildWithTag("CameraTarget");
            if (targ != null) {
                player = targ;
                offset = Vector3.zero;
            }
        }

        finalTransform = new GameObject("OmniscientCameraTarget").transform;

        var angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        distance = minDist;

        if (!player) Unlock();

        yield return null;
        yield return null;
        yield return null;

        copyTransform = true;
        //SetPosition(finalTransform.position);
        //SetRotation(finalTransform.rotation);
    }

    public void AddMotionFilter(ICameraMotionFilter filter) {
        if (!motionFilters.Contains(filter)) {
            motionFilters.Add(filter);
        }
    }

    public void RemoveMotionFilter(ICameraMotionFilter filter) {
        if (motionFilters.Contains(filter)) {
            motionFilters.Remove(filter);
        }
    }

    void Update() {
        if (!initialized) { return; }
        if (suspended) { return; }
        if (!player) { return; }

        if (lockedOn) { target = player.position + offset; }

        if (ControlLocked) {
            return;
        }

        if (!Application.isEditor) {
            return;
            //Debug.Log("is editor");
        }

        if (!Input.GetKey(KeyCode.LeftControl)) {
            var scrollInput = Input.GetAxis("Mouse ScrollWheel") * 500f;
            if (scrollInput != 0 && !UISystem.MouseOverUI()) {
                finalTransform.position += finalTransform.forward * Time.deltaTime * scrollInput;
                distance -= Time.deltaTime * scrollInput;
            }
            distance = Mathf.Clamp(distance, minDist, maxDist);
        }

        if (!player) {
            Debug.Log("No player!");
            Unlock();
        }


        if (Input.GetKeyDown(KeyCode.Escape) && player) {
            lockedOn = true;
        }

        idleTime += Time.deltaTime;

        if (Input.GetMouseButton(MouseButton)) {
            isIdle = false;
            idleTime = 0;
            //idleRotateSpeed = 0;
        } else if (idleTime > idleDuration) {
            isIdle = true;
        }


        if (Input.GetKey(KeyCode.LeftShift)) {
            if (lockedOn) {
                Unlock();
            }
            if (Input.GetMouseButtonDown(MouseButton)) {
                anchorMouse = Input.mousePosition;
                anchorCamera = finalTransform.position;
            }
            if (Input.GetMouseButton(MouseButton)) {
                finalTransform.position = anchorCamera;
                finalTransform.Translate(-1 * (Input.mousePosition - anchorMouse) * 0.05f);
            }
        } else {
            if (Input.GetMouseButtonDown(MouseButton)) {
                target = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distance));
            }
            if (Input.GetMouseButtonUp(MouseButton)) {
                target = Vector3.zero;
            }
        }

        if (isIdle && centerOnIdle) {
            //var newX = player.parent.parent.rotation.eulerAngles.y;
            //Debug.Log(newX + "; " + x);
            //var targetSpeed = Mathf.Repeat(newX, 360f) - Mathf.Repeat(x, 360f);
            //targetSpeed = Mathf.Lerp
            //tarSpeed = Mathf.Sign(tarSpeed) * Mathf.Clamp(Mathf.Abs(tarSpeed), 10f, 180f);
            //idleRotateSpeed = Mathf.MoveTowards(idleRotateSpeed, tarSpeed, 180f * Time.deltaTime);
            //Debug.Log(idleRotateSpeed);
            //x = Mathf.MoveTowardsAngle(x, newX, 1000f * Time.deltaTime);//Mathf.Sign(tarSpeed) * Mathf.Sign(idleRotateSpeed) * Mathf.Abs(idleRotateSpeed) * Time.deltaTime);
        }
    }

    void LateUpdate() {
        if (!initialized) { return; }
        if (!player) { return; }

        CrystallizeEventManager.Environment.RaiseBeforeCameraMove(this, System.EventArgs.Empty);

        if (suspended) {
            transform.position = nextPosition;
            transform.rotation = nextRotation;
        } else {
            UpdateMouseOrbit();
        }
        nextPosition = transform.position;
        nextRotation = transform.rotation;

        CrystallizeEventManager.Environment.RaiseAfterCameraMove(this, System.EventArgs.Empty);
    }

    void UpdateMouseOrbit() {
        if (target != Vector3.zero) {
            if (Input.GetMouseButton(MouseButton)) {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                y = ClampAngle(y, yMinLimit, yMaxLimit);
            }

            var horiz = Input.GetAxis("AltHorizontal") * xSpeed * 0.005f;
            var vert = Input.GetAxis("AltVertical") * ySpeed * 0.005f;
            x += horiz;
            y += vert;
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0, 0, -distance) + target;
            finalTransform.position = position;
            finalTransform.rotation = rotation;
        }

        if (copyTransform) {
            transform.position = finalTransform.position;
            transform.rotation = finalTransform.rotation;
            copyTransform = false;
        } else {
            var dist = Vector3.Distance(transform.position, finalTransform.position);
            var moveDist = (minSpeed + dist) * Time.deltaTime;

            var oldPos = transform.position;
            var newPos = Vector3.MoveTowards(transform.position, finalTransform.position, moveDist);
            foreach (var f in motionFilters) {
                newPos = f.UpdatePosition(transform, oldPos, newPos);
            }
            transform.position = newPos;

            transform.rotation = Quaternion.Lerp(transform.rotation, finalTransform.rotation, moveDist / dist);
        }
    }

    public void Unlock() {
        lockedOn = false;
        target = Vector3.zero;
    }

    static float ClampAngle(float angle, float min, float max) {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    public void LockControl(object obj) {
        if (!controlLocks.Contains(obj))
            controlLocks.Add(obj);
    }

    public void UnlockControl(object obj) {
        if (controlLocks.Contains(obj)) {
            controlLocks.Remove(obj);
        }
    }

    //	bool enabledOnSuspend = false;
    public void Suspend() {
        //enabledOnSuspend = enabled;
        //enabled = false;
        suspended = true;
    }

    public void Resume() {
        //enabled = true;//enabledOnSuspend;
        suspended = false;
    }

    Vector3 CalculatePoint(Transform t) {
        var pos = t.position + offset;
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        return rotation * new Vector3(0, 0, -distance) + pos;
    }

    public void SetPosition(Vector3 position) {
        nextPosition = position;
    }

    public void SetRotation(Quaternion rotation) {
        nextRotation = rotation;
    }

    public void SetTargetAngles(Vector3 angles) {
        x = angles.y;
        y = angles.x;
    }

    public void SetY(float y) {
        this.x = y;
    }

    /*public void ZoomTo(Transform newTarget, float duration = 1f){
        if (zoomSequence != null) {
            zoomSequence.Cancel();
        }
        zoomSequence = new Sequence (ZoomToSequence (newTarget, duration));
    }

    IEnumerator ZoomToSequence(Transform newTarget, float duration){
        Unlock ();
        if (duration > 0) {
            for (float t = 0; t < 1f; t += Time.deltaTime / duration) {
                var pos = CalculatePoint (newTarget);
                transform.position = Vector3.Lerp (transform.position, pos, t * t);

                yield return null;
            }
        }
        transform.position = CalculatePoint (newTarget);
        player = newTarget;
        lockedOn = true;
    }

    public Sequence ZoomAndRotate(Transform cameraTarget, float duration = 1f){
        if (!cameraTarget) {
            throw new UnityException("Camera target cannot be null!");
        }

        if (zoomSequence != null) {
            zoomSequence.Cancel();
        }
        zoomSequence = new Sequence (ZoomAndRotateSequence (cameraTarget, duration));
        return zoomSequence;
    }

    IEnumerator ZoomAndRotateSequence(Transform newTarget, float duration){
        Debug.Log ("zooming to " + newTarget);
        Unlock ();
        if (duration > 0) {
            var originalRot = transform.rotation;
            var originalPos = transform.position;
            for (float t = 0; t < 1f; t += Time.deltaTime / duration) {
                transform.position = Vector3.Lerp(originalPos, newTarget.position, t);
                transform.rotation = Quaternion.Lerp(originalRot, newTarget.rotation, t);
				
                yield return null;
            }
        }
        transform.position = newTarget.position;
        transform.rotation = newTarget.rotation;
    }

    /// <summary>
    /// Tracks the target.
    /// </summary>
    /// <param name="target">Target.</param>
    /// <param name="rate">Rotation rate in degrees per second.</param>
    public void TrackTarget(Transform target, float rate = 30f){
        if (zoomSequence != null) {
            zoomSequence.Cancel();
        }

        zoomSequence = new Sequence (TrackTargetSequence(target, rate));
        zoomSequence.OnDestroy += (object sender, System.EventArgs e) => UnlockControl(this);
    }

    IEnumerator TrackTargetSequence(Transform target, float rate){
        Unlock ();
        LockControl (this);

        while (true) {
            var targetForward = target.position - transform.position;
            var angle = Vector3.Angle(targetForward, transform.forward);
            var targetRotation = Quaternion.LookRotation(targetForward);
            var thisRate = Mathf.Clamp(angle / rate, 0.5f, rate);
            thisRate += 0.1f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, thisRate * Time.deltaTime);

            yield return null;
        }
    }*/

}
