using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Upgrades upgrades;
    [SerializeField] private string upgrade = string.Empty;

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(delegate { upgrades.Upgrade(upgrade); });
    }
}
