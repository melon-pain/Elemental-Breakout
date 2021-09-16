using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class GestureManager : MonoBehaviour
{
    private Touch finger_1;
    private Touch finger_2;

    [Header("Layers"), Tooltip("Which layers to test for gestures")]
    [SerializeField] private LayerMask cullingMask;

    [Header("Tap")]
    [SerializeField] private TapProperty tapProperty;
    [Space(4.0f)] public UnityEvent<TapEventData> OnTap;

    [Header("Swipe")]
    [SerializeField] private SwipeProperty swipeProperty;
    [Space(4.0f)] public UnityEvent<SwipeEventData> OnSwipe;

    [Header("Drag")]
    [SerializeField] private DragProperty dragProperty;
    [Space(4.0f)] public UnityEvent<DragEventData> OnDrag;

    [Header("Pan")]
    [SerializeField] private TwoFingerPanProperty panProperty;
    [Space(4.0f)] public UnityEvent<TwoFingerPanEventData> OnPan;

    [Header("Spread")]
    [SerializeField] private SpreadProperty spreadProperty;
    [Space(4.0f)] public UnityEvent<SpreadEventData> OnSpread;

    [Header("Rotate")]
    [SerializeField] private RotateProperty rotateProperty;
    [Space(4.0f)] public UnityEvent<RotateEventData> OnRotate;

    private Vector2 startPoint = Vector2.zero;
    private Vector2 endPoint = Vector2.zero;
    private float gestureTime = 0.0f;

    private bool wasTouchingUI = false;
    private bool hasGesture = false;

    private EventSystem eventSystem;

    // Start is called before the first frame update
    private void Start()
    {
        eventSystem = EventSystem.current;
    }

    // Update is called once per frame
    private void Update()
    {
        int touchCount = Input.touchCount;

        if (touchCount > 0)
        {
            if (hasGesture)
                return;
            switch (touchCount)
            {
                case 1:
                    OneFingerGesture();
                    break;
                case 2:
                    TwoFingerGesture();
                    break;
            }
        }
        else
        {
            hasGesture = false;
        }

    }
    private void OneFingerGesture()
    {
        //Consume UI
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) || EventSystem.current.currentSelectedGameObject)
        {
            wasTouchingUI = true;
            return;
        }

        finger_1 = Input.GetTouch(0);

        switch (finger_1.phase)
        {
            case TouchPhase.Began:
                startPoint = finger_1.position;
                gestureTime = 0.0f;
                break;
            case TouchPhase.Ended:
                if (wasTouchingUI)
                {
                    wasTouchingUI = false;
                    return;
                }
                endPoint = finger_1.position;
                if (gestureTime <= tapProperty.GetTapTime() && Vector2.Distance(startPoint, endPoint) < (Screen.dpi * tapProperty.GetTapDistance()))
                {
                    Tap();
                }
                else if (gestureTime <= swipeProperty.GetSwipeTime() && Vector2.Distance(startPoint, endPoint) >= (Screen.dpi * swipeProperty.GetMinSwipeDistance()))
                {
                    Swipe();
                }
                break;
            default:
                gestureTime += Time.deltaTime;
                break;
        }
        if (gestureTime >= dragProperty.dragBufferTime)
        {
            Drag();
        }

    }
    private void TwoFingerGesture()
    {
        //Consume UI
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) || 
            EventSystem.current.IsPointerOverGameObject(Input.GetTouch(1).fingerId) || 
            EventSystem.current.currentSelectedGameObject)
        {
            finger_1 = new Touch();
            finger_2 = new Touch();
            wasTouchingUI = true;
            return;
        }
        else if (wasTouchingUI)
        {
            wasTouchingUI = false;
            return;
        }

        finger_1 = Input.GetTouch(0);
        finger_2 = Input.GetTouch(1);

        if (finger_1.phase == TouchPhase.Moved && finger_2.phase == TouchPhase.Moved)
        {
            Vector2 prevPoint1 = GetPreviousPoint(finger_1);
            Vector2 prevPoint2 = GetPreviousPoint(finger_2);

            Vector2 diffVector = finger_1.position - finger_2.position;
            Vector2 prevDiffVector = prevPoint1 - prevPoint2;

            float currDistance = Vector2.Distance(finger_1.position, finger_2.position);
            float prevDistance = Vector2.Distance(prevPoint1, prevPoint2);
            float distance = currDistance - prevDistance;

            if (Mathf.Abs(distance) >= (spreadProperty.MinDistanceChange * Screen.dpi))
            {
                Spread(distance);
                Debug.Log("Spread");
            }

            else if (currDistance >= rotateProperty.minDistance * Screen.dpi)
            {
                Rotate(prevDiffVector, diffVector);
            }
        }
    }

    private void Tap()
    {
        //Check for event listeners
        if (OnTap.GetPersistentEventCount() > 0)
        {
            GameObject hitObj = null;
            Ray r = Camera.main.ScreenPointToRay(finger_1.position);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(r, out hit, Mathf.Infinity, cullingMask))
            {
                hitObj = hit.collider.gameObject;
                Debug.Log(hitObj.name);
            }

            TapEventData tapEventData = new TapEventData(finger_1.position, hitObj);
            OnTap.Invoke(tapEventData);
        }
    }
    private void Swipe()
    {
        //Check for event listeners
        if (OnSwipe.GetPersistentEventCount() > 0)
        {
            Vector2 dir = endPoint - startPoint;

            GameObject hitObj = null;
            Ray r = Camera.main.ScreenPointToRay(startPoint);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(r, out hit, Mathf.Infinity, cullingMask))
            {
                hitObj = hit.collider.gameObject;
                Debug.Log(hitObj.name);
            }

            SwipeDirection swipeDir = SwipeDirection.Right;

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0.0f)
                    swipeDir = SwipeDirection.Right;
                else
                    swipeDir = SwipeDirection.Left;
            }
            else
            {
                if (dir.y > 0.0f)
                    swipeDir = SwipeDirection.Up;
                else
                    swipeDir = SwipeDirection.Down;
            }

            SwipeEventData swipeEventData = new SwipeEventData(startPoint, swipeDir, dir, hitObj);
            OnSwipe.Invoke(swipeEventData);

            Debug.Log("Swipe");
        }
        //hasGesture = true;
    }

    private void Drag()
    {
        if (OnDrag.GetPersistentEventCount() > 0)
        {
            Ray r = Camera.main.ScreenPointToRay(finger_1.position);
            RaycastHit hit = new RaycastHit();
            GameObject hitObj = null;

            if (Physics.Raycast(r, out hit, Mathf.Infinity, cullingMask))
            {
                hitObj = hit.collider.gameObject;
                Debug.Log("Drag hit");
            }

            DragEventData dragEventData = new DragEventData(finger_1, hitObj);
            OnDrag.Invoke(dragEventData);
        }
    }

    private void Pan()
    {
        if (OnPan.GetPersistentEventCount() > 0)
        {
            TwoFingerPanEventData panEventData = new TwoFingerPanEventData(finger_1, finger_2);
            OnPan.Invoke(panEventData);
        }
    }

    private void Spread(float distance)
    {
        if (OnSpread.GetPersistentEventCount() > 0)
        {
            Vector2 mid = Midpoint(finger_1.position, finger_2.position);
            SpreadDirection dir = SpreadDirection.Pinch;

            if (distance > 0)
                dir = SpreadDirection.Spread;

            Ray r = Camera.main.ScreenPointToRay(mid);
            RaycastHit hit = new RaycastHit();
            GameObject hitObj = null;

            if (Physics.Raycast(r, out hit, Mathf.Infinity))
            {
                hitObj = hit.collider.gameObject;
            }

            SpreadEventData spreadEventData = new SpreadEventData(finger_1, finger_2, distance, dir, hitObj);
            OnSpread.Invoke(spreadEventData);
        }
        hasGesture = true;
    }

    private void Rotate(Vector2 prevDiffVector, Vector2 diffVector)
    {
        if (OnRotate.GetPersistentEventCount() > 0)
        {
            float angle = Vector2.Angle(prevDiffVector, diffVector);

            if (angle >= rotateProperty.minChange)
            {
                Vector3 cross = Vector3.Cross(prevDiffVector, diffVector);
                RotationDirection dir = RotationDirection.CW;
                if (cross.z > 0)
                {
                    dir = RotationDirection.CCW;
                }

                Vector2 mid = Midpoint(finger_1.position, finger_2.position);

                Ray r = Camera.main.ScreenPointToRay(mid);
                RaycastHit hit = new RaycastHit();
                GameObject hitObj = null;

                if (Physics.Raycast(r, out hit, Mathf.Infinity))
                {
                    hitObj = hit.collider.gameObject;
                }

                RotateEventData rotateEventData = new RotateEventData(finger_1, finger_2, angle, dir, hitObj);
                OnRotate.Invoke(rotateEventData);
            }
        }
        hasGesture = true;
    }

    private Vector2 GetPreviousPoint(Touch finger)
    {
        return finger.position - finger.deltaPosition;
    }

    private Vector2 Midpoint(Vector2 p1, Vector2 p2)
    {
        return (p1 + p2) / 2;
    }
}
