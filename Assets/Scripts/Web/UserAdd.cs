using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class UserAdd : MonoBehaviour
{
    [SerializeField] private TMP_InputField[] requiredInputs;
    [SerializeField] private UserProfile userProfileScript;
    [SerializeField] private Button confirmButton;
    private bool confirmActive;

    // Update is called once per frame
    void Update()
    {
        confirmActive = true;
        for (int i = 0; i < requiredInputs.Length; i++)
        {
            if(string.IsNullOrEmpty(requiredInputs[i].text))
            {
                confirmActive = false;
            }
        }
        if(!confirmActive && confirmButton.interactable)
        {
            confirmButton.interactable = false;
        }
        else if(confirmActive && !confirmButton.interactable)
        {
            confirmButton.interactable = true;
        }
    }

    public void AddUser()
    {
        Dictionary<string, string> PlayerParams = new Dictionary<string, string>();

        PlayerParams.Add("nickname", requiredInputs[0].text);
        PlayerParams.Add("name", requiredInputs[1].text);
        PlayerParams.Add("email", requiredInputs[2].text);
        PlayerParams.Add("contact", requiredInputs[3].text);

        userProfileScript.CreatePlayer(PlayerParams);
    }
}
