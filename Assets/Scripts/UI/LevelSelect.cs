using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private List<GameObject> levels = new List<GameObject>();

    [ReadOnlyInInspector][SerializeField] private int currentLevel = 0;

    public void Select(SwipeEventData eventData)
    {
        switch (eventData.direction)
        {
            case SwipeDirection.Left:
                Next();
                break;
            case SwipeDirection.Right:
                Previous();
                break;
        }
    }

    public void Next()
    {
        levels[currentLevel].SetActive(false);
        currentLevel++;
        if (currentLevel >= levels.Count)
            currentLevel = 0;
        levels[currentLevel].SetActive(true);
    }

    public void Previous()
    {
        levels[currentLevel].SetActive(false);
        currentLevel--;
        if (currentLevel < 0)
            currentLevel = levels.Count - 1;
        levels[currentLevel].SetActive(true);
    }
}
