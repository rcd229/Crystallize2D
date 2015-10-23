using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ProcessPlayerController : MonoBehaviour, ITemporaryUI<object, object>, IHasCanvasBranch, IDebugMethods {

    public static ProcessPlayerController GetInstance() {
        return new GameObject("PlayerController").AddComponent<ProcessPlayerController>();
    }

    public event EventHandler<EventArgs<object>> Complete;

    public CanvasBranch Branch { get { return CanvasBranch.Root; } }

    Rigidbody player;
    Quaternion cameraRotation;
    float speed = 5f;
	float acceleration = 50f;
    float xForce;
    float yForce;

    Vector3 lastLoggedPosition = Vector3.zero;

    public void Initialize(object param1) {
        player = PlayerManager.Instance.PlayerGameObject.GetComponent<Rigidbody>();
    }

    public void Close() {
        Destroy(gameObject);
    }

    void Update() {
        xForce = Input.GetAxis("Horizontal");
        yForce = Input.GetAxis("Vertical");
    }

    Vector3 lastPos; 
    void FixedUpdate() {
        var force = Vector3.zero;

        if (!PlayerManager.MovementLocked) {
            var camForward = Camera.main.transform.forward;
            camForward.y = 0;
            cameraRotation = Quaternion.LookRotation(camForward, Vector3.up);

            var inputDirection = new Vector3(xForce, 0, yForce);

            force = cameraRotation * inputDirection * speed;
        }

        var vy = player.velocity.y;
        player.velocity = Vector3.MoveTowards(player.velocity, force, acceleration * Time.fixedDeltaTime);
        player.velocity = new Vector3(player.velocity.x, vy, player.velocity.z);

        if ((lastLoggedPosition - player.position).sqrMagnitude > 1) {
            lastLoggedPosition = player.position;
            DataLogger.LogTimestampedData("Position", lastLoggedPosition.ToString());
        }
    }

    #region DEBUG
    public IEnumerable<NamedMethod> GetMethods() {
        return NamedMethod.Collection(HighSpeed);
    }

    public string HighSpeed(string input) {
        float newSpeed;
        if (!float.TryParse(input, out newSpeed)) {
            newSpeed = 10f;
        }
        speed = newSpeed;
        return "Set speed to " + newSpeed;
    }
    #endregion
}
