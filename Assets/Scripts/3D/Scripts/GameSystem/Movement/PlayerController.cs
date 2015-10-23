using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public static void LockMovement(object lockObject){
        PlayerManager.LockMovement(lockObject);
	}

	public static void UnlockMovement(object lockObject){
        PlayerManager.UnlockMovement(lockObject);
	}

	public float speed = 5f;
	public Transform target = null;

	HashSet<object> locks = new HashSet<object>();

    Quaternion cameraRotation;
    float xForce;
    float yForce;

	public bool MovementLocked {
		get {
            return PlayerManager.MovementLocked;
		}
	}

	// Use this for initialization
	void Start () {
		if (!target) {
			var p = PlayerManager.Instance.PlayerGameObject;

			if(!p){
				return;
			}

			target = p.transform;
		}
	}

    void Update() {
        xForce = Input.GetAxis("Horizontal");
        yForce = Input.GetAxis("Vertical");
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if (PlayerManager.MovementLocked) {
			return;
		}

		if (!target) {
			return;
		}

        var rb = target.GetComponent<Rigidbody>();
        var tar = Vector3.zero;
		//target.GetComponent<Rigidbody>().velocity = Vector3.zero;
		if (Input.GetMouseButton (0)) {
            if (!UISystem.MouseOverUI()) {
				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				var hit = new RaycastHit();
				if(Physics.Raycast(ray, out hit)){
					var dir = hit.point - target.transform.position;
					dir.y = 0;
					target.GetComponent<Rigidbody>().velocity = dir.normalized * speed;
				}
			} 
		} else {
            var camForward = Camera.main.transform.forward;
            camForward.y = 0;
            cameraRotation = Quaternion.LookRotation(camForward, Vector3.up);

            var inputDirection = new Vector3(xForce, 0, yForce);

            tar = cameraRotation * inputDirection * speed;
		}
        var vy = rb.velocity.y;
        rb.velocity = Vector3.MoveTowards(rb.velocity, tar, speed * 10f * Time.fixedDeltaTime);
        rb.velocity = new Vector3(rb.velocity.x, vy, rb.velocity.z);
	}
	
}
