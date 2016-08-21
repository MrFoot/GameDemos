using System;
using UnityEngine;


public class KitchenButtonController : RoomButtonController
{
	private MainGameLogic MainGameLogic;

	protected override GameAction Action
	{
		get
		{
			return GameAction.OpenKitchen;
		}
	}

	private float KitchenPercent
	{
		get
		{
			return Mathf.Clamp01(this.MainGameLogic.FoodMeter / 100f);
		}
	}

	protected override Room Room
	{
		get
		{
			return Room.Kitchen;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.MainGameLogic = Main.Instance.MainGameLogic;
	}

	private void Start()
	{
		this.ForceShowPercent();
	}

	protected override void Update()
	{
		base.Update();
		base.Percent = this.KitchenPercent;
	}

	public override void ForceShowPercent()
	{
		base.ForceShowPercent(this.KitchenPercent);
	}

	protected override bool ShouldProgressBarBeRed()
	{
		return this.MainGameLogic.MustEat;
	}
}


