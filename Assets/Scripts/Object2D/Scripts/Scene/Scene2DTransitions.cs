using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Scene2DTransitions : MonoBehaviour
{
    static Scene2DTransitions _instance;
    public static Scene2DTransitions Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = new GameObject("Scene2DTransitions").AddComponent<Scene2DTransitions>();
            }
            return _instance;
        }
    }
    
    public void TransitionToScene(SceneChangeTrigger2D trigger)
    {
        StartCoroutine(TransitionToScene_coroutine(trigger));
    }

    public IEnumerator TransitionToScene_coroutine(SceneChangeTrigger2D trigger){

        var player = GameObject.FindGameObjectWithTag("Player");
        var objs = Object2DLoader.LoadAll(trigger.Scene);
        var target = (from o in objs
                      where o.Guid == trigger.Target
                      select o)
                     .FirstOrDefault();

        //disable character controls
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<PlayerMovement2D>().enabled = false;
        player.GetComponent<Collider2D>().enabled = false;

        //start fade out
        BlackScreenUI.GetInstance().Initialize(0.5f,0.1f,0.5f);

        //move player into door
        for (float t = 0; t < 1f; t+=Time.deltaTime*2)
        {
            player.transform.position += Vector3.up * Time.deltaTime;
            yield return null;
        }
        player.GetComponent<Collider2D>().enabled = true;
        if (target != null)
        {
            player.transform.position = (Vector2)target.Position;
            //player.
        }

        //start blackout
        GameLevel2DSceneResourceManager.LoadLevel(GameLevel2D.GetGameLevel(trigger.Scene));
        player.GetComponent<PlayerMovement2D>().enabled = true;
        player.GetComponent<Collider2D>().enabled = true;
    }

}
