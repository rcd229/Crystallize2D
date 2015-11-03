using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ResourcePathAttribute : System.Attribute {

    public string ResourcePath { get; private set; }

    public ResourcePathAttribute(string resourcePath) {
        this.ResourcePath = resourcePath;
    }

}
