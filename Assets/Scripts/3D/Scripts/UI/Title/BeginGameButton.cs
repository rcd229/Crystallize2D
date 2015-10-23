using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class BeginGameButton : MonoBehaviour, IPointerClickHandler {

	public string level = "Tutorial_Level01";

	public void OnPointerClick (PointerEventData eventData)
	{
		Application.LoadLevel (level);
	}

}
