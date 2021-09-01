using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Image m_Bar;
    [SerializeField] private Image m_TransitionBar;
    private Coroutine TransitionCoroutine = null;
    private float m_OldRatio = 1.0f;
    public void UpdateBar(float ratio)
    {
        if (!m_TransitionBar)
        {
            m_Bar.fillAmount = ratio;
            return;
        }

        if (m_OldRatio > ratio)
        {
            m_Bar.fillAmount = ratio;
            if (TransitionCoroutine != null)
                StopCoroutine(TransitionCoroutine);
            TransitionCoroutine = StartCoroutine(Transition_Low());
        }
        else
        {
            m_TransitionBar.fillAmount = ratio;
            if (TransitionCoroutine != null)
                StopCoroutine(TransitionCoroutine);
            TransitionCoroutine = StartCoroutine(Transition_High());
        }

        m_OldRatio = ratio;
    }

    private IEnumerator Transition_Low()
    {
        m_TransitionBar.color = Color.red;
        yield return new WaitForSeconds(0.5f);

        float oldFillAmount = m_TransitionBar.fillAmount;
        float time = 0.0f;
        while (m_TransitionBar.fillAmount > m_Bar.fillAmount)
        {
            m_TransitionBar.fillAmount = Mathf.Lerp(oldFillAmount, m_Bar.fillAmount, time);
            time += 0.02f;

            yield return new WaitForSeconds(0.01f);
        }

        yield break;
    }

    private IEnumerator Transition_High()
    {
        m_TransitionBar.color = Color.white;
        yield return new WaitForSeconds(0.25f);

        float oldFillAmount = m_Bar.fillAmount;
        float time = 0.0f;
        while (m_Bar.fillAmount < m_TransitionBar.fillAmount)
        {
            m_Bar.fillAmount = Mathf.Lerp(oldFillAmount, m_TransitionBar.fillAmount, time);
            time += 0.02f;

            yield return new WaitForSeconds(0.01f);
        }

        yield break;
    }
}
