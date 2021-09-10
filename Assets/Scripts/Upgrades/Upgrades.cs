using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    #region Stat
    public float baseValue { get { return m_baseValue; } }
    [SerializeField] private float m_baseValue = 0.0f;
    public float additionalValue { get { return m_additionalValue; } }
    [SerializeField] private float m_additionalValue = 100.0f;
    #endregion Stat

    #region Upgrade
    public int upgradeLevel { get { return m_upgradeLevel; } }
    [SerializeField] private int m_upgradeLevel = 1;
    public int maxUpgradeLevel { get { return m_maxUpgradeLevel; } }
    [SerializeField] private int m_maxUpgradeLevel = 5;
    #endregion Upgrade

    #region Cost
    public int baseUpgradeCost { get { return m_baseUpgradeCost; } }
    [SerializeField] private int m_baseUpgradeCost = 100;
    public int additionalUpgradeCost { get { return m_additionalUpgradeCost; } }
    [SerializeField] private int m_additionalUpgradeCost = 100;
    #endregion Cost

    public int GetUpgradeCost()
    {
        return this.m_baseUpgradeCost + (this.m_upgradeLevel * this.m_additionalUpgradeCost);
    }

    public void Upgrade()
    {
        this.m_upgradeLevel = Mathf.Min(this.m_upgradeLevel + 1, this.m_maxUpgradeLevel);
    }

    public bool CanBeUpgraded()
    {
        return this.m_upgradeLevel < this.m_maxUpgradeLevel;
    }

    public float GetValue()
    {
        return this.m_baseValue + (this.m_additionalValue * this.m_upgradeLevel);
    }
}

[System.Serializable][CreateAssetMenu(fileName = "Upgrades")]
public class Upgrades : ScriptableObject
{
    #region Funds
    public int funds { get => m_Funds; }
    [SerializeField, Min(0)] private int m_Funds = 0;
    #endregion Funds

    #region Player 
    public Stat maxHP { get => m_maxHP; }
    [SerializeField] private Stat m_maxHP = new Stat();
    public Stat maxSP { get => m_maxSP; }
    [SerializeField] private Stat m_maxSP = new Stat();
    public Stat maxMP { get => m_maxMP; }
    [SerializeField] private Stat m_maxMP = new Stat();
    #endregion Player

    #region Projectile
    public Stat projectileDMG { get => m_projectileDMG; }
    [SerializeField] private Stat m_projectileDMG = new Stat();
    public Stat projectileCost { get => m_projectileCost; }
    [SerializeField] private Stat m_projectileCost = new Stat();
    public Stat projectileRate { get => m_projectileRate; }
    [SerializeField] private Stat m_projectileRate = new Stat();
    #endregion Projectile

    #region Beam
    public Stat beamDMG { get => m_beamDMG; }
    [SerializeField] private Stat m_beamDMG = new Stat();
    public Stat beamRate { get => m_beamRate; }
    [SerializeField] private Stat m_beamRate = new Stat();
    public Stat beamCost { get => m_beamCost; }
    [SerializeField] private Stat m_beamCost = new Stat();
    #endregion Beam

    public void AddCurrency(int amount)
    {
        this.m_Funds += amount;
    }

    public void LoseCurrency(int amount)
    {
        this.m_Funds = Mathf.Max(0, this.m_Funds - amount);
    }

    public void Upgrade(string stat)
    {
        Stat upgrade = GetType().GetProperty(stat).GetValue(this, null) as Stat;

        if (!upgrade.CanBeUpgraded() || upgrade.GetUpgradeCost() > funds)
        {
            return;
        }

        this.LoseCurrency(upgrade.GetUpgradeCost());
        upgrade.Upgrade();
    }

}
