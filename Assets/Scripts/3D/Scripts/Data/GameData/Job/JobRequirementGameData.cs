using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public static class JobRequirementGameDataExtensions {
    public static bool IsFulfilled<T>(this IEnumerable<T> reqs) where T : JobRequirementGameData {
        foreach (var r in reqs) {
            if (!r.IsFulfilled()) {
                return false;
            }
        }
        return true;
    }
}

[XmlInclude(typeof(PhraseJobRequirementGameData))]
[XmlInclude(typeof(PreviousJobRequirementGameData))]
public abstract class JobRequirementGameData {

    public abstract bool IsFulfilled();

}