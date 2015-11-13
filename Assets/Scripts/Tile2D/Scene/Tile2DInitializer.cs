using UnityEngine;
using System.Collections;

public class Tile2DInitializer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Tile2DSceneResourceManager.Instance.LoadLevel(GameLevel2D.DefaultLevel);
	}
	
}
