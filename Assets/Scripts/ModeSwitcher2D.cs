using System;
using UnityEngine;

public class ModeSwitcher2D : MonoBehaviour
{
    public GameObject[] prefabs;
    GameObject currentPrefab;
    int index;

	public ModeSwitcher2D()
	{
        index = 0;
        currentPrefab = Instantiate<GameObject>(prefabs[index]);
	}

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Destroy(currentPrefab);
            index++;
            currentPrefab = Instantiate<GameObject>(prefabs[index]);
        }
    }
}
