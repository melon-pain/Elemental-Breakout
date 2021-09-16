using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class UserDelete : MonoBehaviour
{
    [SerializeField] private TMP_InputField[] displayInputs;
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

        if (!dropDownSelected)
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

    public void DeleteUser()
    {
        Debug.Log("Delete ID: " + userProfileScript.users[dropDown.value]);
        userProfileScript.DeletePlayer(userProfileScript.users[dropDown.value].id);
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
        displayInputs[0].text = userProfileScript.users[dropDown.value].nickname;
        displayInputs[1].text = userProfileScript.users[dropDown.value].name;
        displayInputs[2].text = userProfileScript.users[dropDown.value].email;
        displayInputs[3].text = userProfileScript.users[dropDown.value].contact;
    }

    public void ClearFieldInput()
    {
        for (int i = 0; i < displayInputs.Length; i++)
        {
            displayInputs[i].text = "";
        }
    }

}