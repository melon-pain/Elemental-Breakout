using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> submenus = new List<GameObject>();
    //private ToggleGroup toggleGroup;
    private void Start()
    {
        //this.toggleGroup = this.GetComponent<ToggleGroup>();
    }

    public void Activate(int index)
    {
        foreach (GameObject submenu in submenus)
        {
            submenu.SetActive(false);
        }

        //int index = toggleGroup.ActiveToggles().GetEnumerator().Current.transform.GetSiblingIndex();
        submenus[index].SetActive(true);
    }
}
