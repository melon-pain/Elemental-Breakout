using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Level")]
public class Level : ScriptableObject
{
    [Header("Score")]
    [SerializeField] private int m_HighScore = 0;
    public bool isFirstTime { get => m_IsFirstTime; set => m_IsFirstTime = value; }
    [SerializeField] private bool m_IsFirstTime = true;
    public bool isCleared { get => m_IsCleared; set => m_IsCleared = value; }
    [SerializeField] private bool m_IsCleared = false;

    public void SetHighScore (int newScore)
    {
        if (newScore > m_HighScore)
            m_HighScore = newScore;
    }

    public void SetClear(bool newClear)
    {
        m_IsCleared = newClear;
    }
}
