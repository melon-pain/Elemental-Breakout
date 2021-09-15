using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIndicator : MonoBehaviour
{
    [SerializeField] private GameObject m_Follow;
    [SerializeField] private RectTransform parentRectTransform;
    private Image m_Image;
    private RectTransform rectTransform;
    private Renderer rend;
    private Vector3 minPosition;
    private Vector3 maxPosition;
    private void Start()
    {
        if (!m_Follow)
            return;
        rectTransform = this.GetComponent<RectTransform>();
        rend = m_Follow.GetComponent<Renderer>();
        minPosition = parentRectTransform.rect.min - this.GetComponent<RectTransform>().rect.min;
        maxPosition = parentRectTransform.rect.max - this.GetComponent<RectTransform>().rect.max;
        m_Image = this.GetComponent<Image>();
    }

    private void Update()
    {
        if (!m_Follow)
            return;
        if (!rend.isVisible)
        {
            m_Image.enabled = true;
            Vector3 worldToScreen = Camera.main.WorldToScreenPoint(m_Follow.transform.position);
            worldToScreen.x = Mathf.Clamp(worldToScreen.x, (rectTransform.rect.width / 2) * 0.8f, (Screen.width - rectTransform.rect.width / 2) * 0.8f);
            worldToScreen.y = Mathf.Clamp(worldToScreen.y, (rectTransform.rect.height / 2) * 0.8f, (Screen.height - rectTransform.rect.height / 2) * 0.8f );

            Vector3 angle = worldToScreen - new Vector3(Screen.width / 2, Screen.height / 2, 0.0f);
            this.transform.position = worldToScreen;
            this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, Mathf.Atan2(angle.y, angle.x) * Mathf.Rad2Deg);
        }
        else
        {
            m_Image.enabled = false;
        }

    }
}
