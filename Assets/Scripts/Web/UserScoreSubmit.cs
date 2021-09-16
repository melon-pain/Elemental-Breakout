using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class UserScoreSubmit : MonoBehaviour
{
    [SerializeField] private TMP_InputField requiredInput;
    [SerializeField] private Button submitButton;
    private bool submitActive;

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
}
