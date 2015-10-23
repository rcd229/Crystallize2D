using UnityEngine;
using System.Collections;

public class ConversationProcessPart {

    static IProcess cameraProcess;

    protected void StartCamera(ConversationArgs obj) {
        if (cameraProcess != null) {
            cameraProcess.ForceExit();
        }

        cameraProcess = ConversationSequence.RequestConversationCamera.Get(obj, Callback, null);
    }

    protected bool StopCamera() {
        if (cameraProcess != null) {
            cameraProcess.ForceExit();
            cameraProcess = null;
            return true;
        }
        return false;
    }

    void Callback(object sender, object output) { }

}