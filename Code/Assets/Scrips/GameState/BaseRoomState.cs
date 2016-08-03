
using System;

public abstract class BaseRoomState : SceneStateSupport
{
	public BaseRoomState(MtaGameStateManager stateManager) : base(stateManager)
	{
	}

	/// <summary>
	/// 改变房间状态
	/// </summary>
	/// <param name="gameAction">Game action.</param>
	/// <param name="data">Data.</param>
	public override void OnAction(GameAction gameAction, object data)
	{
//		switch (gameAction)
//		{
//		case GameAction.OpenTerrace:
//			MainSceneGuiController.Instance.SetGlowOnButton(MainSceneGuiController.Instance.RoomButtonsController.Terrace);
//			this.ChangeRoomState(base.GameStateManager.TerraceState);
//			return;
//		case GameAction.OpenBedroom:
//			MainSceneGuiController.Instance.SetGlowOnButton(MainSceneGuiController.Instance.RoomButtonsController.Bedroom);
//			this.ChangeRoomState(base.GameStateManager.BedroomState);
//			return;
//		case GameAction.OpenKitchen:
//			MainSceneGuiController.Instance.SetGlowOnButton(MainSceneGuiController.Instance.RoomButtonsController.Kitchen);
//			this.ChangeRoomState(base.GameStateManager.KitchenState);
//			return;
//		case GameAction.OpenBathroom:
//			MainSceneGuiController.Instance.SetGlowOnButton(MainSceneGuiController.Instance.RoomButtonsController.Bathroom);
//			this.ChangeRoomState(base.GameStateManager.BathroomState);
//			return;
//		case GameAction.ShowLevelTree:
//			this.ChangeState(base.GameStateManager.LevelTreeState);
//			return;
//		case GameAction.ToggleRecording:
//			EveryplayDialogState.ToggleRecording();
//			return;
//		case GameAction.ShowXPBoosterPopup:
//			this.DialogStateManager.OpenDialog(this.DialogStateManager.TutorialBoosterDialogState, data);
//			return;
//		}
//		throw new NotImplementedException(string.Concat(new object[] {
//			"Unimplemented GameAction Action triggered: ",
//			gameAction,
//			" on ",
//			this
//		}));
	}
	
//	private void ChangeRoomState(CommonRoomState state)
//	{
//		this.ChangeState(state);
//		if (!Outfit7.StateManagement.StateManager.StateChanging)
//		{
//			MainSceneGuiController.Instance.RoomButtonsController.ForceShowAllPercentLabels();
//		}
//	}
	
	public override void OnEnter(BaseGameState previousState, object data)
	{
		base.OnEnter(previousState, data);
		//Main.Instance.TrackingManager.AddEvent("screen-room", "screen-enter", null, this.LevelName, null, null, null, null);
	}
	
	public override void OnExit(BaseGameState nextState, object data)
	{
		base.OnExit(nextState, data);
		//Main.Instance.TrackingManager.AddEvent("screen-room", "screen-exit", null, this.LevelName, null, null, null, null);
	}
	
	public override void OnAppResume()
	{
		base.OnAppResume();
		//Main.Instance.TrackingManager.AddEvent("screen-room", "screen-enter", null, this.LevelName, null, null, null, null);
	}
	
	public override void OnAppPause()
	{
		base.OnAppPause();
		//Main.Instance.TrackingManager.AddEvent("screen-room", "screen-exit", null, this.LevelName, null, null, null, null);
	}
}


