using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

[ResourcePath("UI/ThingTemplateEditor")]
public class ThingTemplateEditorUI : MonoBehaviour, ITemporaryUI<ThingTemplate2D, object> {

    public UIComponents2D components;

    ThingTemplate2D thing;
    IUIComponent<PhraseSequence> nameInput;
    IUIComponent<PhraseSequence> descriptionInput;

    string lastDescription;

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(ThingTemplate2D thing) {
        this.thing = thing;
        nameInput = components.GetInstance<PhraseSequence>(transform, components.phraseLineInput, "Name", thing.Name);
        descriptionInput = components.GetInstance<PhraseSequence>(transform, components.phraseLineInput, "Description", thing.Description);
        if (descriptionInput.Value == null) { 
            descriptionInput.Value = new PhraseSequence(); 
        }
    }

    public void Close() {
        Destroy(gameObject);
    }

    void Update() {
        if (thing != null) {
            thing.Name = nameInput.Value;
            thing.Description = descriptionInput.Value;

            if (lastDescription != thing.Description.GetText()) {
                lastDescription = thing.Description.GetText();
                ThingInventoryLoader.Instance.Save();
            }
        }
    }

}
