using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Cheats")]
public class Cheats : ScriptableObject
{
    [SerializeField] private bool m_InfiniteHP = false;
    public bool infiniteHP { get => m_InfiniteHP; }
    [SerializeField] private bool m_InfiniteMP = false;
    public bool infiniteMP { get => m_InfiniteMP; }

    public void InfiniteHP (bool value)
    {
        m_InfiniteHP = value;
    }

    public void InfiniteMP(bool value)
    {
        m_InfiniteHP = value;
    }
}
