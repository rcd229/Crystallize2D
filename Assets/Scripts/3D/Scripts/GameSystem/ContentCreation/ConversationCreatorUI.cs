using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[ResourcePathAttribute("UI/ContentCreation/ConversationCreationUI")]
public class ConversationCreatorUI : UIMonoBehaviour, ITemporaryUI<ConversationCreationArg, object> {

	public event EventHandler<EventArgs<object>> Complete;
	ConversationCreationArg log;

	const string alertString = "Please correct the highlighted field";

	public Text title;
	public Text description;
	public Text alertText;
	public UIButton addLine;
	public UIButton submit;

	ConversationCreationLine linePrefab;
	GameObject lineParent;


	#region IInitializable implementation
	public void Initialize (ConversationCreationArg args1)
	{
		log = args1.ArgCopy();
		title.text = log.Data.title;
		description.text = log.Data.description;
		alertText.gameObject.SetActive(false);
		linePrefab = Resources.Load<ConversationCreationLine>(ConversationCreationLine.ResourcePath);
		lineParent = gameObject.transform.Find("Lines").gameObject;
		if(log.Data.dialogue == null){
			log.Data.dialogue = new SequentialConveration();
		}
		int index = 0;
		foreach(var words in log.Data.dialogue.Dialogue){
			AddLine(words, log.Data.dialogue.Translations[index]);
			index++;
		}
		addLine.OnClicked += HandleOnClicked;
		submit.OnClicked += OnSubmit;
	}

	void OnSubmit (object sender, EventArgs<UnityEngine.EventSystems.PointerEventData> e)
	{
		alertText.gameObject.SetActive(false);
		var lineArray = lineParent.GetComponentsInChildren<ConversationCreationLine>();
		List<LineCreationData> data = lineArray.Select(s => s.CompileAndValidate()).ToList();
		if(data.Contains(null)){
			alertText.gameObject.SetActive(true);
			alertText.text = alertString;
		}else{
			SequentialConveration conv = new SequentialConveration();
			conv.Dialogue = data.Select(s => s.words).ToList();
			conv.Translations = data.Select(s => s.translation).ToList();
			bool changed = ConversationDatabaseController.Instance.modify(log.UserInfo, log.Data.cid, log.Data.title, log.Data.description, conv);
			Close ();
		}

	}

	void HandleOnClicked (object sender, EventArgs<UnityEngine.EventSystems.PointerEventData> e)
	{
		AddLine(null, "");
	}
	#endregion

	void AddLine(List<PhraseSequence> words, string translation){
		var newLine = SceneResourceManager.Instantiate<ConversationCreationLine>(linePrefab);
		var go = newLine.gameObject;
		go.transform.SetParent(lineParent.transform);
		go.transform.SetAsLastSibling();
		if(words != null){
			newLine.JText.AddPresetWords(words);
		}
		newLine.translation.text = translation;
	}

	#region ICloseable implementation
	public void Close ()
	{
		GameObject.Destroy(this.gameObject);
		Complete.Raise(this, null);
	}
	#endregion
}
