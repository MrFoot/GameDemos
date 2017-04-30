using UnityEngine;
using System.Collections;
using FootStudio.Framework;
using FootStudio.Util;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppStart : MonoBehaviour {

	private const string Tag = "StartUpController";

	private const long RequiredFreeSpaceForExtract = 104857600L; //100M

    //Download
    private bool mIsStartedDownload = false;

    private bool mIsAssetsDownloaded = false;

    //Extract
    private bool mIsStartedExtract = false;

    private ResourceManager.DecompressInfo DecompressInfo;

    private bool CompressionFailMessageShown = false;

    //Load Main

	private bool StartedMainLoad = false;

	private AsyncOperation AsyncMain;

    private int DisplayProgress;
	
    //UI
    public Text Progress;

    public Slider slider;


	void Awake() {
		UnityLogHandler.RegisterMe ();
		DontDestroyOnLoad (this.gameObject);
		DoDownloadingAssets ();
	}

    /// <summary>
    /// 下载资源
    /// </summary>
	public void DoDownloadingAssets() {
        mIsStartedDownload = true;
        mIsAssetsDownloaded = true;
		StartExtracting ();
	}

    /// <summary>
    /// 解压资源
    /// </summary>
	private void StartExtracting()
	{
		this.CompressionFailMessageShown = false;
		this.DecompressInfo = new ResourceManager.DecompressInfo ();
		StartCoroutine(ResourceManager.ExtractArchive(this.DecompressInfo, AppStart.RequiredFreeSpaceForExtract));
		
        if (!this.DecompressInfo.Done) {
			mIsStartedExtract = true;
			//开始解压资源,显示相关内容
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!pauseStatus && this.DecompressInfo.FailedDecompressing && mIsAssetsDownloaded)
		{
			this.StartExtracting();
		}
	}

	void Update() {
        if (mIsAssetsDownloaded && mIsStartedExtract && !DecompressInfo.Done) 
        {
			//根据this.DecompressInfo.Progress;显示进度条，一般情况不显示进度
            UpdateDecompressProgress();

            if (this.DecompressInfo.FailedDecompressing)
            {
                if (!this.CompressionFailMessageShown)
                {
                    this.CompressionFailMessageShown = true;
                    //显示减压错误
                }
            }
		}
		else if (this.DecompressInfo.Done) 
        {
			if(!StartedMainLoad) 
            {
                StartedMainLoad = true;
				StartCoroutine(LoadMain());
			}

            if (StartedMainLoad && AsyncMain != null && !AsyncMain.isDone)
            {
                UpdateLoadProgress();
            }
		}
       
	}

	IEnumerator LoadMain() {
		SetSuggestion ();
		yield return null;

		ResourceManager.UnloadUnusedResources ();
        this.AsyncMain = SceneManager.LoadSceneAsync("Main");
        this.AsyncMain.allowSceneActivation = false;

        yield return this.AsyncMain;
        
        Debug.Log("+++++++++++++ AsyncMain.Done");
		yield return new WaitForSeconds(1f);

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
                //Debug.LogError("allowSceneActivation = true");
            }
        }

        Progress.text = "Progress : " + DisplayProgress + "%";
        slider.value = (float)DisplayProgress / 100f;
	}

    /// <summary>
    /// 更新解压进度
    /// </summary>
    private void UpdateDecompressProgress() 
    {
        if (!this.DecompressInfo.Done)
        {
            Progress.text = "Decompressing : " + (int)(this.DecompressInfo.Progress * 100) + "%";
            slider.value = this.DecompressInfo.Progress;
        }
    }
}
