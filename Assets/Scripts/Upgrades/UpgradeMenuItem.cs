using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeMenuItem : MonoBehaviour
{
    [SerializeField] private Upgrades upgrades;
    [SerializeField] private string m_UpgradeStat = string.Empty;
    [Header("UI")]
    [SerializeField] private SegmentedBar m_UpgradeBar;
    [SerializeField] private Button m_UpgradeButton;
    [SerializeField] private TMP_Text m_CostText;
    [SerializeField] private TMP_Text m_FundsText;
    private Stat stat;

    private void Start()
    {
        stat = upgrades.GetType().GetProperty(m_UpgradeStat).GetValue(upgrades, null) as Stat;
        m_UpgradeBar.SetSegments(stat.maxUpgradeLevel);
        m_UpgradeButton.onClick.AddListener(delegate { upgrades.Upgrade(m_UpgradeStat); });
        m_UpgradeButton.onClick.AddListener(delegate { this.UpdateItem(); });
        UpdateItem();
    }

    public void UpdateItem()
    {
        m_UpgradeBar.UpdateBar((float)stat.upgradeLevel / stat.maxUpgradeLevel);
        m_CostText.text = stat.GetUpgradeCost().ToString() + "G";
        m_UpgradeButton.interactable = stat.CanBeUpgraded() && upgrades.currency >= stat.GetUpgradeCost();
        m_FundsText.text = $"Funds: <b>{upgrades.currency}G</b>";
    }
}
