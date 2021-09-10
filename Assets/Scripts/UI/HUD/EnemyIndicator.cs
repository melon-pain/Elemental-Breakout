using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIndicator : MonoBehaviour
{
    [SerializeField] private GameObject m_Follow;
    [SerializeField] private RectTransform parentRectTransform;
    [SerializeField] private Image m_Image;
    private RectTransform rectTransform;
    private Renderer rend;
    private Vector3 minPosition;
    private Vector3 maxPosition;
    private void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();
        rend = m_Follow.GetComponent<Renderer>();
        minPosition = parentRectTransform.rect.min - this.GetComponent<RectTransform>().rect.min;
        maxPosition = parentRectTransform.rect.max - this.GetComponent<RectTransform>().rect.max;
        m_Image = this.GetComponent<Image>();
    }

    private void Update()
    {
        if (!rend.isVisible)
        {
            m_Image.enabled = true;
            Vector3 worldToScreen = Camera.main.WorldToScreenPoint(m_Follow.transform.position);
            worldToScreen.x = Mathf.Clamp(worldToScreen.x, rectTransform.rect.width / 2, Screen.width - rectTransform.rect.width / 2);
            worldToScreen.y = Mathf.Clamp(worldToScreen.y, rectTransform.rect.height / 2, Screen.height - rectTransform.rect.height / 2);
            this.transform.position = worldToScreen;
        }
        else
        {
            m_Image.enabled = false;
        }

    }
}
