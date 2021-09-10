using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    private List<UpgradeMenuItem> items;

    private void Start()
    {
        items = new List<UpgradeMenuItem>(GetComponentsInChildren<UpgradeMenuItem>());
    }
    public void UpdateAllItems()
    {
        foreach(UpgradeMenuItem item in items)
        {
            item.UpdateItem();
        }
    }
}
