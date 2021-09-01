using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SwipeProperty
{
    [Tooltip("Minimum distance covered to be considered a Swipe")]
    [SerializeField] private float minSwipeDistance = 2.0f;

    [Tooltip("Max gesture time until it's not a swipe anymore")]
    [SerializeField] private float swipeTime = 0.7f;

    public float GetMinSwipeDistance()
    {
        return minSwipeDistance;
    }
    public float GetSwipeTime()
    {
        return swipeTime;
    }
}
