using UnityEngine;
using System.Collections;

public class ItemGivenEventArgs : System.EventArgs {

    public int GlobalID { get; set; }
    public int ItemID { get; set; }

    public ItemGivenEventArgs(int gid, int iid) {
        this.GlobalID = gid;
        this.ItemID = iid;
    }

}
