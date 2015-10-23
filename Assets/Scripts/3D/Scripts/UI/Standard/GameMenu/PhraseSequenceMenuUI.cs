using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ResourcePathAttribute("UI/PhraseSequenceMenu")]
public class PhraseSequenceMenuUI : ConfirmMenuUI<PhraseSequence> 

{

	#region implemented abstract members of SelectionMenuUI

	protected override void InitializeButton (GameObject obj, PhraseSequence item)
	{
		obj.GetComponentInChildren<Text>().text = item.GetText();
		obj.AddComponent<DataContainer>().Store(item);
	}
	

	#endregion


	
}
