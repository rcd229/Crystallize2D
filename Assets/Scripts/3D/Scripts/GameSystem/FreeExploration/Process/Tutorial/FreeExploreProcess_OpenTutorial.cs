using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class FreeExploreProcess_OpenTutorial : BaseFreeExploreProcess {

    protected override void AfterInitialize() {
        PlayerDataConnector.SetHUDPartEnabled(HUDPartType.Map, true);
        PlayerDataConnector.SetHUDPartEnabled(HUDPartType.ChatBox, true);
        PlayerDataConnector.SetHUDPartEnabled(HUDPartType.QuestStatus, true);
        new MapIntroProcess().Run(null, BeginExplore, this);

        // TODO: find a better way to manage this
        QuestUtil.RaiseFlag(NPCQuestFlag.PointPlaceUnlockedFlag);
    }

    class MapIntroProcess : EnumeratorProcess<object, object> {
        public override IEnumerator<SubProcess> Run(object args) {
            yield return Get(UILibrary.MessageBox, "You can now explore freely.");
            yield return Get(UILibrary.MessageBox, "You can use the map to find important characters.");
        }
    }

}
