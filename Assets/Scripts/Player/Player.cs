using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour, IDamage
{
    #region Upgrades
    [SerializeField] private Upgrades upgrades;
    #endregion Upgrades

    #region Player Stats
    [Header("HP")]
    #region HP
    [Tooltip("The maximum amount of Health Points the player can have")]
    [SerializeField] private float m_MaxHP;
    [Tooltip("The amount of Health Points the player currently has")]
    [SerializeField] private float m_CurrentHP;
    [SerializeField] private Bar m_HPBar;
    [SerializeField] private TMP_Text m_HPText;
    private bool m_IsInvulnerable = false;
    private Coroutine InvulnerableCoroutine = null;
    #endregion HP

    #region SP
    [Header("SP")]
    [Tooltip("The maximum amount of Shield Points the player can have")]
    [SerializeField] private float m_MaxSP;
    [Tooltip("The amount of Shield Points the player currently has")]
    [SerializeField] private float m_CurrentSP;
    [SerializeField] private Bar m_SPBar;
    private Coroutine RecoverSPCoroutine = null;
    #endregion SP

    [SerializeField] private float m_InvulnerableTime = 2.0f;

    #endregion Player Stats

    public UnityEvent OnPlayerDeath = new UnityEvent();

    private void Start()
    {
        this.IntializeStats();
    }

    private void IntializeStats()
    {
        m_MaxHP = upgrades.maxHP.GetValue();
        m_MaxSP = upgrades.maxSP.GetValue();

        m_CurrentHP = m_MaxHP;
        m_CurrentSP = m_MaxSP;

        SegmentedBar spBar = m_SPBar as SegmentedBar;
        spBar.SetSegments((int)m_CurrentSP);

        m_HPText.text = $"{m_CurrentHP} / {m_MaxHP}";
    }

    public void TakeDamage(Element attacking, float amount)
    {
        if (m_IsInvulnerable || GetComponent<PlayerMovement>().isDodging)
            return;
        if (m_CurrentSP > 0.0f)
        {
            m_CurrentSP--;
            m_SPBar.UpdateBar(m_CurrentSP / m_MaxSP);
            if (RecoverSPCoroutine != null)
                StopCoroutine(RecoverSPCoroutine);
            RecoverSPCoroutine = StartCoroutine(RecoverSP());

            if (InvulnerableCoroutine != null)
                StopCoroutine(InvulnerableCoroutine);
            StartCoroutine(BecomeInvulnerable());
        }
        else if (m_CurrentHP > 0.0f)
        {
            m_CurrentHP -= amount;
            m_HPBar.UpdateBar(m_CurrentHP / m_MaxHP);
            m_HPText.text = $"{m_CurrentHP} / {m_MaxHP}";

            if (m_CurrentHP <= 0.0)
            {
                if (RecoverSPCoroutine != null)
                    StopCoroutine(RecoverSPCoroutine);
                OnPlayerDeath.Invoke();
            }
            else
            {
                if (RecoverSPCoroutine != null)
                    StopCoroutine(RecoverSPCoroutine);
                RecoverSPCoroutine = StartCoroutine(RecoverSP());
            }
        }
    }

    private IEnumerator RecoverSP()
    {
        yield return new WaitForSeconds(5.0f);

        while (m_CurrentSP < m_MaxSP)
        {
            m_CurrentSP++;
            m_SPBar.UpdateBar(m_CurrentSP / m_MaxSP);
            yield return new WaitForSeconds(1.0f);
        }

        yield break;
    }

    private IEnumerator BecomeInvulnerable()
    {
        m_IsInvulnerable = true;
        yield return new WaitForSeconds(m_InvulnerableTime);
        m_IsInvulnerable = false;
        yield break;
    }
}
