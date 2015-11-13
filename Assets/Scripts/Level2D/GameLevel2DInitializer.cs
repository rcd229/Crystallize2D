using UnityEngine;
using System.Collections;

public class GameLevel2DInitializer : MonoBehaviour {

	void Start () {
        GameLevel2DSceneResourceManager.LoadLevel(GameLevel2D.DefaultLevel);
	}
	
}
