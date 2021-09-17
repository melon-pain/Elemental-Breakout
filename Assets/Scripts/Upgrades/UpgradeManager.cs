using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private Upgrades m_Upgrades;

    private void Awake()
    {
        m_Upgrades.Load();
    }

    private void OnDestroy()
    {
        m_Upgrades.Save();
    }

    private void OnApplicationPause(bool pause)
    {
        m_Upgrades.Save();
    }
}
