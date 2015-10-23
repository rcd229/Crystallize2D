using UnityEngine;
using System.Collections;

public class AvatarConstructor : MonoBehaviour {

    bool male = false;
    GameObject instance;

	// Use this for initialization
	void Start () {
        UpdateAvatar();
	}
	
	// Update is called once per frame
    //void Update () {
    //    if (Input.GetKeyDown(KeyCode.Y))
    //    {
    //        male = !male;
    //        UpdateAvatar();
    //    }
    //}

    public void UpdateAvatar()
    {
        if (instance)
        {
            Destroy(instance);
        }

        instance = AppearanceLibrary.CreateObject(PlayerData.Instance.Appearance.GetResourceData()); //AvatarComponentLibrary.Instance.GetAvatarInstance(male);
        instance.transform.SetParent(transform);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
        if (male) {
            instance.transform.localScale = 1.1f * Vector3.one;
        }
    }

    public void SetAvatar(AppearancePlayerData appearance) {
        if (instance) {
            Destroy(instance);
        }

        instance = AppearanceLibrary.CreateObject(appearance.GetResourceData()); //AvatarComponentLibrary.Instance.GetAvatarInstance(male);
        instance.transform.SetParent(transform);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
        if (male) {
            instance.transform.localScale = 1.1f * Vector3.one;
        }
    }

}
