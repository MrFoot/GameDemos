using UnityEditor;
using UnityEngine;

public class CreateAssetBundles
{
    [MenuItem("AssetBundles/Build AssetBundles %I")]
    static void BuildAllAssetBundles() {
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.None, BuildTarget.StandaloneOSXUniversal);

        Debug.Log("Build Suc");
    }
}

public class GetAssetBundleNames
{
    [MenuItem("AssetBundles/Get AssetBundle names")]
    static void GetNames ()
    {
        var names = AssetDatabase.GetAllAssetBundleNames();
        foreach (var name in names)
            Debug.Log ("AssetBundle: " + name);
    }
}