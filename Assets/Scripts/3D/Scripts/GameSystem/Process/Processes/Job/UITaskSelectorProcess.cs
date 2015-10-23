using UnityEngine;
using System.Collections;

public class UITaskSelectorProcess : UIProcess<TaskSelectorArgs, JobTaskRef> {

    public UITaskSelectorProcess() : base(UILibrary.Tasks) { }

}