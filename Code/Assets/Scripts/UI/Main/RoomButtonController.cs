using System;
using UnityEngine;


public abstract class RoomButtonController : GameActionButtonController
{
	[Range(0f, 1f)]
	private float percent = 0f;

	private float TargetPercent;

	private float PercentSpeed = 0.6f;

	private float BackgroundColorTransitionSpeed = 3f;

	public bool BlockProgress;

    //public UIProgressBar ProgressBar;

    //public UIWidget Thumb;

	public PercentageLabelController PercentageLabelController;

	public GameObject SelectedButtonEffect;

//	[SerializeField]
//	private GameObject UpgradeArrow;

	private static Color RedBackgroundColor = new Color32(255, 5, 5, 255);

	private static Color WhiteBackgroundColor = Color.white;

	private static Color PurpleThumbColor = new Color32(214, 134, 190, 255);

	private static Color WhiteThumbColor = new Color32(255, 228, 247, 255);

	//private UIWidget ProgressBarBackground;

	private bool ForceShowPercentOnFirstUpdate;

	private AnimationCurve ThumbWidthCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 40f),
			new Keyframe(0.01f, 75f),
			new Keyframe(0.02f, 80f),
			new Keyframe(0.03f, 90f),
			new Keyframe(0.04f, 100f),
			new Keyframe(0.05f, 111f),
			new Keyframe(0.06f, 113f),
			new Keyframe(0.07f, 123f),
			new Keyframe(0.1f, 126f),
			new Keyframe(0.15f, 139f),
			new Keyframe(0.2f, 149f),
			new Keyframe(0.3f, 155f),
			new Keyframe(0.35f, 158f),
			new Keyframe(0.4f, 158f),
			new Keyframe(0.5f, 162f),
			new Keyframe(0.6f, 158f),
			new Keyframe(0.65f, 158f),
			new Keyframe(0.7f, 155f),
			new Keyframe(0.8f, 149f),
			new Keyframe(0.85f, 139f),
			new Keyframe(0.9f, 126f),
			new Keyframe(0.93f, 118f),
			new Keyframe(0.94f, 115f),
			new Keyframe(0.95f, 105f),
			new Keyframe(1f, 40f)
		});

	protected float Percent
	{
		get
		{
			return this.percent;
		}
		set
		{
			if (this.percent == value || this.BlockProgress)
			{
				return;
			}
			this.TargetPercent = value;
			this.UpdatePercentLabel();
		}
	}

	protected override void Awake ()
	{
		base.Awake ();
		//this.ProgressBarBackground = this.ProgressBar.backgroundWidget;
	}

	protected abstract Room Room
	{
		get;
	}

	public void ShowUpgradingArrow(bool isUpgrading)
	{
		//NGUITools.SetActive(this.UpgradeArrow, isUpgrading);
	}

	protected void ForceShowPercent(float targetPercent)
	{
		this.percent = targetPercent;
		//this.PercentageLabelController.ForceShowPercent(this.percent);
		//this.ProgressBar.value = this.percent;
		//this.Thumb.width = Mathf.RoundToInt(this.ThumbWidthCurve.Evaluate(this.percent));
		//this.ProgressBarBackground.color = ((!this.ShouldProgressBarBeRed()) ? RoomButtonController.WhiteBackgroundColor : RoomButtonController.RedBackgroundColor);
		//this.Thumb.color = ((!this.ShouldProgressBarBeRed()) ? RoomButtonController.PurpleThumbColor : RoomButtonController.WhiteThumbColor);
	}

	private void SetProgressBarPercent(float val)
	{
        //this.ProgressBar.value = val;
        //this.Thumb.width = Mathf.RoundToInt(this.ThumbWidthCurve.Evaluate(val));
	}

	protected virtual bool ShouldProgressBarBeRed()
	{
		return false;
	}

	protected virtual void Update()
	{
		//this.CheckAndShowUpgradingArrow();
		if (this.ForceShowPercentOnFirstUpdate)
		{
			//this.ForceShowPercent();
			this.ForceShowPercentOnFirstUpdate = false;
		}
		//this.UpdatePercentLabel();
		//this.UpdateProgressBarColor();
	}

	private void UpdateProgressBarColor()
	{
        /*
		if (this.ShouldProgressBarBeRed())
		{
			if (this.ProgressBarBackground.color != RoomButtonController.RedBackgroundColor)
			{
				this.ProgressBarBackground.color = Vector4.MoveTowards(this.ProgressBarBackground.color, RoomButtonController.RedBackgroundColor, Time.deltaTime * this.BackgroundColorTransitionSpeed);
				this.Thumb.color = Vector4.MoveTowards(this.Thumb.color, RoomButtonController.WhiteThumbColor, Time.deltaTime * this.BackgroundColorTransitionSpeed);
			}
		}
		else if (this.ProgressBarBackground.color != Color.white)
		{
			this.ProgressBarBackground.color = Vector4.MoveTowards(this.ProgressBarBackground.color, RoomButtonController.WhiteBackgroundColor, Time.deltaTime * this.BackgroundColorTransitionSpeed);
			this.Thumb.color = Vector4.MoveTowards(this.Thumb.color, RoomButtonController.PurpleThumbColor, Time.deltaTime * this.BackgroundColorTransitionSpeed);
		}
         * */
	}

	private void UpdatePercentLabel()
	{
		if (Mathf.Abs(this.TargetPercent - this.percent) > 0.0001f)
		{
			this.percent = Mathf.MoveTowards(this.percent, this.TargetPercent, Time.deltaTime * this.PercentSpeed);
			this.PercentageLabelController.Percent = this.percent;
			this.SetProgressBarPercent(this.percent);
		}
	}

	public virtual void ForceShowPercent()
	{
	}

	public void ForceHidePercent()
	{
		//this.PercentageLabelController.ForceHidePercent();
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			this.ForceShowPercentOnFirstUpdate = true;
		}
	}

//	public void CheckAndShowUpgradingArrow()
//	{
//		if (Main.Instance.GameStateManager.CurrentState == null)
//		{
//			return;
//		}
//		RoomItem upgradingItem = Main.Instance.RoomItemManager.UpgradingItem;
//		if (upgradingItem != null && Main.Instance.GameStateManager.CurrentState.LevelName != upgradingItem.Category.Room.Level.Name && upgradingItem.IsUpgradeFinished)
//		{
//			this.ShowUpgradingArrow(upgradingItem.Category.Room == this.Room);
//		}
//		else
//		{
//			this.ShowUpgradingArrow(false);
//		}
//	}
}


