using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;

using TMPro;

public class User
{
    public int id;
    public string nickname;
    public string name;
    public string email;
    public string contact;

    public User(int _id, string _nickname, string _name, string _email, string _contact)
    {
        id = _id;
        nickname = _nickname;
        name = _name;
        email = _email;
        contact = _contact;
    }
}

public class UserProfile : MonoBehaviour
{
    //[SerializeField] private TextMeshProUGUI playersText;
    [SerializeField] private TextMeshProUGUI playerText;
    [SerializeField] private UnityEvent OnUserProfileGetAll;
    public List<User> users = new List<User>();
    public string BaseURL
    {
        get { return "https://my-user-scoreboard.herokuapp.com/api/"; }
    }

    public void OnEnable()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            GetAllPlayers();
        }
    }

    public void DeletePlayer(int id)
    {
        Debug.Log("Deleting Player");
        StartCoroutine(DeletePlayerRoutine(id));
    }

    IEnumerator DeletePlayerRoutine(int id)
    {
        UnityWebRequest request = new UnityWebRequest(BaseURL + "players/" + id, "DELETE");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        Debug.Log($"Status Code: {request.responseCode}");
        if (string.IsNullOrEmpty(request.error))
        {
            Debug.Log($"Deleted Player: {request.downloadHandler.text}");
            GetAllPlayers();
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }

    public void EditPlayer(Dictionary<string, string> PlayerParams, int playerId)
    {
        Debug.Log("Editing");
        StartCoroutine(EditPlayerRoutine(PlayerParams, playerId));
    }

    IEnumerator EditPlayerRoutine(Dictionary<string, string> PlayerParams, int playerId)
    {
        string requestString = JsonConvert.SerializeObject(PlayerParams);
        byte[] requestData = new UTF8Encoding().GetBytes(requestString);

        UnityWebRequest request = new UnityWebRequest(BaseURL + "players/" + playerId, "PUT");

        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(requestData);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        Debug.Log($"Status Code: {request.responseCode}");
        if (string.IsNullOrEmpty(request.error))
        {
            Debug.Log($"Created Player: {request.downloadHandler.text}");
            GetAllPlayers();
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
        users.Clear();

        UnityWebRequest request = new UnityWebRequest(BaseURL + "players", "GET");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        Debug.Log($"Status Code: {request.responseCode}");
        if (string.IsNullOrEmpty(request.error))
        {
            Debug.Log($"Got Players: {request.downloadHandler.text}");
            List<Dictionary<string, string>> playerListRaw =
                JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(request.downloadHandler.text);

            playerText.text = "";
            foreach (Dictionary<string, string> player in playerListRaw)
            {
                playerText.GetComponentInChildren<TextMeshProUGUI>().text +=
                    $"ID: {player["id"]}\n" +
                    $"Nickname: {player["nickname"]}\n" +
                    $"Name: {player["name"]}\n" +
                    $"Email: {player["email"]}\n" +
                    $"Contact {player["contact"]}\n\n";

                User person = new User(int.Parse(player["id"]), player["nickname"], player["name"], player["email"], player["contact"]);
                
                users.Add(person);

                //Debug.Log($"Player Nickname: {player["nickname"]}");
            }
            OnUserProfileGetAll.Invoke();
            Debug.Log($"Get all invoke");
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }

    public void CreatePlayer(Dictionary<string, string> PlayerParams)
    {
        Debug.Log("Creating Player");
        StartCoroutine(PostPlayerRoutine(PlayerParams));
    }

    IEnumerator PostPlayerRoutine(Dictionary<string, string> PlayerParams)
    {
        string requestString = JsonConvert.SerializeObject(PlayerParams);
        byte[] requestData = new UTF8Encoding().GetBytes(requestString);

        UnityWebRequest request = new UnityWebRequest(BaseURL + "players", "POST");

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

}
