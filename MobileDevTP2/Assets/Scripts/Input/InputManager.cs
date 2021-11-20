using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Action<Vector2> AxisInputReceived;
    public Action<Vector2> CircleInputReceived;
    public Action<Vector2> WallInputReceived;
    public Action<Vector2> FireInputReceived;
    [SerializeField] ShipController player;
    [SerializeField] TurretController turret;
    [SerializeField] Vector2 axisInput;

    //Unity Events
    private void Start()
    {
        AxisInputReceived += player.OnNewDirection;
        CircleInputReceived += turret.OnNewTargetArea;
    }

    //Methods
    Vector2 GetMidPosOfCircle(Vector2 firstTouch, Vector2 midTouch, Vector2 lastTouch)
    {
        //calculate midPos between first and mid touch
        Vector2 firstToMid = Vector2.Lerp(firstTouch, midTouch, 0.5f);
     
        //calculate midPos between last and mid touch
        Vector2 LastToMid = Vector2.Lerp(lastTouch, midTouch, 0.5f);

        //calculate final midPos
        return Vector2.Lerp(firstToMid, LastToMid, 0.5f); 
    }

    //Event Receivers
    public void OnAxisInputReceived(Vector2 newInput)
    {
        axisInput = newInput;
        AxisInputReceived?.Invoke(newInput);
    }
    public void OnGestureInputReceived(Vector2 firstPos, Vector2 midPos, Vector2 lastPos)
    {
        //if distance between first and last is less than first and mid, set the gesture as circle
        if ((firstPos - lastPos).sqrMagnitude < (firstPos - midPos).sqrMagnitude)
        {
            //send mid of circle to turret
            CircleInputReceived.Invoke(GetMidPosOfCircle(firstPos, midPos, lastPos));
            return; //end method, as the gesture was already defined
        }

        //vertical angle x < 30 | x > 160
        Vector2 shipPos = player.transform.position;
        Vector2 closestPoint;
        Vector2 farthestPoint;
        if ((firstPos - shipPos).sqrMagnitude < (lastPos - shipPos).sqrMagnitude) //fist is closer
        {
            closestPoint = firstPos;
            farthestPoint = lastPos;
        }
        else
        {
            closestPoint = lastPos;
            farthestPoint = firstPos;
        }
        float lineAngle = Vector2.Angle(closestPoint - shipPos, farthestPoint - shipPos);

        if (lineAngle < 20 || lineAngle > 160)
        {
            Debug.Log("Vertical");
        }
        else
        {
            Debug.Log("Horizontal");
        }
    }
}