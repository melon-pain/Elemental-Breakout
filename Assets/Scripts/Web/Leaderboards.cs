using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;

using TMPro;

public class Leaderboards : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playersText;
    [SerializeField] private UnityEvent OnLeaderboardOpen;
    [SerializeField] private UnityEvent OnLeaderboardClose;

    public string BaseURL
    {
        get { return "https://my-user-scoreboard.herokuapp.com/api/"; }
    }

    public void OnEnable()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            OnLeaderboardOpen.Invoke();
            GetAllScores();
        }
    }

    public void OnDisable()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            OnLeaderboardClose.Invoke();
        }
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
            int standing = 1;
            playersText.text = "";
            Debug.Log($"Got Scores: {request.downloadHandler.text}");
            List<Dictionary<string, string>> scoreListRaw =
                JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(request.downloadHandler.text);

            foreach (Dictionary<string, string> score in scoreListRaw)
            {
                playersText.text += $"{standing}. {score["user_name"]} - {score["score"]}\n";
                standing++;
                Debug.Log($"Name: {score["user_name"]}");
                Debug.Log($"Score: {score["score"]}");
            }
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }
}
