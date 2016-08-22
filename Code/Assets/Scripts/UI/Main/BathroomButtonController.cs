using System;
using UnityEngine;


public class BathroomButtonController : RoomButtonController
{
	private MainGameLogic MainGameLogic;

	protected override GameAction Action
	{
		get
		{
			return GameAction.OpenBathroom;
		}
	}

	private float BathroomPercent
	{
		get
		{
			return Mathf.Clamp01(this.MainGameLogic.GroomMeter / 100f);
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
		base.Percent = this.BathroomPercent;
	}

	public override void ForceShowPercent()
	{
		base.ForceShowPercent(this.BathroomPercent);
	}

	protected override bool ShouldProgressBarBeRed()
	{
		return this.MainGameLogic.MustGroom;
	}
}


