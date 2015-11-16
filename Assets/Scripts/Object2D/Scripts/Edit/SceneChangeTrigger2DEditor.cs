using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

// To add controls to the editor, you need to use this attribute
[Object2DEditor(typeof(SceneChangeTrigger2D))]
// All editors should derive from Object2DEditorBase, otherwise an error will be thrown
public class SceneChangeTrigger2DEditor : Object2DEditorBase {
    public override void ConstructEditor() {
        // if we want a reference to the typed Object2D, we need to cast it
        var o = (SceneChangeTrigger2D)Object;
        // TODO: the GetAllLevels method should be moved out of GameLevel2D and just check which folders exist in the data folder
        var levels = GameLevel2DLoader.GetAllLevels().ToArray();

        // you can add whatever controls you want
        // currently, only DropDown, string inputs and buttons are implemented, 
        // but more can be added by modifying the Object2DEditorControls class and 
        // the prefab: Scripts/Object2D/Resources/Object2DEditorBase
        Controls.AddDropDown(levels.Select(l => l.levelname), 
            GetSelectedLevelIndex(levels, o.Scene), 
            (i) => SelectScene(o, levels, i));
        // TODO: add dropdown for target
        var objs = Object2DLoader.LoadAll(o.Scene);
        var targets = (from t in objs
                      where t is SceneTarget2D
                      select t).ToList();
        var currentTarget = (from target in targets where target.Guid == o.Target select target).FirstOrDefault();
        if (o.Target == Guid.Empty && targets.Count > 0)
        {
            o.Target = targets[0].Guid;
        }
        Controls.AddDropDown(targets.Select(s => s.Name),
            targets.IndexOf(currentTarget),
            (i) => SelectTarget(o, targets, i));
        Controls.AddButton(Save, "Save");
    }

    int GetSelectedLevelIndex(IEnumerable<GameLevel2D> levels, string levelname) {
        int index = 0;
        foreach(var l in levels) {
            if(l.levelname == levelname) {
                return index;
            }
            index++;
        }
        return 0;
    }


    void SelectScene(SceneChangeTrigger2D obj, GameLevel2D[] levels, int selected) {
        obj.Scene = levels[selected].levelname;
        Save();
    }

    void SelectTarget(SceneChangeTrigger2D obj, List<Object2D> objects, int selected)
    {
        obj.Target = objects[selected].Guid;
        Save();
    }
}
