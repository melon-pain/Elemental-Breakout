using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private List<UpgradeMenuItem> items;

    public void UpdateAllItems()
    {
        foreach(UpgradeMenuItem item in items)
        {
            if(item.gameObject.activeSelf)
            {
                item.UpdateItem();
            }
        }
    }
}
