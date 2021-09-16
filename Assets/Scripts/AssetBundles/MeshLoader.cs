using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshLoader : MonoBehaviour
{
    [SerializeField] private AssetBundleManager manager;
    [SerializeField] private string bundleName;
    [SerializeField] private string assetName;
    [SerializeField] private string subAssetName;
    [SerializeField] private bool isMultiple = false;

    private void Start()
    {
        LoadMesh();
    }

    public void LoadMesh()
    {
        //AssetBundle bundle = manager.LoadBundle(bundleName);

        if (isMultiple)
        {
            StartCoroutine(LoadMultiple());
        }
        else
        {
            StartCoroutine(Load());
        }

    }

    private IEnumerator LoadMultiple()
    {
        AssetBundle bundle = manager.LoadBundle(bundleName);
        AssetBundleRequest req = bundle.LoadAssetWithSubAssetsAsync<Mesh>(assetName);
        
        while (!req.isDone)
        {
            yield return null;
        }
        UnityEngine.Object[] assets = req.allAssets;

        this.GetComponent<MeshFilter>().mesh = Array.Find(assets, item => item.name == subAssetName) as Mesh;

        Destroy(this);
        yield break;
    }

    private IEnumerator Load()
    {
        AssetBundle bundle = manager.LoadBundle(bundleName);

        AssetBundleRequest req = bundle.LoadAssetAsync<Mesh>(assetName);

        while (!req.isDone)
        {
            yield return null;
        }

        this.GetComponent<MeshFilter>().mesh = req.asset as Mesh;

        Destroy(this);
        yield break;
    }
}
