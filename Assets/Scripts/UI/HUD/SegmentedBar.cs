using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SegmentedBar : Bar
{
    [SerializeField] private Image m_Mask;

    public void SetSegments(int count)
    {
        m_Mask.pixelsPerUnitMultiplier = count;
    }
}
