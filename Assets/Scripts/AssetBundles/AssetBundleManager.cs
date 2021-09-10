using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Asset Bundle Manager")]
public class AssetBundleManager : ScriptableObject
{
    private string bundlesRootPath
    {
        get
        {
        #if UNITY_EDITOR
            return Application.streamingAssetsPath;
        #elif UNITY_ANDROID
            return Application.persistentDataPath;
        #endif
        }
    }

    private Dictionary<string, AssetBundle> loadedBundles = new Dictionary<string, AssetBundle>();

    public AssetBundle LoadBundle(string bundleName)
    {
        if (loadedBundles.ContainsKey(bundleName))
        {
            return loadedBundles[bundleName];
        }
        AssetBundle ret = AssetBundle.LoadFromFile(Path.Combine(bundlesRootPath, bundleName));

        if (ret != null)
            loadedBundles.Add(bundleName, ret);

        return ret;
    }

    public T GetAsset<T>(string bundleName, string assetName) where T : Object
    {
        T ret = null;

        AssetBundle bundle = LoadBundle(bundleName);

        if (bundle != null)
        {
            ret = bundle.LoadAsset<T>(assetName);
        }

        return ret;
    }
}
