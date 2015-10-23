using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerCustomizer : MonoBehaviour {

    public InputField playerName;
    public Text nameText;
    public Toggle maleToggle;
    public Toggle femaleToggle;
    public Transform target;
	public Text noticeText;
    public Button confirmButton;

    AppearancePlayerData appearance = new AppearancePlayerData();
    GameObject instance;
    string orignalName;
	//Button confirmButton;
	public Button cancelButton;

	IEnumerator Start() {
        //if (!GameSettings.Instance.Local) {
        //    while (!Network.isClient) {
        //        yield return null;
        //    }
        //    confirmButton = gameObject.transform.Find("Button").gameObject.GetComponent<Button>();
        //}

        yield return null;

        nameText.text = PlayerData.Instance.PersonalData.Name;

        var n = RandomNameGenerator.GetRandomMaleName();
        Debug.Log(n);
		noticeText.enabled = false;

        appearance.SkinColor = 1;
        if (UnityEngine.Random.value > 0.5f) {
            maleToggle.isOn = true;
            playerName.text = RandomNameGenerator.GetRandomMaleName();
        } else {
            maleToggle.isOn = false;
            playerName.text = RandomNameGenerator.GetRandomFemaleName();
        }
        orignalName = playerName.text;

        appearance.TopMaterial = 3;
        appearance.BottomMaterial = 1;
        RegenerateAppearance();
    }

    void Update() {
        var caret = playerName.transform.Find(playerName.name + " Input Caret");
        if (caret) {
            caret.GetComponent<RectTransform>().pivot = new Vector2(0, 0.25f);
        }
    }

    public void SetMaleGender(bool isMale) {
        if(isMale && femaleToggle.isOn){
            femaleToggle.isOn = false;
        } else if (!isMale && !femaleToggle.isOn) {
            femaleToggle.isOn = true;
        }
        SetGender(isMale);

        if (playerName.text == orignalName) {
            if (isMale) {
                playerName.text = RandomNameGenerator.GetRandomMaleName();
            } else {
                playerName.text = RandomNameGenerator.GetRandomFemaleName();
            }
            orignalName = playerName.text;
        }
    }

    public void SetFemaleGender(bool isFemale) {
        if(isFemale && maleToggle.isOn){
            maleToggle.isOn = false;
        } else if (!isFemale && !maleToggle.isOn) {
            maleToggle.isOn = true;
        }
        SetGender(!isFemale);

        if (playerName.text == orignalName) {
            if (!isFemale) {
                playerName.text = RandomNameGenerator.GetRandomMaleName();
            } else {
                playerName.text = RandomNameGenerator.GetRandomFemaleName();
            }
            orignalName = playerName.text;
        }
    }

    void SetGender(bool isMale) {
        int newGender = 1;
        if (isMale) {
            newGender = 0;
        }

        if (appearance.Gender != newGender) {
            appearance.Gender = newGender;
            RegenerateAppearance();
        }
    }

    public void SetHairType(int type) {
        appearance.HairType = type;
        RegenerateAppearance();
    }

    public void SetHairColor(int type) {
        appearance.HairColor = type;
        RegenerateAppearance();
    }

    public void SetEyeColor(int type) {
        appearance.EyeColor = type;
        RegenerateAppearance();
    }

    public void SetSkinColor(int type) {
        appearance.SkinColor = type;
        RegenerateAppearance();
    }

	public void CancelPlayerDesign(){
	    Application.LoadLevel("Title");
	}

    public void SaveToPlayerData() {
		//disable the button
        //PlayerData.Initialize(new PlayerData());
        //if (GameSettings.Instance.Local) {
            //PlayerData.Instance.PersonalData.SetName(playerName.text); //playerName.text;
            PlayerData.Instance.Appearance = appearance;
            PlayerDataLoader.Save();
            Application.LoadLevel("Start");
        //} else {
        //    cancelButton.enabled = false;
        //    confirmButton.enabled = false;
        //    string name = playerName.text;
        //    //PlayerData.Instance.PersonalData.SetName(name);
        //    PlayerData.Instance.Appearance = appearance;
        //    CrystallizeNetwork.Client.RequestNameFromServer(name, HandleNameResponse);
        //    //RPCFunctions.Instance.RequestNameFromServer(name);
        //}
    }

	void HandleNameResponse (bool result)
	{
		if(result){
	        PlayerDataLoader.Save();
            //RPCFunctions.Instance.SavePlayerDataToServer();
	        Application.LoadLevel("Start");
		}else{
			//TODO show prompt to re-enter name
			noticeText.enabled = true;
			noticeText.text = "Invalid name. Please Select a new name";
			cancelButton.enabled = true;
			confirmButton.enabled = true;
		}

	}

    public void Randomize() {
        //appearance = AppearanceLibrary.GetRandomAppearance();
    }

    void RegenerateAppearance() {
        if (instance) {
            Destroy(instance);
        }

        instance = AppearanceLibrary.CreateObject(appearance.GetResourceData());
        instance.transform.parent = target;
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
    }

}