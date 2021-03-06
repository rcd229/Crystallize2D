﻿using UnityEngine;
using System;
using System.Collections.Generic;
using Util;
using System.Linq;

public class Interaction2DController : MonoBehaviour
{
    float camRayLength = 100f;

    void Awake() {
        MainCanvas.main.Add(transform);
    }

    void Start() {
        CollisionMap2D.Instance.SetVisualsEnabled(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !TextDisplayUI.open)
        {
            //Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, fwd);
            Debug.Log("casting ray:" + hit.transform);
            //check for IInteractable interface
            //if so, cast it as that and call interact function

            if (hit.transform)
            {
                if (hit.transform.parent)
                {
                    IInteractable2D[] intrfaces = hit.transform.parent.gameObject.GetInterfacesInChildren<IInteractable2D>();
                    foreach (IInteractable2D i in intrfaces)
                    {
                        i.Interact();
                    }
                }
                else
                {
                    IInteractable2D[] intrfaces = hit.transform.gameObject.GetInterfacesInChildren<IInteractable2D>();
                    foreach (IInteractable2D i in intrfaces)
                    {
                        i.Interact();
                    }
                }
                
            }

        }
    }
}
