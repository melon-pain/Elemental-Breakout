using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Level level;

    [Header("Score")]
    [SerializeField] private int m_Score = 0;
    [SerializeField] private TMP_Text m_ScoreText;

    [Header("Game Over")]
    [SerializeField] private GameObject m_GameOverScreen;

    private void Start()
    {
        if (level.isFirstTime)
        {
            ShowTutorial();
        }
    }

    private void ShowTutorial()
    {

    }

    public void AddScore(int amount)
    {
        m_Score += amount;
        m_ScoreText.text = $"Score: <b>{m_Score}</b>";
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1.0f;
    }

    public void GameOver(bool isFinished)
    {
        if (isFinished)
        {
            level.SetHighScore(m_Score);
            level.isFirstTime = false;
            level.isCleared = true;
        }
    }

}
