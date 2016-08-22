using UnityEngine;
using System.Collections;
using Soulgame.Util;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUpController : MonoBehaviour {

	private const string Tag = "StartUpController";

	private const long RequiredFreeSpaceForExtract = 104857600L; //100M

	private bool AssetsDownloaded;

	private bool CompressionFailMessageShown;

	private bool DidStartExtracting;

	private bool StartedMainLoad = false;

	private bool MainLoading;

	private AsyncOperation AsyncMain;

    private int DisplayProgress;
	
    //UI
    public Text Progress;

	private ResourceManager.DecompressInfo DecompressInfo;

	void Awake() {
		UnityLogHandler.RegisterMe ();
		DontDestroyOnLoad (this.gameObject);
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
				StartCoroutine(LoadMain());
			}
			UpdateLoadProgress();
		} else if (this.DecompressInfo.FailedDecompressing) {
			if (!this.CompressionFailMessageShown) {
				this.CompressionFailMessageShown = true;
				//显示减压错误
			}
		} else if (this.AssetsDownloaded) {
			//根据this.DecompressInfo.Progress;显示进度条，一般情况不显示进度
            UpdateDecompress();
		}
	}

	IEnumerator LoadMain() {
		SetSuggestion ();
		yield return null;

		ResourceManager.UnloadUnusedResources ();
		yield return null;

        this.AsyncMain = SceneManager.LoadSceneAsync("Aquarium");
        this.AsyncMain.allowSceneActivation = false;
        this.MainLoading = true;
        yield return this.AsyncMain;

        this.MainLoading = false;
        Debug.Log("+++++++++++++ AsyncMain.Done");

		//可以弹礼包之类的
		yield return null;

		//关闭Loading页面
		ResourceManager.UnloadUnusedResources ();
		Destroy (this.gameObject);
        Debug.Log("Destoryed");
	}

	/// <summary>
	/// 设置一些tips
	/// </summary>
	private void SetSuggestion() {

	}

    /// <summary>
    /// 更新加载进度
    /// </summary>
	private void UpdateLoadProgress()
	{
        if (this.MainLoading)
		{
            float progress = ((this.AsyncMain != null) ? this.AsyncMain.progress : 1f);

            if (progress < 0.89)
            {
                DisplayProgress = (int)(progress * 100);
            }
            else
            {
                DisplayProgress = Mathf.Min(100, ++DisplayProgress);

                if (DisplayProgress == 100 && !this.AsyncMain.allowSceneActivation)
                {
                    this.AsyncMain.allowSceneActivation = true;
                }
            }

            Progress.text = "Progress : " + DisplayProgress + "%";
		}
		    
	}

    /// <summary>
    /// 更新解压进度
    /// </summary>
    private void UpdateDecompress() 
    {
        if (!this.DecompressInfo.Done)
            Progress.text = "Decompressing : " + (int)(this.DecompressInfo.Progress * 100) + "%";
    }
}
