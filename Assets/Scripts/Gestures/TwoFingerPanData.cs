using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoFingerPanEventData : System.EventArgs
{
    public Touch finger1 { get; private set; }
    public Touch finger2 { get; private set; }
    
    public TwoFingerPanEventData(Touch finger1, Touch finger2)
    {
        this.finger1 = finger1;
        this.finger2 = finger2;
    }
}
