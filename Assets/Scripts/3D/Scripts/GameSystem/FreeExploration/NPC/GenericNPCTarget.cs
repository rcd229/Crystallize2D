using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class GenericNPCTarget : UniqueIDData<GenericNPCTarget> {
    public static readonly GenericNPCTarget Person = new GenericNPCTarget("Any person", new Guid("524c4ac85f9e40c09dfb875ea28e4009"));

    public GenericNPCTarget(string name, Guid guid) : base(name, guid) { }
}
