using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.Advertisements;

public class AdFinishEventArgs : EventArgs
{
    public string PlacementID
    {
        get; private set;
    }

    public ShowResult AdResult
    {
        get; private set;
    }

    public AdFinishEventArgs(string id, ShowResult result)
    {
        PlacementID = id;
        AdResult = result;
    }
}
