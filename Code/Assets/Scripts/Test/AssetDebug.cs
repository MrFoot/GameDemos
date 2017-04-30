using UnityEngine;
using System.Collections;
using FootStudio.Framework;

public class AssetDebug : MonoBehaviour {

    public bool DebugGUI;

    public bool LoadBundle;

    public bool LoadAssetFromBundle;
    public bool LoadAssetFromResources;

    public bool UnloadAssets;
    public bool UnloadBundle;

    Test1 _t;
	// Use this for initialization
	void Start () {
        AssetManager.Initialize();
        _t = new Test1();
	}


	// Update is called once per frame
	void Update () {
        AssetManager.Update();
	}

    void OnGUI() {
        if (AssetManager.DebugInfo != DebugGUI) {
            AssetManager.DebugInfo = DebugGUI;
        }

        if (UnloadAssets)
        {
            _t.UnloadAssets();
            UnloadAssets = false;
        }

        if (UnloadBundle)
        {
            _t.UnloadBundle();
            UnloadBundle = false;
        }

        if (LoadAssetFromBundle)
        {
            _t.LoadAssetFromBundle();
            LoadAssetFromBundle = false;
        }

        if(LoadAssetFromResources)
        {
            _t.LoadAssetFromResources();
            LoadAssetFromResources = false;
        }

        if (LoadBundle)
        {
            _t.LoadBundle();
            LoadBundle = false;
        }

        AssetManager.OnGUI();
    }
}

public class Test1 {

    AssetManager.Asset cubes;
    float tBegin;
    AssetManager.Bundle _bundle;

    string objName = "Cube1";

    private void OnAssetLoaded(AssetManager.Asset asset) {
        Debug.Log(string.Format("asset : {0} , States : {1}",asset.Name,asset.State));

        cubes = asset;
        //GameObject go = GameObject.Instantiate(cubes.AssetObject) as GameObject;
        Debug.Log("CostTime  =  " + (Time.realtimeSinceStartup - tBegin) + "s");

        if (asset.State == AssetManager.State.Loaded)
        {
            GameObject.Instantiate(cubes.AssetObject);
        }

    }

    public void LoadAssetFromBundle()
    {
        tBegin = Time.realtimeSinceStartup;
        AssetManager.LoadAsyncAssetFromBundle(_bundle, objName, typeof(GameObject), OnAssetLoaded);
    }

    public void LoadAssetFromResources()
    {
        tBegin = Time.realtimeSinceStartup;
        AssetManager.LoadAssetFromResources("amberjack", typeof(GameObject), (AssetManager.Asset asset) =>
        {
            if (asset.State == AssetManager.State.Loaded)
            {
                GameObject.Instantiate(asset.AssetObject);
            }
        });
    }

    private void OnBundleLoaded(AssetManager.Bundle bundle) {
        Debug.Log(string.Format("bundle : {0} , States : {1}", bundle.Name, bundle.State));
        Debug.Log("CostTime  =  " + (Time.realtimeSinceStartup - tBegin) + "s");

        _bundle = bundle;

    }

    public void LoadBundle() {
        tBegin = Time.realtimeSinceStartup;
        //AssetManager.LoadAsyncBundle("cubes", OnBundleLoaded);


        //AssetManager.LoadBundle("StreamingAssets", (AssetManager.Bundle bundle) =>
        //{
        //    AssetManager.LoadAsyncAssetFromBundle(bundle, "AssetBundleManifest", typeof(AssetBundleManifest), (AssetManager.Asset asset) =>
        //    {
        //        AssetBundleManifest mani = asset.AssetObject as AssetBundleManifest;

        //        string[] dependName = mani.GetAllDependencies("cubes");

        //        foreach (string name in dependName)
        //        {
        //            AssetManager.LoadBundle(name, OnBundleLoaded);
        //        }

        //        AssetManager.LoadBundle("cubes", OnBundleLoaded);
        //    });

        //});

        AssetManager.LoadBundle("cubes", OnBundleLoaded);
    }

    public void UnloadBundle() {
        if (_bundle != null && _bundle.State != AssetManager.State.Invalid)
        {
            AssetManager.UnloadBundle(_bundle,false);
            Debug.Log("UnloadBundle Suc");
        }
    }

    public void UnloadAssets() {
        if (cubes != null && cubes.State != AssetManager.State.Invalid)
        {
            AssetManager.UnloadAsset(cubes);
            Debug.Log("UnloadAssets Suc");
        }
    }
}



