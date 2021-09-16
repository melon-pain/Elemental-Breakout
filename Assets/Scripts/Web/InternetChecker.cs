using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InternetChecker : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private Toggle[] toggles;
    [SerializeField] private GameObject[] panels;

    // Update is called once per frame
    void Update()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if(!buttons[i].IsInteractable())
                {
                    buttons[i].interactable = true;
                }
            }

            for (int i = 0; i < toggles.Length; i++)
            {
                if(!toggles[i].IsInteractable())
                {
                    toggles[i].interactable = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].IsInteractable())
                {
                    buttons[i].interactable = false;
                }
            }

            for (int i = 0; i < toggles.Length; i++)
            {
                if(toggles[i].IsInteractable())
                {
                    toggles[i].interactable = false;
                }
            }

            for (int i = 0; i < panels.Length; i++)
            {
                if(panels[i].activeSelf)
                {
                    panels[i].SetActive(false);
                }
            }
        }
    }
}
