using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PlayerController2D : MonoBehaviour {

    public float speed = 5f;
    public float acceleration = 20f;
    
    Vector2 velocity = Vector2.zero;
    Rigidbody2D mover;

    void Start() {
        mover = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (PlayerManager.MovementLocked) {
            return;
        }

        var xForce = Input.GetAxis("Horizontal");
        var yForce = Input.GetAxis("Vertical");
        var force = new Vector2(xForce, yForce).normalized;
        velocity = Vector2.MoveTowards(velocity, force * speed, acceleration * Time.deltaTime);
        mover.velocity = velocity;
    }

}
