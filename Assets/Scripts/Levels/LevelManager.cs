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
    [SerializeField] private TMP_Text m_ScoreText;

    [Header("Game Over")]
    [SerializeField] private GameObject m_LevelClearScreen;
    [SerializeField] private TMP_Text m_LevelClearText;
    [SerializeField] private TMP_Text m_TotalScoreText;
    [SerializeField] private TMP_Text m_TotalFundsText;
    [SerializeField] private GameObject m_ContinueButton;
    [SerializeField] private UnityEvent OnLevelClear;
    [SerializeField] private UnityEvent OnGameOver;

    [Header("Upgrades")]
    [SerializeField] private Upgrades upgrades;

    private void Start()
    {
        m_ScoreText.text = $"Score: <b>{level.score}</b>";
    }


    private void FixedUpdate()
    {
        m_TimeElapsed += Time.fixedDeltaTime;
    }

    public void AddScore(int amount)
    {
        level.AddScore(amount);
        m_ScoreText.text = $"Score: <b>{level.score}</b>";
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
                level.AddScore(1000);
            }

            int funds = level.score / 10;

            upgrades.AddCurrency(funds);
            m_TotalFundsText.text = funds.ToString();

            m_LevelClearText.text = "LEVEL CLEAR";
            m_ContinueButton.SetActive(true);

            OnLevelClear.Invoke();
        }
        else
        {
            level.ResetScore();
            m_LevelClearText.text = "GAME OVER";
            m_ContinueButton.SetActive(false);

            OnGameOver.Invoke();
        }

        PauseGame();
        m_LevelClearScreen.SetActive(true);
        m_TotalScoreText.text = level.score.ToString();
    }
}
