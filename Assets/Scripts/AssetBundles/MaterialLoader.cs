using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialLoader : MonoBehaviour
{
    [SerializeField] private AssetBundleManager manager;
    [SerializeField] private string textureBundleName;
    [SerializeField] private string materialBundleName;
    [SerializeField] private string assetName;
    [SerializeField] private string shaderName = "Universal Render Pipeline/Lit";

    [SerializeField] private List<string> assetNames = new List<string>();
    [SerializeField] private bool isMultiple = false;

    private void Start()
    {
        LoadMaterial();;
    }

    public void LoadMaterial()
    {
        if (isMultiple)
            StartCoroutine(LoadMultiple());
        else
            StartCoroutine(Load());
    }

    private IEnumerator LoadMultiple()
    {
        AssetBundle textureBundle = manager.LoadBundle(textureBundleName);
        AssetBundle materialBundle = manager.LoadBundle(materialBundleName);

        AssetBundleRequest textureReq = textureBundle.LoadAllAssetsAsync();

        while (!textureReq.isDone)
        {
            yield return null;
        }
        Material[] mats = new Material[assetNames.Count];
        for (int i = 0; i < assetNames.Count; i++)
        {
            AssetBundleRequest materialReq = materialBundle.LoadAssetAsync<Material>(assetNames[i]);
            while (!materialReq.isDone)
            {
                yield return null;
            }
            Material mat = materialReq.asset as Material;
            Debug.Log(mat);
            mat.shader = Shader.Find(shaderName);
            mats[i] = mat;
        }

        this.GetComponent<MeshRenderer>().materials = mats;
        yield break;
    }

    private IEnumerator Load()
    {
        AssetBundle textureBundle = manager.LoadBundle(textureBundleName);
        AssetBundle materialBundle = manager.LoadBundle(materialBundleName);

        AssetBundleRequest textureReq = textureBundle.LoadAllAssetsAsync();
        AssetBundleRequest materialReq = materialBundle.LoadAssetAsync<Material>(assetName);

        while (!(textureReq.isDone && materialReq.isDone))
        {
            yield return null;
        }


        Material mat = materialReq.asset as Material;
        mat.shader = Shader.Find(shaderName);
        this.GetComponent<MeshRenderer>().material = mat;

        yield break;
    }
}
