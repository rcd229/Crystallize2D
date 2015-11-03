using UnityEngine;
using System.Collections;

public class PlayerMovement2D : MonoBehaviour {

    public float speed = 6f;
    bool FacingDown = true;
    bool FacingUp = false;
    bool FacingLeft = false;
    bool FacingRight = false;
    Animator anim;
    Rigidbody2D player;

    // Use this for initialization
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float movev = Input.GetAxis("Vertical");
        float moveh = Input.GetAxis("Horizontal");
        player.velocity = new Vector2(moveh * speed, movev * speed);
        anim.SetFloat("Speed", player.velocity.magnitude);

        var normalizedir = player.velocity.normalized;
        if (player.velocity.magnitude > 0.5) {
            if (Mathf.Abs(normalizedir.x) > Mathf.Abs(normalizedir.y))
            {
                //moving right
                if (normalizedir.x > 0)
                {
                    anim.Play("Right");
                }
                else
                {
                    anim.Play("Left");
                }
            }
            else
            {
                if (normalizedir.y > 0)
                {
                    anim.Play("Up");
                }
                else
                {
                    anim.Play("Down");
                }
            }
        } 
    }
}
