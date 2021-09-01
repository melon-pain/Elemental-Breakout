using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, ISwipeHandler
{
    [Header("Model")]
    [SerializeField] private GameObject model;

    [Header("Movement")]
    [SerializeField] private float m_MoveSpeed = 10.0f;
    [SerializeField] private float m_DodgeSpeed = 25.0f;
    private Vector3 m_DodgeDirection = Vector3.zero;
    public bool isDodging { get => m_IsDodging; }
    private bool m_IsDodging = false;
    private Coroutine DodgeCoroutine = null;

    private OnScreenJoystick m_Joystick;
    private void Start()
    {
        m_Joystick = FindObjectOfType<OnScreenJoystick>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!m_IsDodging)
            Move();
    }

    private void Move()
    {
        Vector3 axis = this.m_Joystick.axis;
        this.transform.localPosition += axis * m_MoveSpeed * Time.deltaTime;
        Vector3 targetEulerAngles = model.transform.localEulerAngles;
        model.transform.localEulerAngles = new Vector3(Mathf.LerpAngle(targetEulerAngles.x, -axis.y * 45, 0.1f), targetEulerAngles.y, Mathf.LerpAngle(targetEulerAngles.z, -axis.x * 45, 0.1f));
    }

    private IEnumerator Dodge()
    {
        float time = 0.0f;

        Vector3 oldPosition = this.transform.localPosition;
        Quaternion oldRotation = model.transform.localRotation;
        m_IsDodging = true;
        while (time < 1.0f)
        {
            this.transform.localPosition = Vector3.Lerp(oldPosition, oldPosition + (m_DodgeDirection * m_DodgeSpeed), time);
            model.transform.localEulerAngles += new Vector3(0.0f, 0.0f, 360.0f) * 0.025f;
            time += 0.025f;
            yield return new WaitForSeconds(0.01f);
        }
        m_IsDodging = false;

        yield break;
    }

    public void OnSwipe(SwipeEventData eventData)
    {
        if ((eventData.direction == SwipeDirection.Left || eventData.direction == SwipeDirection.Right) && !m_IsDodging)
        {
            m_DodgeDirection = eventData.swipeVector.normalized;
            DodgeCoroutine = StartCoroutine(Dodge());
        }
    }
}
