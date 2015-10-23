using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ConversationCameraType {

    public static readonly ConversationCameraType OverShoulder = Get<BaseConversationCameraController>();
    public static readonly ConversationCameraType FirstPerson = Get(FirstPersonConversationCameraController.GetInstance);

    public static ConversationCameraType Default { get { return FirstPerson; } }

    static ConversationCameraType Get<T>() where T : Component, ITemporaryUI<ConversationArgs, object> {
        var cct = new ConversationCameraType();
        cct.CameraController.Set<T>();
        return cct;
    }

    static ConversationCameraType Get(Func<ITemporaryUI<ConversationArgs, object>> getter) {
        var cct = new ConversationCameraType();
        cct.CameraController.Set(getter);
        return cct;
    }

    public readonly UIFactoryRef<ConversationArgs, object> CameraController = new UIFactoryRef<ConversationArgs,object>();

}
