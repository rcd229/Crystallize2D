using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UIDraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {
	#region IDropHandler implementation

	public void OnDrop (PointerEventData eventData)
	{
		//throw new System.NotImplementedException ();
	}

	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		//throw new System.NotImplementedException ();
	}

	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		GetComponent<RectTransform> ().position = eventData.position;
	}

	#endregion

	#region IBeginDragHandler implementation
	public void OnBeginDrag (PointerEventData eventData)
	{
		//throw new System.NotImplementedException ();
	}
	#endregion

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
