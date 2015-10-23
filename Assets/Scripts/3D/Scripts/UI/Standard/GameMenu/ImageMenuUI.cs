using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class ImageMenuUI<T> : ConfirmMenuUI<T> 

	where T : ImageMenuItem

{
	#region implemented abstract members of SelectionMenuUI
	protected override void InitializeButton (GameObject obj, T item)
	{
		Image image = obj.GetComponent<Image> ();
		image.sprite = item.Image;
		obj.AddComponent<DataContainer>().Store(item);
	}
	#endregion
		
}
