using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [ReadOnlyInInspector] public float progress = 0.0f;
    public UnityEvent OnLoadSceneStart = new UnityEvent();
    public UnityEvent<float> OnLoadSceneProgress = new UnityEvent<float>();
    public UnityEvent OnLoadSceneFinished = new UnityEvent();

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
        if (OnLoadSceneStart.GetPersistentEventCount() > 0)
            OnLoadSceneStart.Invoke();
        while (operation.progress < 0.9f)
        {
            this.progress = operation.progress;

            if (OnLoadSceneProgress.GetPersistentEventCount() > 0)
                OnLoadSceneProgress.Invoke(this.progress);
            yield return new WaitForEndOfFrame();
        }

        OnLoadSceneFinished.Invoke();

        operation.allowSceneActivation = true;
    }

    private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode mode)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, mode);
        operation.allowSceneActivation = false;
        if (OnLoadSceneStart.GetPersistentEventCount() > 0)
            OnLoadSceneStart.Invoke();
        while (operation.progress < 0.9f)
        {
            this.progress = operation.progress;

            if (OnLoadSceneProgress.GetPersistentEventCount() > 0)
                OnLoadSceneProgress.Invoke(this.progress);
            yield return new WaitForEndOfFrame();
        }

        OnLoadSceneFinished.Invoke();

        operation.allowSceneActivation = true;
    }
}
