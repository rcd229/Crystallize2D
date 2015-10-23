using UnityEngine;
using System.Collections;

public class AvatarComponentLibrary : ScriptableObject {

    static AvatarComponentLibrary _instance;

    public static AvatarComponentLibrary Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<AvatarComponentLibrary>("AvatarLibrary"); 
            }
            return _instance;
        }
    }

    public GameObject GetAvatarInstance(bool male)
    {
        GameObject baseModel = null;
        if (male)
        {
            baseModel = Instantiate<GameObject>(Resources.Load<GameObject>("Avatar/MaleBase"));
        }
        else
        {
            baseModel = Instantiate<GameObject>(Resources.Load<GameObject>("Avatar/FemaleBase"));
        }
        return baseModel;
    }

}
