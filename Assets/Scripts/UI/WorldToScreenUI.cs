using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldToScreenUI : MonoBehaviour
{
    [SerializeField] private Transform m_Follow = null;
    [SerializeField] private bool m_SmoothFollow = false;

    private void Update()
    {
        if (m_SmoothFollow)
        {
            Vector3 velocity = Vector3.zero;
            this.transform.position = Vector3.SmoothDamp(this.transform.position, Camera.main.WorldToScreenPoint(m_Follow.position), ref velocity, 0.05f);
        }
        else
        {
            this.transform.position = Camera.main.WorldToScreenPoint(m_Follow.position);
        }
    }
}
