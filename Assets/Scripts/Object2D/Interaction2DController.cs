using UnityEngine;
using System;
using System.Collections.Generic;
using Util;
using System.Linq;

public class Interaction2DController : MonoBehaviour
{
    float camRayLength = 100f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, fwd);

            //check for IInteractable interface
            //if so, cast it as that and call interact function

            if (hit.transform)
            {
                IInteractable2D[] intrfaces = hit.rigidbody.gameObject.GetInterfacesInChildren<IInteractable2D>();
                foreach (IInteractable2D i in intrfaces) {
                    i.Interact();
                }
            }

        }
    }
}
