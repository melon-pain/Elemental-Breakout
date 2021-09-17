using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBGM : MonoBehaviour
{
    [SerializeField] private AssetBundleManager manager;
    [SerializeField] private string assetName;
    [SerializeField] private string bossAssetName;
    [SerializeField] private string gameOverAssetName;
    [SerializeField] private string clearAssetName;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        StartCoroutine(Load());
    }

    public void LoadBossBGM()
    {
        StartCoroutine(LoadBoss());
    }

    public void LoadClearBGM()
    {
        StartCoroutine(LoadClear());
    }

    public void LoadGameOverBGM()
    {
        StartCoroutine(LoadGameOver());
    }

    private IEnumerator Load()
    {
        AssetBundle bundle = manager.LoadBundle("bgmbundle");
        AssetBundleRequest req = bundle.LoadAssetAsync<AudioClip>(assetName);

        while (!req.isDone)
        {
            yield return null;
        }

        audioSource.clip = req.asset as AudioClip;
        audioSource.Play();
        yield break;
    }

    private IEnumerator LoadBoss()
    {
        AssetBundle bundle = manager.LoadBundle("bgmbundle");
        AssetBundleRequest req = bundle.LoadAssetAsync<AudioClip>(bossAssetName);

        while (!req.isDone)
        {
            yield return null;
        }

        audioSource.clip = req.asset as AudioClip;
        audioSource.Play();
        yield break;
    }

    private IEnumerator LoadClear()
    {
        AssetBundle bundle = manager.LoadBundle("bgmbundle");
        AssetBundleRequest req = bundle.LoadAssetAsync<AudioClip>(clearAssetName);

        while (!req.isDone)
        {
            yield return null;
        }

        audioSource.clip = req.asset as AudioClip;
        audioSource.Play();
        yield break;
    }

    private IEnumerator LoadGameOver()
    {
        AssetBundle bundle = manager.LoadBundle("bgmbundle");
        AssetBundleRequest req = bundle.LoadAssetAsync<AudioClip>(gameOverAssetName);

        while (!req.isDone)
        {
            yield return null;
        }

        audioSource.clip = req.asset as AudioClip;
        audioSource.Play();
        yield break;
    }
}