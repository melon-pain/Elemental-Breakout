using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnScreenJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Smoothing Filtering")]
    [SerializeField] private bool m_EnableSmoothing = true;
    [SerializeField, Min(1.0f)] private float m_SmoothSpeed = 10.0f;

    private RectTransform m_Base;
    private RectTransform m_Thumb;
    public Vector3 axis { get { return m_Axis; } }
    private Vector3 m_Axis = Vector3.zero;
    private float m_MovementRange = 0.0f;
    private bool m_isDragging = true;

    private Coroutine ResetCoroutine = null;

    private void Start()
    {
        this.m_Base = this.transform.GetChild(0).GetComponent<RectTransform>();
        this.m_Thumb = this.transform.GetChild(1).GetComponent<RectTransform>();
        this.m_MovementRange = m_Base.rect.width / 2.0f - m_Thumb.rect.width / 2.0f;
    }

    private void Update()
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_isDragging = true;
        if (ResetCoroutine != null)
        {
            StopCoroutine(ResetCoroutine);
            ResetCoroutine = null;
        }
        EventSystem.current.SetSelectedGameObject(m_Thumb.gameObject, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_Thumb.position = eventData.position;
        m_Thumb.anchoredPosition = Vector3.ClampMagnitude(m_Thumb.anchoredPosition, m_MovementRange);

        if (m_EnableSmoothing)
            m_Axis = m_Thumb.anchoredPosition / m_MovementRange;
        else
            m_Axis = m_Thumb.anchoredPosition.normalized;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_isDragging = false;
        ResetCoroutine = StartCoroutine(Reset());
        EventSystem.current.SetSelectedGameObject(null, eventData);
    }

    private IEnumerator Reset()
    {
        Vector3 oldAxis = m_Axis;
        Vector3 oldPos = m_Thumb.anchoredPosition;
        float t = 0.0f;

        while (t < 1.0f && !m_isDragging && m_EnableSmoothing)
        {
            this.m_Axis = Vector3.Lerp(oldAxis, Vector3.zero, t);
            this.m_Thumb.anchoredPosition = Vector3.Lerp(oldPos, Vector3.zero, t);
            t += 0.01f * this.m_SmoothSpeed;
            yield return new WaitForSeconds(0.01f);
        }

        this.m_Axis = Vector3.zero;
        this.m_Thumb.anchoredPosition = Vector3.zero;
        yield break;
    }
}
