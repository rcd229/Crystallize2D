using UnityEngine;
using System.Collections;

public interface IInitializable<in T> {

    void Initialize(T args1);

}
