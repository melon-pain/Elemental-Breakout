using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private Upgrades m_Upgrades;

    private void Awake()
    {
        string filePath = Application.persistentDataPath + "/upgrades.json";
        if (File.Exists(filePath))
        {
            Debug.Log("File exists!");
            JsonUtility.FromJsonOverwrite(File.ReadAllText(filePath), m_Upgrades);
        }
    }

    private void OnDestroy()
    {
        string filePath = Application.persistentDataPath + "/upgrades.json";
        File.WriteAllText(filePath, JsonUtility.ToJson(m_Upgrades));
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            string filePath = Application.persistentDataPath + "/upgrades.json";
            File.WriteAllText(filePath, JsonUtility.ToJson(m_Upgrades));
        }
    }
}
