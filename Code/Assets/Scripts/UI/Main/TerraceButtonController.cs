using System;
using UnityEngine;


public class TerraceButtonController : RoomButtonController
{
	private MainGameLogic MainGameLogic;

	protected override GameAction Action {
		get {
			return GameAction.OpenTerrace;
		}
	}

	private float TerracePercent
	{
		get
		{
			return Mathf.Clamp01(this.MainGameLogic.FunMeter / 100f);
		}
	}

	protected override Room Room {
		get {
			return Room.Terrace;
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
		base.Percent = this.TerracePercent;
	}

	public override void ForceShowPercent()
	{
		base.ForceShowPercent(this.TerracePercent);
	}

	protected override bool ShouldProgressBarBeRed()
	{
		return this.MainGameLogic.MustMakeFun;
	}
}


