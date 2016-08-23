using System;
using UnityEngine;
using UnityEngine.UI;
using Soulgame.Util;

public class AquariumSceneController : BaseSceneController
{
    public Button BtnFisheries;

    public Button BtnShop;

	public AquariumSceneController ()
	{
		this.Level = Level.Aquarium;
	}

    public void OnBtnFisheries()
    {
        GameLog.Debug("OnBtnFisheries");
        Main.Instance.GameStateManager.FireAction(GameAction.ToFisheries);
    }

    public void OnBtnShop() 
    {
        GameLog.Debug("OnBtnShop");
        Main.Instance.GameStateManager.FireAction(GameAction.ToShop);
    }


}


