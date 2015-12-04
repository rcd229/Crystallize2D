using UnityEngine;
using System.Collections;

public class PlayerMovement2D : MonoBehaviour {

    float snap = 0.5f;

    public float speed = 6f;
    //bool FacingDown = true;
    //bool FacingUp = false;
    //bool FacingLeft = false;
    //bool FacingRight = false;
    Animator anim;
    Rigidbody2D player;

    Vector2? target;

    // Use this for initialization
    void Start() {
        player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        float movev = Input.GetAxis("Vertical");
        float moveh = Input.GetAxis("Horizontal");
        var move = new Vector2(moveh, movev).normalized;
        player.velocity = new Vector2(move.x * speed, move.y * speed);

        if (move.sqrMagnitude > 0) {
            float x = 0, y = 0;
            if (move.x > 0) {
                x = Mathf.Floor((player.transform.position.x + snap) / snap) * snap;
            } else if (move.x < 0) {
                x = Mathf.Ceil((player.transform.position.x - snap) / snap) * snap;
            } else {
                x = Mathf.RoundToInt(player.transform.position.x / snap) * snap;
            }

            if (move.y > 0) {
                y = Mathf.Floor((player.transform.position.y + snap) / snap) * snap;
            } else if (move.y < 0) {
                y = Mathf.Ceil((player.transform.position.y - snap) / snap) * snap;
            } else {
                y = Mathf.RoundToInt(player.transform.position.y / snap) * snap;
            }

            target = new Vector2(x, y);
        } else if (target.HasValue) {
            player.MovePosition(Vector2.MoveTowards(player.position, target.Value, speed * Time.deltaTime));
            if (player.position == target.Value) {
                target = null;
            }
        }


        anim.SetFloat("Speed", player.velocity.magnitude);
        var normalizedir = player.velocity.normalized;
        if (player.velocity.magnitude > 0.5f) {
            if (Mathf.Abs(normalizedir.x) > Mathf.Abs(normalizedir.y)) {
                //moving right
                if (normalizedir.x > 0) {
                    anim.Play("Right");
                } else {
                    anim.Play("Left");
                }
            } else {
                if (normalizedir.y > 0) {
                    anim.Play("Up");
                } else {
                    anim.Play("Down");
                }
            }
        }
    }

    public void setDirection(string direction) {
        anim.Play(direction);
    }
}
