using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [ReadOnlyInInspector] public float progress = 0.0f;
    public UnityEvent<float> OnLoadSceneProgress = new UnityEvent<float>();

    public void LoadSceneSingle(int sceneBuildIndex)
    {
        if (sceneBuildIndex < 0 || sceneBuildIndex > SceneManager.sceneCount - 1)
        {
            Debug.LogError("Invalid index");
            return;
        }    
        StartCoroutine(LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Single));
    }

    public void LoadSceneSingle(string sceneName)
    {
        Debug.Log("OK");
        if (sceneName == string.Empty)
        {
            Debug.LogError($"Invalid name {sceneName == string.Empty}");
            return;
        }
        StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Single));
    }

    public void LoadSceneAdditive(int sceneBuildIndex)
    {
        if (sceneBuildIndex < 0 || sceneBuildIndex > SceneManager.sceneCount - 1)
        {
            Debug.LogError("Invalid index");
            return;
        }
        StartCoroutine(LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Additive));
    }

    public void LoadSceneAdditive(string sceneName)
    {
        if (sceneName == string.Empty)
        {
            Debug.LogError($"Invalid name {sceneName == string.Empty}");
            return;
        }
        StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Additive));
    }

    private IEnumerator LoadSceneAsync(int sceneBuildIndex, LoadSceneMode mode)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneBuildIndex, mode);

        while (!operation.isDone)
        {
            this.progress = operation.progress;
            if (OnLoadSceneProgress != null)
                OnLoadSceneProgress.Invoke(this.progress);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode mode)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, mode);

        while (!operation.isDone)
        {
            yield return new WaitForSeconds(1f);
            this.progress = operation.progress;

            if (OnLoadSceneProgress != null)
                OnLoadSceneProgress.Invoke(this.progress);
            yield return new WaitForEndOfFrame();
        }
    }
}
