using UnityEngine;
using System;
using System.Collections;

[ResourcePath("Camera/ConversationCameraController")]
public class BaseConversationCameraController : MonoBehaviour, ITemporaryUI<ConversationArgs>  {

    public Transform cameraTransform;

    public event EventHandler<EventArgs<object>> Complete;

    ConversationArgs args;
    float height = 1.5f;

    public void Initialize(ConversationArgs args) {
        this.args = args;
    }

    public void Close() {
        Destroy(gameObject);
    }

	// Use this for initialization
	IEnumerator Start () {
        var actor = args.Target;
        var pgo = PlayerManager.Instance.PlayerGameObject.GetComponent<Rigidbody>();

        yield return null;

        pgo.position = actor.transform.position + actor.transform.forward * actor.GetComponent<DialogueActor>().distanceToPlayer;
        pgo.transform.forward = -actor.transform.forward;
        pgo.velocity = Vector3.zero;


        transform.SetParent(PlayerManager.Instance.PlayerGameObject.transform);
        transform.localPosition = Vector3.zero;
        OmniscientCamera.main.Suspend();

        height = args.Target.transform.GetHead().position.y; 
	}

    void OnDestroy()
    {
        if (Camera.main) {
            OmniscientCamera.main.Resume();
        }
    }
	
	// Update is called once per frame
	void Update () {
        var forward = args.Target.transform.position - transform.position;
        forward.y = 0;
        transform.forward = forward;
        OmniscientCamera.main.SetPosition( cameraTransform.position);
        var rot = Quaternion.LookRotation(GetHeadPosition() - cameraTransform.position);
        OmniscientCamera.main.SetRotation(rot);
	}

    Vector3 GetHeadPosition() {
        return args.Target.transform.position + Vector3.up * height;
    }

}
