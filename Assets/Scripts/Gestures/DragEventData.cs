using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragEventData : System.EventArgs
{
    public Touch TargetFinger { get; private set; }

    public GameObject HitObject { get; private set; } = null;

    public DragEventData(Touch targetFinger, GameObject hitObject = null)
    {
        TargetFinger = targetFinger;
        HitObject = hitObject;
    }
}
