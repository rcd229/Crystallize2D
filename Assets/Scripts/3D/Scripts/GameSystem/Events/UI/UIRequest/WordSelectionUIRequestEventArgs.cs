using UnityEngine;
using System.Collections;

public class WordSelectionUIRequestEventArgs : UIRequestEventArgs {

    public IWordDropHandler Container { get; set; }

    public WordSelectionUIRequestEventArgs(GameObject menuParent, IWordDropHandler container) : base(menuParent) {
        this.Container = container;
    }

}
