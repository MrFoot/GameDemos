using UnityEngine;
using System.Collections;
using Soulgame.Util;
using System;

public class MainGameLogic : MonoBehaviour {

	public enum GroomingStateEnum
	{
		NotGrooming,
		Showering,
		TeethCleaning
	}

	public enum MakingFunStateEnum
	{
		NotMakingFun,
		Petting,
		PlayingMiniGame
	}

	private const string Tag = "MainGameLogic";

	private const string PrefBedroomLightOn = "MainGameLogic.BedroomLightOn";

	private const float FunMeterLowThreshold = 30f;

	private const float FoodMeterLowThreshold = 30f;

	private const float GroomMeterLowThreshold = 30f;

	private const float SleepyMeterLowThreshold = 30f;

	public const float DefaultFunMeter = 100f;

	public const float DefaultFoodMeter = 100f;

	public const float DefaultBodyCleanliness = 100f;

	public const float DefaultTeethCleanliness = 100f;

	public const float DefaultSleepyMeter = 100f;

	private const float FunMeterHighThreshold = 60f;

	private const float FoodMeterHighThreshold = 60f;

	private const float GroomMeterHighThreshold = 60f;

	private const float SleepyMeterHighThreshold = 60f;

	private const float SleepyMeterLimit = 90f;

	public const float FunMeterCap = 100f;

	public const float FunMeterMax = 200f;

	public const float FoodMeterMax = 100f;

	public const float GroomMeterMax = 100f;

	public const float BodyCleanlinessMax = 100f;

	public const float TeethCleanlinessMax = 100f;

	public const float SleepyMeterMax = 100f;

	public bool IsBedroomLightOn
	{
		get;
		private set;
	}

	public bool IsSleeping
	{
		get;
		private set;
	}

	public double Xp
	{
		get;
		private set;
	}

	/// <summary>
	/// 快乐程度
	/// </summary>
	/// <value>The fun meter.</value>
	public float FunMeter
	{
		get;
		private set;
	}

	public bool MustMakeFun
	{
		get
		{
			return this.FunMeter <= FunMeterLowThreshold;
		}
	}

	public float BodyCleanliness
	{
		get;
		private set;
	}

	public float TeethCleanliness
	{
		get;
		private set;
	}

	public float GroomMeter
	{
		get
		{
			float num = this.BodyCleanliness + this.TeethCleanliness;
			return num * num * 0.0025f; //除400
		}
	}

	public MainGameLogic.GroomingStateEnum GroomingState
	{
		get;
		private set;
	}

	public bool IsGrooming
	{
		get
		{
			return this.GroomingState != MainGameLogic.GroomingStateEnum.NotGrooming;
		}
	}

	public bool MustGroom
	{
		get
		{
			return this.MustShower || this.MustCleanTeeth;
		}
	}

	public bool MustShower
	{
		get
		{
			return this.BodyCleanliness <= GroomMeterLowThreshold;
		}
	}

	public bool MustCleanTeeth
	{
		get
		{
			return this.TeethCleanliness <= GroomMeterLowThreshold;
		}
	}

	public float SleepyMeter
	{
		get;
		private set;
	}

	public bool MustSleep
	{
		get
		{
			return this.ShouldSleep && this.SleepyMeter <= SleepyMeterLowThreshold;
		}
	}

	public bool ShouldSleep
	{
		get
		{
			return this.CanSleep && this.SleepyMeter <= SleepyMeterHighThreshold;
		}
	}

	public bool CanSleep
	{
		get
		{
			return this.SleepyMeter <= SleepyMeterLimit;
		}
	}

	public float FoodMeter
	{
		get;
		private set;
	}

	public bool MustEat
	{
		get
		{
			return this.ShouldEat && this.FoodMeter <= FoodMeterLowThreshold;
		}
	}

	public bool ShouldEat
	{
		get
		{
			return this.CanEat && this.FoodMeter <= FoodMeterHighThreshold;
		}
	}

	public bool CanEat
	{
		get
		{
			return !MainGameLogic.AreApproxEqual(this.FoodMeter, FoodMeterMax);
		}
	}

	private int LevelCache = -1;

	public int Level
	{
		get
		{
			if (this.LevelCache == -1)
			{
				this.LevelCache = MainGameLogic.GetLevelForXp(this.Xp);
			}
			return this.LevelCache;
		}
	}

	public void OnAppResume()
	{

	}

	public void OnAppPause()
	{

	}

	public void Init() {
		this.IsBedroomLightOn = UserPrefs.GetBool(MainGameLogic.PrefBedroomLightOn, true);
	}

	public static void ClearPrefs()
	{
		UserPrefs.Remove(MainGameLogic.PrefBedroomLightOn);
	}

	public static int GetLevelForXp(double xp)
	{
		return 1;
	}

	/// <summary>
	/// 大约等于
	/// </summary>
	/// <returns><c>true</c>, if approx equal was ared, <c>false</c> otherwise.</returns>
	/// <param name="meter1">Meter1.</param>
	/// <param name="meter2">Meter2.</param>
	private static bool AreApproxEqual(float meter1, float meter2)
	{
		return Math.Abs(meter1 - meter2) < 0.4f;
	}

	private static bool AreEqual(float meter1, float meter2)
	{
		return Math.Abs(meter1 - meter2) < 0.001f;
	}

	private static bool AreEqual(double xp1, double xp2)
	{
		return Math.Abs(xp1 - xp2) < 0.001;
	}
}
