using UnityEngine;
using System;
using System.Collections;
using System.Xml.Serialization;

public class ProcessTypeRef {

    [XmlIgnore]
    Type _processType;
    [XmlIgnore]
    public Type ProcessType {
        get { return _processType; }
        set {
            _processType = value;
            ProcessTypeName = value.AssemblyQualifiedName;
        }
    }

    public string ProcessTypeName {
        get {
            if (_processType != null) {
                return _processType.AssemblyQualifiedName;
            }
            return typeof(TempProcess<JobTaskRef, object>).AssemblyQualifiedName;
        }
        set { _processType = Type.GetType(value); }
    }

    public ProcessTypeRef() {
        _processType = typeof(TempProcess<JobTaskRef, object>);
    }

    public ProcessTypeRef(Type t){
        _processType = t;
    }

}