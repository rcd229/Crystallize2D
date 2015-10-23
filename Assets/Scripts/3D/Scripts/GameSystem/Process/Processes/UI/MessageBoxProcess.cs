using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class MessageBoxProcess : UIProcess<string, object> {

    public MessageBoxProcess() : base(UILibrary.MessageBox) { }

}
