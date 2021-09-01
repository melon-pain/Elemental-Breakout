using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Level level;

    [Header("Time")]
    [SerializeField] private float m_TimeElapsed;

    [Header("Score")]
    [SerializeField] private int m_Score = 0;
    [SerializeField] private TMP_Text m_ScoreText;

    [Header("Game Over")]
    [SerializeField] private GameObject m_GameOverScreen;
    [SerializeField] private TMP_Text m_TotalScoreText;
    [SerializeField] private TMP_Text m_TotalFundsText;

    [Header("Upgrades")]
    [SerializeField] private Upgrades upgrades;

    private void Start()
    {
        if (level.isFirstTime)
        {
            ShowTutorial();
        }
    }

    private void FixedUpdate()
    {
        m_TimeElapsed += Time.fixedDeltaTime;
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
            if (m_TimeElapsed < 240.0f)
            {
                m_Score += 1000;
            }

            level.SetHighScore(m_Score);
            level.isFirstTime = false;
            level.isCleared = true;

            int funds = m_Score / 10;
            upgrades.AddCurrency(funds);
            m_TotalFundsText.text = funds.ToString();
        }

        PauseGame();
        m_GameOverScreen.SetActive(true);
        m_TotalScoreText.text = m_Score.ToString();
    }

}
