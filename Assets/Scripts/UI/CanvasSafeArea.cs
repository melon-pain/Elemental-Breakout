using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Canvas))]
public class CanvasSafeArea : MonoBehaviour
{
    [SerializeField] private List<RectTransform> rects = new List<RectTransform>();

    private Rect lastSafeArea = Rect.zero;
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>(); 
        lastSafeArea = Screen.safeArea;
        ApplySafeArea();
    }

    private void Update()
    {
        if (lastSafeArea != Screen.safeArea)
        {
            lastSafeArea = Screen.safeArea;
            ApplySafeArea();
        }
    }

    private void ApplySafeArea()
    {
        if (rects.Count <= 0)
        {
            return;
        }

        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= canvas.pixelRect.width;
        anchorMin.y /= canvas.pixelRect.height;
        anchorMax.x /= canvas.pixelRect.width;
        anchorMax.y /= canvas.pixelRect.height;

        foreach (RectTransform rect in rects)
        {
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
        }
    }
}