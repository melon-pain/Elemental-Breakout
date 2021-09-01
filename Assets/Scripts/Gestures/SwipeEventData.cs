using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}

public class SwipeEventData : System.EventArgs
{
    public Vector2 position { get; private set; } = Vector2.zero;
    public SwipeDirection direction { get; private set; }
    public Vector2 swipeVector { get; private set; } = Vector2.zero;
    public GameObject gameObject { get; private set; } = null;

    public SwipeEventData(Vector2 pos, SwipeDirection dir, Vector2 vector, GameObject obj = null)
    {
        this.position = pos;
        this.direction = dir;
        this.swipeVector = vector;
        this.gameObject = obj;
    }
}

public interface ISwipeHandler
{
    public void OnSwipe(SwipeEventData eventData);
}
