using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;

public class WebScoreHandler : MonoBehaviour
{
    public string BaseURL
    {
        get { return "https://my-user-scoreboard.herokuapp.com/api/"; }
    }

    public void GetAllScores()
    {
        Debug.Log("Getting scores");
        StartCoroutine(GetAllScoresRoutine());
    }

    IEnumerator GetAllScoresRoutine()
    {
        UnityWebRequest request = new UnityWebRequest(BaseURL + "get_scores/4", "GET");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        Debug.Log($"Status Code: {request.responseCode}");
        if (string.IsNullOrEmpty(request.error))
        {
            Debug.Log($"Got Scores: {request.downloadHandler.text}");
            List<Dictionary<string, string>> scoreListRaw =
                JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(request.downloadHandler.text);

            foreach (Dictionary<string, string> score in scoreListRaw)
            {
                Debug.Log($"Name: {score["user_name"]}");
                Debug.Log($"Score: {score["score"]}");
            }
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
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
        ScoreParams.Add("user_name", "Ian");
        ScoreParams.Add("score", "15");

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
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }
}
