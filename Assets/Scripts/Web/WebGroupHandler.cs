using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;

public class WebGroupHandler : MonoBehaviour
{
    public string BaseURL
    {
        get { return "https://my-user-scoreboard.herokuapp.com/api/"; }
    }

    public void EditGroup()
    {
        Debug.Log("Editing");
        StartCoroutine(EditGroupRoutine());
    }

    IEnumerator EditGroupRoutine()
    {
        Dictionary<string, string> GroupParams = new Dictionary<string, string>();

        GroupParams.Add("group_name", "Horse Power");

        string requestString = JsonConvert.SerializeObject(GroupParams);
        byte[] requestData = new UTF8Encoding().GetBytes(requestString);

        UnityWebRequest request = new UnityWebRequest(BaseURL + "groups/4", "PUT");

        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(requestData);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        Debug.Log($"Status Code: {request.responseCode}");
        if (string.IsNullOrEmpty(request.error))
        {
            Debug.Log($"Created Group: {request.downloadHandler.text}");
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }

    public void GetGroup()
    {
        Debug.Log("Getting Group");
        StartCoroutine(GetGroupRoutine());
    }

    IEnumerator GetGroupRoutine()
    {
        UnityWebRequest request = new UnityWebRequest(BaseURL + "groups/4", "GET");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        Debug.Log($"Status Code: {request.responseCode}");
        if (string.IsNullOrEmpty(request.error))
        {
            Debug.Log($"Got Group: {request.downloadHandler.text}");
            Dictionary<string, string> groupRaw =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(request.downloadHandler.text);

            Debug.Log($"Group Name {groupRaw["group_name"]}");
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }

    public void GetAllGroups()
    {
        Debug.Log("Getting groups");
        StartCoroutine(GetAllGroupsRoutine());
    }

    IEnumerator GetAllGroupsRoutine()
    {
        UnityWebRequest request = new UnityWebRequest(BaseURL + "groups", "GET");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        Debug.Log($"Status Code: {request.responseCode}");
        if (string.IsNullOrEmpty(request.error))
        {
            Debug.Log($"Got Groups: {request.downloadHandler.text}");
            List<Dictionary<string, string>> groupListRaw =
                JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(request.downloadHandler.text);

            foreach (Dictionary<string, string> group in groupListRaw)
            {
                Debug.Log($"Group Name: {group["group_name"]}");
            }
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }

    public void CreateGroup()
    {
        Debug.Log("Creating Group");
        StartCoroutine(PostGroupRoutine());
    }

    IEnumerator PostGroupRoutine()
    {
        Dictionary<string, string> GroupParams = new Dictionary<string, string>();

        GroupParams.Add("group_num", "4");
        GroupParams.Add("group_name", "Horse Power");
        GroupParams.Add("game_name", "Elemental Breakout");

        string requestString = JsonConvert.SerializeObject(GroupParams);
        byte[] requestData = new UTF8Encoding().GetBytes(requestString);

        UnityWebRequest request = new UnityWebRequest(BaseURL + "groups", "POST");

        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(requestData);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        Debug.Log($"Status Code: {request.responseCode}");
        if (string.IsNullOrEmpty(request.error))
        {
            Debug.Log($"Created Group: {request.downloadHandler.text}");
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }
}
