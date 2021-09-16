using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLevel : MonoBehaviour
{
    [SerializeField] private Level level;
    [SerializeField] private List<GameObject> levels = new List<GameObject>();

    private void Start()
    {
        levels[level.currentLevel - 1].SetActive(true);
    }

}
