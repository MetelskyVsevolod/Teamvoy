using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("AssetBundles", BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows64);
        //BuildPipeline.BuildAssetBundles ("AssetBundles", BuildAssetBundleOptions.None, BuildTarget.Android);
    }
}