using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Transform follow;
    private Vector3 velocity;
    private void Update()
    {
        this.transform.position = Vector3.SmoothDamp(this.transform.position, Camera.main.WorldToScreenPoint(follow.position), ref velocity, 0.05f);
    }
}
