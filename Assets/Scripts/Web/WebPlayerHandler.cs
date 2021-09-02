using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;

public class WebPlayerHandler : MonoBehaviour
{
    public string BaseURL
    {
        get { return "https://my-user-scoreboard.herokuapp.com/api/"; }
    }

    public void DeletePlayer()
    {
        Debug.Log("Deleting Player");
        StartCoroutine(DeletePlayerRoutine());
    }

    IEnumerator DeletePlayerRoutine()
    {
        UnityWebRequest request = new UnityWebRequest(BaseURL + "players/28", "DELETE");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        Debug.Log($"Status Code: {request.responseCode}");
        if (string.IsNullOrEmpty(request.error))
        {
            Debug.Log($"Deleted Player: {request.downloadHandler.text}");
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }

    public void EditPlayer()
    {
        Debug.Log("Editing");
        StartCoroutine(EditPlayerRoutine());
    }

    IEnumerator EditPlayerRoutine()
    {
        Dictionary<string, string> PlayerParams = new Dictionary<string, string>();

        PlayerParams.Add("name", "John Adriel Santos");
        PlayerParams.Add("email", "john_adriel_santos@dlsu.edu.ph");

        string requestString = JsonConvert.SerializeObject(PlayerParams);
        byte[] requestData = new UTF8Encoding().GetBytes(requestString);

        UnityWebRequest request = new UnityWebRequest(BaseURL + "players/28", "PUT");

        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(requestData);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        Debug.Log($"Status Code: {request.responseCode}");
        if (string.IsNullOrEmpty(request.error))
        {
            Debug.Log($"Created Player: {request.downloadHandler.text}");
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }

    public void GetPlayer()
    {
        Debug.Log("Getting player");
        StartCoroutine(GetPlayerRoutine());
    }

    IEnumerator GetPlayerRoutine()
    {
        UnityWebRequest request = new UnityWebRequest(BaseURL + "players/28", "GET");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        Debug.Log($"Status Code: {request.responseCode}");
        if (string.IsNullOrEmpty(request.error))
        {
            Debug.Log($"Got Player: {request.downloadHandler.text}");
            Dictionary<string, string> playerRaw =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(request.downloadHandler.text);

            Debug.Log($"Player Nickname {playerRaw["nickname"]}");
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }

    public void GetAllPlayers()
    {
        Debug.Log("Getting players");
        StartCoroutine(GetAllPlayersRoutine());
    }

    IEnumerator GetAllPlayersRoutine()
    {
        UnityWebRequest request = new UnityWebRequest(BaseURL + "players", "GET");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        Debug.Log($"Status Code: {request.responseCode}");
        if (string.IsNullOrEmpty(request.error))
        {
            Debug.Log($"Got Players: {request.downloadHandler.text}");
            List<Dictionary<string, string>> playerListRaw =
                JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(request.downloadHandler.text);

            foreach(Dictionary<string, string> player in playerListRaw)
            {
                Debug.Log($"Player Nickname: {player["nickname"]}");
            }
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }

    public void CreatePlayer()
    {
        Debug.Log("Creating Player");
        StartCoroutine(PostPlayerRoutine());
    }

    IEnumerator PostPlayerRoutine()
    {
        Dictionary<string, string> PlayerParams = new Dictionary<string, string>();

        PlayerParams.Add("nickname", "Garfield");
        PlayerParams.Add("name", "James Gabriel Omalin");
        PlayerParams.Add("email", "james_gabriel_omalin@dlsu.edu.ph");
        PlayerParams.Add("contact", "09215128712");

        string requestString = JsonConvert.SerializeObject(PlayerParams);
        byte[] requestData = new UTF8Encoding().GetBytes(requestString);

        UnityWebRequest request = new UnityWebRequest(BaseURL + "players", "POST");

        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(requestData);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        Debug.Log($"Status Code: {request.responseCode}");
        if(string.IsNullOrEmpty(request.error))
        {
            Debug.Log($"Created Player: {request.downloadHandler.text}");
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }
}
