using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PlayerFloatingNameHolder : MonoBehaviour, IFloatingNameHolder {

    void Start() {
        FloatingNameUI.GetInstance().SetName(transform, PlayerData.Instance.PersonalData.Name);
    }

}
