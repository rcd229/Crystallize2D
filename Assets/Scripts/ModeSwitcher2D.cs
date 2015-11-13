using System;
using UnityEngine;

public class ModeSwitcher2D : MonoBehaviour
{
    public GameObject[] prefabs;
    GameObject currentPrefab;
    int index;

	public void Start()
	{
        index = 0;
        currentPrefab = Instantiate<GameObject>(prefabs[index]);
	}

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Destroy(currentPrefab);
            index = (index + 1) % prefabs.Length;
            currentPrefab = Instantiate<GameObject>(prefabs[index]);
        }
    }
}
