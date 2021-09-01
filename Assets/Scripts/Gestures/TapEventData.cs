using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TapEventData : System.EventArgs
{
    public Vector2 position { get; private set; } = Vector2.zero;
    public GameObject gameObject { get; private set; } = null;
    public TapEventData (Vector2 pos, GameObject obj = null)
    {
        this.position = pos;
        this.gameObject = obj;
    }
}
