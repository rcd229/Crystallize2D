using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class JobTaskPanelUI : UIPanel, ITemporaryUI<TaskSelectorArgs, JobTaskRef> {

    const string ResourcePath = "UI/JobTaskPanel";
    public static JobTaskPanelUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<JobTaskPanelUI>(ResourcePath);
    }
    
    public GameObject buttonPrefab;
    public RectTransform buttonParent;

    public event System.EventHandler<EventArgs<JobTaskRef>> Complete;

    int variation = 0;
    TaskSelectorArgs args;

    public void Initialize(TaskSelectorArgs param1) {
        args = param1;
        var tasks = new List<JobTaskGameData>(param1.Job.GameDataInstance.Tasks);
        //tasks.Add(param1.Job.GameDataInstance.PromotionTask);
        UIUtil.GenerateChildren(tasks, new List<GameObject>(), buttonParent, CreateChild);

        var instance = Instantiate<GameObject>(buttonPrefab);
        instance.GetComponentInChildren<Text>().text = "Promotion";
        instance.GetComponent<UIButton>().OnClicked += Promotion_OnClicked;
        instance.transform.SetParent(buttonParent.transform, false);
    }

    GameObject CreateChild(JobTaskGameData task) {
        var instance = Instantiate<GameObject>(buttonPrefab);
        instance.GetComponentInChildren<Text>().text = task.Name;
        instance.GetComponent<UIButton>().OnClicked += JobTaskPanelUI_OnClicked;
        instance.AddComponent<DataContainer>().Store(task);
        return instance;
    }

    void JobTaskPanelUI_OnClicked(object sender, System.EventArgs e) {
        var task = ((Component)sender).GetComponent<DataContainer>().Retrieve<JobTaskGameData>();
        Exit(new JobTaskRef(args.Job, task, variation, JobTaskType.Normal));
    }

    void Promotion_OnClicked(object sender, System.EventArgs e) {
        Exit(new JobTaskRef(args.Job, args.Job.GameDataInstance.PromotionTask, PromotionTaskData.PromotionVariation, JobTaskType.Promotion));
    }

    void Exit(JobTaskRef args) {
        Complete.Raise(this, new EventArgs<JobTaskRef>(args));
    }

    public void SetVariation(string variation) {
        int v = 0;
        int.TryParse(variation, out v);
        this.variation = v;
    }

}