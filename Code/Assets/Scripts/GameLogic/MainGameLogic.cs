using UnityEngine;
using System.Collections;
using Soulgame.Util;

public class MainGameLogic : MonoBehaviour {

	private const string Tag = "MainGameLogic";

	private const string PrefBedroomLightOn = "MainGameLogic.BedroomLightOn";

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
}
