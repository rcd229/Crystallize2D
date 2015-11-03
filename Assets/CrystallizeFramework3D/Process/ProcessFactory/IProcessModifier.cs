using UnityEngine;
using System.Collections;

public interface IProcessModifier<I> {

    void ModifyProcess(IProcess<I> process);

}