using UnityEngine;
using System;
using System.Collections;

public class JobRefContainer : MonoBehaviour {

    public JobID jobID;
    public IJobRef Data {
        get {
            return new IDJobRef(jobID);
        }
    }
}
