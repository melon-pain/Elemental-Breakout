using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Level level;

    [Header("Time")]
    [SerializeField] private float m_TimeElapsed;

    [Header("Score")]
    [SerializeField] private int m_levelScore;
    [SerializeField] private TMP_Text m_ScoreText;

    [Header("Level Clear")]
    [SerializeField] private GameObject m_LevelClearScreen;
    [SerializeField] private TMP_Text m_TotalScoreText;
    [SerializeField] private TMP_Text m_TotalFundsText;
    [SerializeField] private UserScoreSubmit m_UserScoreSubmit;
    [SerializeField] private UnityEvent OnLevelClear;
    [SerializeField] private UnityEvent OnGameOver;

    [Header("Game Over")]
    [SerializeField] private GameObject m_GameOverScreen;
    [SerializeField] private TMP_Text m_GameOverScoreText;

    [Header("Upgrades")]
    [SerializeField] private Upgrades upgrades;

    private void Start()
    {
        m_ScoreText.text = $"Score: <b>{level.score + m_levelScore}</b>";
    }


    private void FixedUpdate()
    {
        m_TimeElapsed += Time.fixedDeltaTime;
    }

    public void AddScore(int amount)
    {
        m_levelScore += amount;
        m_ScoreText.text = $"Score: <b>{level.score + m_levelScore}</b>";
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
                AddScore(1000);
            }

            level.AddScore(m_levelScore);

            int funds = level.score / 10;

            upgrades.AddCurrency(funds);
            m_TotalFundsText.text = funds.ToString();
            m_LevelClearScreen.SetActive(true);
            m_TotalScoreText.text = level.score.ToString();
            m_UserScoreSubmit.userScore = level.score;
            OnLevelClear.Invoke();
        }
        else
        {
            m_GameOverScreen.SetActive(true);
            m_GameOverScoreText.text = level.score.ToString();
            OnGameOver.Invoke();
        }

        m_ScoreText.text = $"Score: <b>{level.score}</b>";
        PauseGame();
    }
}
