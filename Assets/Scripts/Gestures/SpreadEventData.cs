using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpreadDirection
{
    Spread,
    Pinch
}

public class SpreadEventData : System.EventArgs
{
    public Touch finger1 { get; private set; } 
    public Touch finger2 { get; private set; }
    public float distanceDelta { get; private set; } = 0.0f;
    public SpreadDirection direction { get; private set; }
    public GameObject hitObject { get; private set; } = null;

    public SpreadEventData(Touch finger1, Touch finger2, float distanceDelta, SpreadDirection dir, GameObject hitObject = null)
    {
        this.finger1 = finger1;
        this.finger2 = finger2;
        this.distanceDelta = distanceDelta;
        this.direction = dir;
        this.hitObject = hitObject;
    }
}

public interface ISpreadHandler
{
    public void OnSpread(SpreadEventData eventData);
}
