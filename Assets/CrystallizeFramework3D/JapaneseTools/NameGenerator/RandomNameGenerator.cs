using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomNameGenerator : MonoBehaviour {

	static string[] maleNames;
    static string[] femaleNames;
    static string[] commonMaleNames;
    static string[] commonFemaleNames;

	public static string[] MaleNames {
		get {
			if(maleNames == null){
				var text = Resources.Load<TextAsset>("MaleNames");
				maleNames = text.text.Split('\n');
			}
			return maleNames;
		}
	}

    public static string[] FemaleNames {
        get {
            if (femaleNames == null) {
                var text = Resources.Load<TextAsset>("FemaleNames");
                femaleNames = text.text.Split('\n');
            }
            return femaleNames;
        }
    }

    public static string[] CommonMaleNames {
        get {
            if (commonMaleNames == null) {
                var text = Resources.Load<TextAsset>("CommonMaleNames");
                commonMaleNames = text.text.Split('\n');
            }
            return commonMaleNames;
        }
    }

    public static string[] CommonFemaleNames {
        get {
            if (commonFemaleNames == null) {
                var text = Resources.Load<TextAsset>("CommonFemaleNames");
                commonFemaleNames = text.text.Split('\n');
            }
            return commonFemaleNames;
        }
    }

    public static string GetRandomName(bool isMale) {
        if (isMale) {
            return GetRandomMaleName();
        } else {
            return GetRandomFemaleName();
        }
    }

	public static string GetRandomMaleName(){
		string randomName = "";
		while (randomName == "") {
            while (randomName.Trim().Length < 2) {
                randomName = MaleNames[Random.Range(0, MaleNames.Length)];
            }
		}
		return randomName.Trim();
	}

    public static string GetRandomFemaleName() {
        string randomName = "";
        while (randomName == "") {
            while (randomName.Trim().Length < 2) {
                randomName = FemaleNames[Random.Range(0, FemaleNames.Length)];
            }
        }
        return randomName.Trim();
    }

    public static string GetRandomCommonName(bool isMale) {
        if (isMale) {
            return GetRandomCommonMaleName();
        } else {
            return GetRandomCommonFemaleName();
        }
    }

    public static string GetRandomCommonMaleName() {
        string randomName = "";
        while (randomName == "") {
            while (randomName.Trim().Length < 2) {
                randomName = CommonMaleNames[Random.Range(0, CommonMaleNames.Length)];
            }
        }
        return randomName.Trim();
    }

    public static string GetRandomCommonFemaleName() {
        string randomName = "";
        while (randomName == "") {
            while (randomName.Trim().Length < 2) {
                randomName = CommonFemaleNames[Random.Range(0, CommonFemaleNames.Length)];
            }
        }
        return randomName.Trim();
    }

}
