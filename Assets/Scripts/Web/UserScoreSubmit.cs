using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;

public class UserScoreSubmit : MonoBehaviour
{
    public string BaseURL
    {
        get { return "https://my-user-scoreboard.herokuapp.com/api/"; }
    }

    [SerializeField] private TMP_InputField requiredInput;
    [SerializeField] private Button submitButton;
    private bool submitActive;

    public int userScore = 0;

    private void Start()
    {
        submitButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(requiredInput.gameObject.activeSelf && submitButton.gameObject.activeSelf)
        {
            submitActive = true;
            if (string.IsNullOrEmpty(requiredInput.text))
            {
                submitActive = false;
            }
            if (!submitActive && submitButton.interactable)
            {
                submitButton.interactable = false;
            }
            else if (submitActive && !submitButton.interactable)
            {
                submitButton.interactable = true;
            }
        }
    }

    public void CreateScore()
    {
        Debug.Log("Creating Score");
        StartCoroutine(PostScoreRoutine());
    }

    IEnumerator PostScoreRoutine()
    {
        Dictionary<string, string> ScoreParams = new Dictionary<string, string>();

        ScoreParams.Add("group_num", "4");
        ScoreParams.Add("user_name", requiredInput.text);
        ScoreParams.Add("score", userScore.ToString());

        string requestString = JsonConvert.SerializeObject(ScoreParams);
        byte[] requestData = new UTF8Encoding().GetBytes(requestString);

        UnityWebRequest request = new UnityWebRequest(BaseURL + "scores", "POST");

        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(requestData);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        Debug.Log($"Status Code: {request.responseCode}");
        if (string.IsNullOrEmpty(request.error))
        {
            Debug.Log($"Created Score: {request.downloadHandler.text}");
            requiredInput.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }

}
