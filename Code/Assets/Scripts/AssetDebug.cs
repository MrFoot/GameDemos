using UnityEngine;
using System.Collections;
using Soulgame.Asset;

public class AssetDebug : MonoBehaviour {

    public bool DebugGUI;

    public bool LoadBundle;
    public bool LoadAsset;

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

        if (LoadAsset)
        {
            _t.LoadAsset();
            LoadAsset = false;
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
    private void OnAssetLoaded(AssetManager.Asset asset) {
        Debug.Log(string.Format("asset : {0} , States : {1}",asset.Name,asset.State));

        cubes = asset;
        //GameObject go = GameObject.Instantiate(cubes.AssetObject) as GameObject;
        Debug.Log("CostTime  =  " + (Time.realtimeSinceStartup - tBegin) + "s");

        if (asset.State == AssetManager.State.Loaded)
        {
        }

    }

    private void OnBundleLoaded(AssetManager.Bundle bundle) {
        Debug.Log(string.Format("bundle : {0} , States : {1}", bundle.Name, bundle.State));
        Debug.Log("CostTime  =  " + (Time.realtimeSinceStartup - tBegin) + "s");

        _bundle = bundle;
        /*
        if (bundle.State == AssetManager.State.Loaded)
        {
            tBegin = Time.realtimeSinceStartup;
            AssetManager.LoadAssetFromBundle(bundle, "Cube1", typeof(GameObject), OnAssetLoaded);
        }
        */
    }

    public void LoadAsset() {
        tBegin = Time.realtimeSinceStartup;
        //AssetManager.LoadAsyncAssetFromResources("Prefabs/Cube1", typeof(GameObject), OnAssetLoaded);
        AssetManager.LoadAsyncAssetFromBundle(_bundle, "Cube1", typeof(GameObject), OnAssetLoaded);
    }

    public void LoadBundle() {
        tBegin = Time.realtimeSinceStartup;
        AssetManager.LoadAsyncBundle("cubes", OnBundleLoaded);
        //AssetManager.LoadBundle("cubes", OnBundleLoaded);
    }

    public void UnloadBundle() {
        if (_bundle != null && _bundle.State != AssetManager.State.Invalid)
            AssetManager.UnloadBundle(_bundle);
    }

    public void UnloadAssets() {
        if (cubes != null && cubes.State != AssetManager.State.Invalid)
            AssetManager.UnloadAsset(cubes);
    }
}



