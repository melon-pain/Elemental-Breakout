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

    }

    public AssetBundle LoadBundle(string bundleName)
    {
        if (loadedBundles.ContainsKey(bundleName))
        {
            return loadedBundles[bundleName];
        }
        else
        {
            Debug.Log(loadedBundles.Keys.Count);
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

    public void UnloadAllBundles()
    {
        AssetBundle.UnloadAllAssetBundles(true);
    }
}
