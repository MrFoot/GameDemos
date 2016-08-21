using UnityEngine;
using System.Collections;
using System;
using Soulgame.Util;

public class MainUIController : MonoBehaviour {

	private bool ForceStartMainPanel = true;

	public bool IsAnyRoomButtonActive = true;

	//public UIRoot MainUIRoot;

	[SerializeField]
	private GameObject TerraceButtonContainerRef;

	[SerializeField]
	private GameObject KitchenButtonContainerRef;

	[SerializeField]
	private GameObject BedroomButtonContainerRef;

	[SerializeField]
	private GameObject BathroomButtonContainerRef;

	[SerializeField]
	private GameObject ShopMenuPanelRef;

	[SerializeField]
	private GameObject FarmGameContainerRef;

	public RoomButtonsController RoomButtonsController;

	[SerializeField]
	private TerraceButtonController TerraceButtonController;

	[SerializeField]
	private KitchenButtonController KitchenButtonController;

	[SerializeField]
	private BathroomButtonController BathroomButtonController;

	[SerializeField]
	private BedroomButtonController BedroomButtonController;

	public static MainUIController Instance
	{
		get;
		private set;
	}

	void Awake() {
		DontDestroyOnLoad (this);
		MainUIController.Instance = this;

	}

	void Start() {

	}

	private void SetTweenVariables()
	{
        /*
		UIPanel uiPanel = this.MainUIRoot.GetComponent<UIPanel> ();
		uiPanel.ResetAnchors ();
		uiPanel.UpdateAnchors ();
         * */
	}

	public void SetGlowOnButton(RoomButtonController roomButtonController)
	{
		//NGUITools.SetActive(roomButtonController.SelectedButtonEffect.gameObject, true);
	}

	public void OnCommonRoomStatePreEnter(CommonRoomState commonRoomState)
	{
		RoomButtonController roomButtonController = null;
		switch (((Level.LevelEnum) ((int) Enum.Parse(typeof(Level.LevelEnum), commonRoomState.LevelEnum.Name))))
		{
		case Level.LevelEnum.Terrace:
			roomButtonController = this.TerraceButtonController;
			break;

		case Level.LevelEnum.Kitchen:
			roomButtonController = this.KitchenButtonController;
			break;

		case Level.LevelEnum.Bathroom:
			roomButtonController = this.BathroomButtonController;
			break;

		case Level.LevelEnum.Bedroom:
			roomButtonController = this.BedroomButtonController;
			break;
		}
		this.OnCommonRoomStatePreEnter(roomButtonController);
	}

	public void OnCommonRoomStatePreEnter(RoomButtonController roomButtonController)
	{
		Assert.NotNull(roomButtonController, "Room button is not null");
		this.RoomButtonsController.Select(roomButtonController);
		roomButtonController.ForceShowPercent();
		roomButtonController.transform.localScale = Vector3.one;
	}

	public void OnCommonRoomStateEnter()
	{
		this.ToggleAllButtons(true);
	}

	public void OnCommonRoomStateExit()
	{
		this.ToggleAllButtons(false);
		this.RoomButtonsController.ForceHideAllPercentLabels();
	}

	public void ToggleAllButtons(bool active)
	{
        /*
		if (active && this.ForceStartMainPanel)
		{
			if (this.MainUIRoot.GetComponent<UIPanel>() != null)
			{
				this.MainUIRoot.GetComponent<UIPanel>().Refresh();
			}
		}
		this.ToggleNonRoomButtons(active);
		this.ToggleRoomButtons(active);
		if (active && this.ForceStartMainPanel)
		{
			this.ForceStartMainPanel = false;
			UIRoot.Broadcast("ResetAnchors");
			UIRoot.Broadcast("UpdateAnchors");
		}
         * */
	}

	public void ToggleNonRoomButtons(bool active)
	{
        //NGUITools.SetActive(this.ShopMenuPanelRef, active);
        //NGUITools.SetActive(this.FarmGameContainerRef, active);
	}

	public void ToggleRoomButtons(bool active)
	{
        /*
		NGUITools.SetActive(this.TerraceButtonContainerRef, active);
		NGUITools.SetActive(this.KitchenButtonContainerRef, active);
		NGUITools.SetActive(this.BathroomButtonContainerRef, active);
		NGUITools.SetActive(this.BedroomButtonContainerRef, active);
		this.IsAnyRoomButtonActive = active;
         * */
	}

	public void ToggleMainUI(bool enable) {
		//NGUITools.SetActive (base.gameObject, enable);
	}

	public void OnWardrobeEnter(BaseGameState previousState)
	{
		//NGUITools.SetActive(this.CoinsAndDiamondsContainerRef, true);
	}

	public void OnWardrobeExit(BaseGameState nextState)
	{
//		if (nextState != Main.Instance.GameStateManager.MakeUpState)
//		{
//			NGUITools.SetActive(this.CoinsAndDiamondsContainerRef, false);
//		}
	}
}
