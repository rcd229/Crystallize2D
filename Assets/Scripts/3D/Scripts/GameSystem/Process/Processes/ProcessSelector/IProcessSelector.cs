using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public interface IProcessSelector<I> {
    ProcessFactory<I> SelectProcess(ProcessFactory<I> defaultFactory, I inputArgs);
}
