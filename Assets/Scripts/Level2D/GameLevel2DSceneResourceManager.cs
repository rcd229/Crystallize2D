using UnityEngine;
using System.Collections;

public class GameLevel2DSceneResourceManager : MonoBehaviour {

    public static void LoadLevel(GameLevel2D level) {
        Tile2DSceneResourceManager.Instance.LoadLevel(level);
        Object2DSceneResourceManager.Instance.LoadLevel(level);
    }

}
