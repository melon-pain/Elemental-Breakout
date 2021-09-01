using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationDirection
{
    CW,
    CCW
}

public class RotateEventData : System.EventArgs
{
    public Touch finger1 { get; private set; }
    public Touch finger2 { get; private set; }
    public float angle { get; private set; }
    public RotationDirection rotationDirection { get; private set; } = RotationDirection.CW;
    public GameObject hitObject { get; private set; } = null;

    public RotateEventData(Touch finger1, Touch finger2, float angle, RotationDirection dir, GameObject hitObject = null)
    {
        this.finger1 = finger1;
        this.finger2 = finger2;
        this.angle = angle;
        this.rotationDirection = dir;
        this.hitObject = hitObject;
    }
}

public interface IRotateHandler
{
    public void OnRotate(RotateEventData eventData);
}
