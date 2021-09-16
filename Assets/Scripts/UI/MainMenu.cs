using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> submenus = new List<GameObject>();
    private void Start()
    {
    }

    public void Activate(int index)
    {
        foreach (GameObject submenu in submenus)
        {
            submenu.SetActive(false);
        }

        submenus[index].SetActive(true);
    }
}
