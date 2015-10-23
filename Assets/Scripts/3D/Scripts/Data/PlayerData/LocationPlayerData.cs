using UnityEngine;
using System.Collections;

public class LocationPlayerData {

    public const int DefaultAreaID = -1;

    public int AreaID { get; set; }

    public LocationPlayerData() {
        AreaID = DefaultAreaID;
    }

}
