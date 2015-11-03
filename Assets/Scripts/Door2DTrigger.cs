using UnityEngine;
using System.Collections;

public class Door2DTrigger : MonoBehaviour
{

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            var player = other.gameObject;
            if (player.GetComponent<Rigidbody2D>().velocity.y > 1f)
            {
                Debug.Log("Player has hit a door");
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("Player is in door");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Player has left door");
    }
}
