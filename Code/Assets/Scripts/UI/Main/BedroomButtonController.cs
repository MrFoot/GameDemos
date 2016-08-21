using System;
using UnityEngine;


public class BedroomButtonController : RoomButtonController
{
	private MainGameLogic MainGameLogic;

	protected override GameAction Action
	{
		get
		{
			return GameAction.OpenBedroom;
		}
	}

	private float BedroomPercent
	{
		get
		{
			return Mathf.Clamp01(this.MainGameLogic.SleepyMeter / 100f);
		}
	}

	protected override Room Room
	{
		get
		{
			return Room.Bedroom;
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
		base.Percent = this.BedroomPercent;
	}

	public override void ForceShowPercent()
	{
		base.ForceShowPercent(this.BedroomPercent);
	}

	protected override bool ShouldProgressBarBeRed()
	{
		return this.MainGameLogic.MustSleep;
	}
}


