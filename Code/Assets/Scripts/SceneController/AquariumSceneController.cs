using System;
using UnityEngine;
using UnityEngine.UI;
using Soulgame.Util;
using System.Collections;
using System.Collections.Generic;

public class AquariumSceneController : BaseSceneController
{
    public Button BtnFisheries;

    public Button BtnShop;

    public List<CharacterBase> Fishes;

	public AquariumSceneController ()
	{
		this.Level = Level.Aquarium;
        Fishes = new List<CharacterBase>();
	}

    public void OnBtnFisheries()
    {
        GameLog.Debug("OnBtnFisheries");
        //Main.Instance.GameStateManager.FireAction(GameAction.ToFisheries);
    }

    public void OnBtnShop() 
    {
        GameLog.Debug("OnBtnShop");
        //Main.Instance.GameStateManager.FireAction(GameAction.ToShop);
        Fishes[0].Init();
    }

    public override void OnEnter() {
        base.OnEnter();

        CharacterBase Fish = CharacterFactory.CreateCharacter(0);
        Fishes.Add(Fish);
    }

    public override void OnUpdate() {
        for (int i = 0 ; i < Fishes.Count ; i++)
        {
            Fishes[i].OnUpdate();
        }
        base.OnUpdate();
    }

}


