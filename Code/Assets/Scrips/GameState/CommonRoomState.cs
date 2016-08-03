
using System;

public abstract class CommonRoomState : BaseRoomState
{
	public CommonRoomState(MtaGameStateManager stateManager) : base(stateManager)
	{
	}

	public override void OnPreExit(BaseGameState nextState, object data)
	{
		base.OnPreExit(nextState, data);
		if (!(nextState is CommonRoomState))
		{
//			base.DialogStateManager.CheckAndCloseCurrentDialogIfPresent(base.DialogStateManager.Bee7NotificationDialogState);
//			base.DialogStateManager.CheckAndCloseCurrentDialogIfPresent(base.DialogStateManager.UpdateBannerDialogState);
//			Main.Instance.AdManager.BannerEnabled = false;
		}
	}

	public override BaseGameState OnPreEnter(BaseGameState previousState, object data)
	{
		base.OnPreEnter(previousState, data);
//		if (previousState != null)
//		{
//			MainSceneGuiController.Instance.OnCommonRoomStatePreEnter(this);
//		}
//		if (previousState is PurchaseDiamondsState)
//		{
//			ItemStateShareData<FoodItem, FoodItemCategory> itemStateShareData = data as ItemStateShareData<FoodItem, FoodItemCategory>;
//			if (itemStateShareData != null)
//			{
//				bool flag = Main.Instance.FoodItemManager.CheckBuyAndConsumeEnergyPotion();
//				if (flag)
//				{
//					if (itemStateShareData.State == AbstractItemStateShareData.ItemState.BuyAndConsumeOrOpenDialog)
//					{
//						return base.GameStateManager.GetGameWallState;
//					}
//					if (this is BedroomState)
//					{
//						Main.Instance.SceneStateManager.BedroomSceneState.TriggerEnergyPotionEffectOnEnter = true;
//					}
//					else
//					{
//						if (!(this is TerraceState))
//						{
//							throw new InvalidProgramException("Undefined behaviour. Are you trying to auto trigger potion effect otherwhere than Bedroom or terrace?");
//						}
//						Main.Instance.SceneStateManager.TerraceSceneState.TriggerEnergyPotionEffectOnEnter = true;
//					}
//				}
//			}
//		}
		return this;
	}

	public override void OnEnter(BaseGameState previousState, object data)
	{
//		if (previousState == null)
//		{
//			MainSceneGuiController.Instance.OnCommonRoomStatePreEnter(this);
//		}
		base.OnEnter(previousState, data);
//		this.UpdateGridButtonTriggerTime();
//		Main.Instance.AdManager.BannerEnabled = (base.DialogStateManager.CurrentState != base.DialogStateManager.UpdateBannerDialogState);
//		MainSceneGuiController.Instance.OnCommonRoomStateEnter();
//		Main.Instance.StickerAlbumPromoManager.ResetDidShowPromo();
		this.CheckAndOpenDialogsOnEnter(previousState, data);
//		Main.Instance.PublisherHelper.ToggleNotificationShowing(true);
	}

	public override void OnExit(BaseGameState nextState, object data)
	{
		base.OnExit(nextState, data);
//		Main.Instance.PublisherHelper.ToggleNotificationShowing(false);
//		if (!(nextState is CommonRoomState))
//		{
//			MainSceneGuiController.Instance.OnCommonRoomStateExit();
//		}
	}

	public override void OnAction(GameAction gameAction, object data)
	{
		switch (gameAction) {
		case GameAction.BackButton:
			this.ChangeState (null);
			return;
		}
	}

	public override void OnUpdate()
	{
//		if (base.GameStateManager.Bee7GameWallState.GameWallShown)
//		{
//			base.GameStateManager.Bee7GameWallState.HideGameWall(delegate
//			                                                     {
//				Main.Instance.DialogStateManager.OpenDialog(Main.Instance.DialogStateManager.InterstitialDialogState);
//			});
//			return;
//		}
//		bool flag = this.TryOpenAutoNews();
//		if (flag)
//		{
//			return;
//		}
//		Main.Instance.RoomEditController.OnCommonAndEditRoomUpdate();
		this.CheckAndTryToOpenPendingDialogsAndStates();
//		if (Time.time > this.TriggerTime && this.TriggerTime > 0f)
//		{
//			MainSceneGuiController.Instance.GetGridButton.CheckAndTriggerButtonAnimation();
//			MainSceneGuiController.Instance.GetManualNewsButton.CheckAndTriggerButtonAnimation();
//		}
	}

	private bool TryOpenAutoNews()
	{
//		if (O7Test<MtaTest>.Instance.IsTestMode)
//		{
//			return false;
//		}
//		if (!Main.Instance.AutoNewsManager.IsNewsReady)
//		{
//			return false;
//		}
//		if (base.DialogStateManager.CurrentState != null)
//		{
//			this.ReportSkipNewsEvent("dialog=" + base.DialogStateManager.CurrentState.GetType().Name);
//			return false;
//		}
//		if (!AppPlugin.IsNetworkAvailable)
//		{
//			this.ReportSkipNewsEvent("no-network");
//			return false;
//		}
//		if (base.GameStateManager.WhatsNewState.ShownInSessionId == Main.Instance.AppSession.SessionId)
//		{
//			this.ReportSkipNewsEvent("whats-new-same-session");
//			return false;
//		}
//		base.GameStateManager.NewsProxyState.OpenNews(Main.Instance.AutoNewsInteraction);
		return false;
	}

	public override void OnAppResume()
	{
		base.OnAppResume();
		//this.UpdateGridButtonTriggerTime();
		//Main.Instance.PublisherHelper.ToggleNotificationShowing(true);
	}
	
	public override void OnAppPause()
	{
		//Main.Instance.PublisherHelper.ToggleNotificationShowing(false);
		base.OnAppPause();
		//this.TriggerTime = -1f;
	}

	// Outfit7.MyTalkingAngela.GameState.CommonRoomState
	private void CheckAndTryToOpenPendingDialogsAndStates()
	{
		base.GameStateManager.TryToAutoOpen(base.GameStateManager.CheckAndOpenRestoreStatesFunct);
		base.GameStateManager.TryToAutoOpen(base.GameStateManager.CheckAndOpenCommonRoomStatesFunct);
//		base.GameStateManager.TryToAutoOpen(base.DialogStateManager.CheckAndOpenAgeGateDialogFunct);
//		base.GameStateManager.TryToAutoOpen(base.DialogStateManager.CheckAndOpenCommonRoomDialogsFunct);
//		base.GameStateManager.TryToAutoOpen(base.DialogStateManager.CheckAndOpenRewardDialogsFunct);
//		base.GameStateManager.TryToAutoOpen(base.DialogStateManager.CheckAndOpenFloaterAdFunct);
	}

	protected bool ConsumeOrBuyEnergyPotion()
	{
//		if (Main.Instance.FoodItemManager.CheckBuyAndConsumeEnergyPotion())
//		{
//			Main.Instance.AudioManager.MainAudioPlayer.PlayOneShotSoundSFX(AudioClipNames.BuyDiamond);
//			SpecialEffectHelper.TriggerEnergyPotionEfect();
//			return true;
//		}
//		ItemStateShareData<FoodItem, FoodItemCategory> data = new ItemStateShareData<FoodItem, FoodItemCategory>(FoodItemFactory.EnergyPotion, AbstractItemStateShareData.ItemState.BuyAndConsume);
//		this.ChangeState(base.GameStateManager.PurchaseDiamondState, data);
		return false;
	}
	
	
	protected virtual void CheckAndOpenDialogsOnEnter(BaseGameState previousState, object data)
	{
//		if (previousState == base.GameStateManager.WardrobeState || previousState == base.GameStateManager.FoodStoreState || previousState == base.GameStateManager.StickerAlbumState || previousState == base.GameStateManager.GetGameWallState)
//		{
//			Main.Instance.DialogStateManager.OpenDialog(Main.Instance.DialogStateManager.InterstitialDialogState);
//		}
	}
}


