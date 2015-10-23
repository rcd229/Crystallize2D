using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

public class TilePlacer : MonoBehaviour {

	public List<GameObject> tiles;
    private GameObject tile;
    private int tileIndex;
    private List<GameObject> tileList;
    private Timer doubleClickTimer;
    private bool isFirstClick;
    private bool isDoubleClick;
    private int milliseconds;


    // Use this for initialization
    void Start () {
        tileIndex = 0;
        tile = tiles[tileIndex];
        tileList = new List<GameObject>();
        doubleClickTimer = new Timer();
        isFirstClick = true;
        isDoubleClick = false;
        milliseconds = 0;
        doubleClickTimer.Interval = 100;
        //doubleClickTimer.Tick += new EventHandler(doubleClickTimer_Tick);
    }

    private void doubleClickTimer_Tick(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update () {
        tileCreateDestroy();
	}

    //place tile onscreen
    void tileCreateDestroy()
    {
        //left click creates or destroys tiles
        if (Input.GetMouseButtonDown(0))
        {
            if (isFirstClick)
            {
                isFirstClick = false;
                doubleClickTimer.Start();
            }

            var mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos.z = 0;
            mousePos.x = Mathf.RoundToInt(mousePos.x);
            mousePos.y = Mathf.RoundToInt(mousePos.y);

            /*if (mouseClicks > 1)
            {
                doubleClickTimer += Time.fixedDeltaTime;

                //is a double click
                if (doubleClickTimer - mouseTimerLimit > 0)
                {
                    doubleClickTimer = 0;
                    mouseClicks = 0;

                    //double click removes tile
                    var removed = tileList.Find(r => r.transform.position == mousePos);
                    Destroy(removed);
                    tileList.Remove(removed);
                }
            }

            else
            {
                Debug.Log("single click");
                //single click places tile
                var tileInstance = Instantiate<GameObject>(tile);

                tileInstance.transform.position = mousePos;
                tileList.Add(tileInstance);

                if (doubleClickTimer > mouseTimerLimit)
                {
                    mouseClicks = 0;
                    doubleClickTimer = 0;
                }
            }
        }

        //right clicking once cycles through available tiles
        else if (Input.GetMouseButtonDown(1))
        {
            tileIndex++;
            tile = tiles[tileIndex];
        }

        else
        {
            return;
        }*/
        }
    }

    void ClearAllTiles()
    {
        foreach (var t in tileList)
        {
            Destroy(t);
        }
        tileList.Clear();
    }
}
