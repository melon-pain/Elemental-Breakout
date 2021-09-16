using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Level")]
public class Level : ScriptableObject
{
    [SerializeField] private int currentLevel = 1;
    [Header("Score")]
    [SerializeField] private int m_Score = 0;
    public int score { get => m_Score; }

    public void AddScore(int amount)
    {
        m_Score += amount;
    }

    public void ResetScore()
    {
        m_Score = 0;
    }

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }
}
