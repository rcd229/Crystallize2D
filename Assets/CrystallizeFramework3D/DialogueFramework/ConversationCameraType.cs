using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConversationCameraType {

    static ConversationCameraType() {
        FrameworkInitializer.Get();
    }

    public static ConversationCameraType Default { get; set; }

    public readonly UIFactoryRef<ConversationArgs, object> CameraController = new UIFactoryRef<ConversationArgs, object>();
}
