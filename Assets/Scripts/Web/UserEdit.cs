using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class UserEdit : MonoBehaviour
{

    [SerializeField] private TMP_InputField[] requiredInputs;
    [SerializeField] private UserProfile userProfileScript;
    [SerializeField] private TMP_Dropdown dropDown;
    [SerializeField] private Button confirmButton;
    private bool confirmActive;
    private bool dropDownSelected;

    private void Start()
    {
        confirmButton.interactable = false;
        dropDownSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        confirmActive = true;
        for (int i = 0; i < requiredInputs.Length; i++)
        {
            if (string.IsNullOrEmpty(requiredInputs[i].text))
            {
                confirmActive = false;
            }
        }
        if(!dropDownSelected)
        {
            confirmActive = false;
        }

        if (!confirmActive && confirmButton.interactable)
        {
            confirmButton.interactable = false;
        }
        else if (confirmActive && !confirmButton.interactable)
        {
            confirmButton.interactable = true;
        }
    }

    public void EditUser()
    {
        Dictionary<string, string> PlayerParams = new Dictionary<string, string>();

        PlayerParams.Add("nickname", requiredInputs[0].text);
        PlayerParams.Add("name", requiredInputs[1].text);
        PlayerParams.Add("email", requiredInputs[2].text);
        PlayerParams.Add("contact", requiredInputs[3].text);

        userProfileScript.EditPlayer(PlayerParams, userProfileScript.users[dropDown.value].id);
    }

    public void UpdateDropDown()
    {
        dropDown.ClearOptions();
        for (int i = 0; i < userProfileScript.users.Count; i++)
        {
            TMP_Dropdown.OptionData newData = new TMP_Dropdown.OptionData();
            newData.text = userProfileScript.users[i].name;
            dropDown.options.Add(newData);
        }
        ClearFieldInput();
        dropDownSelected = false;
    }

    public void OnDropDownChange()
    {
        UpdateFieldInput();
        dropDownSelected = true;
    }

    public void UpdateFieldInput()
    {
        requiredInputs[0].text = userProfileScript.users[dropDown.value].nickname;
        requiredInputs[1].text = userProfileScript.users[dropDown.value].name;
        requiredInputs[2].text = userProfileScript.users[dropDown.value].email;
        requiredInputs[3].text = userProfileScript.users[dropDown.value].contact;
    }

    public void ClearFieldInput()
    {
        for (int i = 0; i < requiredInputs.Length; i++)
        {
            requiredInputs[i].text = "";
        }
    }

}
