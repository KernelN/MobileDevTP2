#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchManager : MonoBehaviour
{
    public Action<Vector2, Vector2, Vector2> NewGestureInput;
    [SerializeField] InputManager manager;
    [SerializeField] List<Vector2> touchPositions;
    [SerializeField] LayerMask notTouchableMask;
    bool gestureBuilding = false;

    //Methods Unity Events
    private void Start()
    {
        NewGestureInput += manager.OnGestureInputReceived;
    }
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            AddTouchToGestureList(Input.GetTouch(0));
        }
    }

    //Methods
    void AddTouchToGestureList(Touch touch)
    {
        // Handle finger movements based on touch phase.
        switch (touch.phase)
        {
            // Record initial touch position if it's not touching the V-Stick neither the ship
            case TouchPhase.Began:
                 // Construct a ray from the current touch coordinates
                 Ray ray = Camera.main.ScreenPointToRay(touch.position);
                //Add the touch position to the gesture's touch list if it didn't collide with any obstacles
                if (!Physics.Raycast(ray, int.MaxValue, notTouchableMask))
                {                    
                    touchPositions.Add(touch.position);
                    gestureBuilding = true;
                }
                break;

            //Add mid touch to gesture's touch positions list
            case TouchPhase.Moved:
                if (!gestureBuilding) return;
                touchPositions.Add(touch.position);
                break;

            //Add last touch to gesture's touch positions list
            case TouchPhase.Ended:
                if (!gestureBuilding) return;
                touchPositions.Add(touch.position);
                BuildGesture();
                break;
        }
    }
    void BuildGesture()
    {
        //Get positions in world space
        Vector2 firstPos = Camera.main.ScreenToWorldPoint(touchPositions[0]);
        Vector2 midPos = Camera.main.ScreenToWorldPoint(touchPositions[touchPositions.Count / 2]);
        Vector2 lastPos = Camera.main.ScreenToWorldPoint(touchPositions[touchPositions.Count - 1]);

        //Clear list, as the values are no longer needed
        touchPositions.Clear();
        gestureBuilding = false; //set gesture as ended

        //Send output
        NewGestureInput?.Invoke(firstPos, midPos, lastPos);
    }
}
#endif