using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TapProperty
{
    [Tooltip("Maximum allowable time until its not a tap anymore")]
    [SerializeField] private float tapTime = 0.7f;
    [Tooltip("Maximum allowable distance until its not a tap anymore")]
    [SerializeField] private float tapDistance = 0.1f;

    public float GetTapTime()
    {
        return tapTime;
    }

    public float GetTapDistance()
    {
        return tapDistance;
    }
}
