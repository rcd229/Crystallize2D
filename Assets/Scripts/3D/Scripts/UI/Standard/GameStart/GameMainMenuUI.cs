using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameMainMenuUI : UIPanel, ITemporaryUI<object, bool> {

    const string ResourcePath = "UI/GameMainMenu";
	public static GameMainMenuUI GetInstance() {
		return GameObjectUtil.GetResourceInstance<GameMainMenuUI>(ResourcePath);
    }

	public UIButton LoadGame;
	public UIButton NewGame;

    public event EventHandler<EventArgs<bool>> Complete;

    public void Initialize(object param1)
    {
		LoadGame.OnClicked += (sender, e) => Exit (false);
		NewGame.OnClicked += (sender, e) => Exit (true);

    }
	
    void Exit(bool isNewGame) {
        Complete.Raise(this, new EventArgs<bool>(isNewGame));
    }

}