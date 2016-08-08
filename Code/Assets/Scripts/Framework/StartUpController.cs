using UnityEngine;
using System.Collections;
using Soulgame.Util;
using UnityEngine.SceneManagement;

public class StartUpController : MonoBehaviour {

	private const string Tag = "StartUpController";

	private const long RequiredFreeSpaceForExtract = 104857600L; //100m

	private bool AssetsDownloaded;

	private bool CompressionFailMessageShown;

	private bool DidStartExtracting;

	private bool StartedMainLoad = false;

	private bool MainLoading;
	
	private bool MainSceneGuiLoading;
	
	private bool RoomLoading;

	private AsyncOperation AsyncMain;
	
	private AsyncOperation AsyncMainSceneGui;
	
	private AsyncOperation AsyncRoom;

	private ResourceManager.DecompressInfo DecompressInfo;

	void Awake() {
		UnityLogHandler.RegisterMe ();
		DontDestroyOnLoad (this);
		DoDownloadingAssets ();
	}

	public void DoDownloadingAssets() {
		this.AssetsDownloaded = true;
		StartExtracting ();
	}

	private void StartExtracting()
	{
		this.CompressionFailMessageShown = false;
		this.DecompressInfo = new ResourceManager.DecompressInfo ();
		StartCoroutine(ResourceManager.ExtractArchive(this.DecompressInfo, StartUpController.RequiredFreeSpaceForExtract));
		if (!this.DecompressInfo.Done) {
			this.DidStartExtracting = true;
			//开始解压资源,显示相关内容
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!pauseStatus && this.DecompressInfo.FailedDecompressing && this.AssetsDownloaded)
		{
			this.StartExtracting();
		}
	}

	void Update() {
		if (this.DecompressInfo.Done) {
			if(!this.StartedMainLoad) {
				if (this.DidStartExtracting) {
					this.DidStartExtracting = false;
					//解压完成
				}
				this.StartedMainLoad = true;
				StartCoroutine(LoadMainAndRoom());
			}
			UpdateProgressBar();
		} else if (this.DecompressInfo.FailedDecompressing) {
			if (!this.CompressionFailMessageShown) {
				this.CompressionFailMessageShown = true;
				//显示减压错误
			}
		} else if (this.AssetsDownloaded) {
			//根据this.DecompressInfo.Progress;显示进度条，一般情况不显示进度
		}
	}

	IEnumerator LoadMainAndRoom() {
		SetSuggestion ();
		yield return null;

		ResourceManager.UnloadUnusedResources ();
		yield return null;

		this.AsyncMain = SceneManager.LoadSceneAsync("Main");
		this.MainLoading = true;
		yield return this.AsyncMain;

        this.AsyncMainSceneGui = SceneManager.LoadSceneAsync("MainGui");
		this.MainSceneGuiLoading = true;
		yield return this.AsyncMainSceneGui;

		//加载房间
        //this.AsyncRoom = SceneManager.LoadSceneAsync(MtaGameStateManager.Instance.EntryState.LevelName);
		this.RoomLoading = true;
		yield return this.AsyncRoom;

		//可以弹礼包之类的
		yield return null;

		//关闭页面
		//MtaGameStateManager.BlockUpdatesOnStart = false;
		ResourceManager.UnloadUnusedResources ();
		Destroy (this.gameObject);
	}

	/// <summary>
	/// 设置一些tips
	/// </summary>
	private void SetSuggestion() {

	}

	private void UpdateProgressBar()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		if (this.MainLoading)
		{
			num = ((this.AsyncMain != null) ? this.AsyncMain.progress : 1f);
		}
		if (this.MainSceneGuiLoading)
		{
			num2 = ((this.AsyncMainSceneGui != null) ? this.AsyncMainSceneGui.progress : 1f);
		}
		if (this.RoomLoading)
		{
			num3 = ((this.AsyncRoom != null) ? this.AsyncRoom.progress : 1f);
		}
		float splashScreenProgress = num * 0.33f + num3 * 0.33f + num2 * 0.33f;
		//更新进度条
	}
}
