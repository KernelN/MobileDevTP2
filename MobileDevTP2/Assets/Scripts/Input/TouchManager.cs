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
                if (!Physics.Raycast(ray, 100, notTouchableMask))
                {                    
                    touchPositions.Add(Camera.main.ScreenToWorldPoint(touch.position));
                }
                break;

            //Add mid touch to gesture's touch positions list
            case TouchPhase.Moved:
                touchPositions.Add(Camera.main.ScreenToWorldPoint(touch.position));
                break;

            //Add last touch to gesture's touch positions list
            case TouchPhase.Ended:
                touchPositions.Add(Camera.main.ScreenToWorldPoint(touch.position));
                BuildGesture();
                break;
        }
    }
    void BuildGesture()
    {
        //Get positions
        Vector2 firstPos = touchPositions[0];
        Vector2 midPos = touchPositions[touchPositions.Count / 2];
        Vector2 lastPos = touchPositions[touchPositions.Count - 1];

        //Clear list, as the values are no longer needed
        touchPositions.Clear();

        //Send output
        NewGestureInput?.Invoke(firstPos, midPos, lastPos);
    }
}
#endif