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

    public void Awake()
    {
        loadedBundles.Clear();
        Debug.Log("AWAKE");
    }

    public AssetBundle LoadBundle(string bundleName)
    {
        if (loadedBundles.ContainsKey(bundleName))
            return loadedBundles[bundleName];
        else
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(bundlesRootPath, bundleName));
            if (bundle)
            {
                loadedBundles.Add(bundleName, bundle);
                return bundle;
            }
            else
                return null;
        }
    }

    public void UnloadBundle(string bundleName)
    {
        loadedBundles[bundleName].Unload(false);
        loadedBundles.Remove(bundleName);
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

    public T[] GetAssetsWithSubAssets<T>(string bundleName, string assetName) where T : Object
    {
        T[] ret = null;

        AssetBundle bundle = LoadBundle(bundleName);

        if (bundle != null)
        {
            ret =  bundle.LoadAssetWithSubAssets<T>(assetName);
        }

        return ret;
    }
}
