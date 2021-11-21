using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Action<Vector2> AxisInputReceived;
    public Action<Vector2> CircleInputReceived;
    public Action<Vector2, Vector2, float> WallInputReceived;//midPos,angle,length
    public Action<Vector2, Vector2, float> FireInputReceived;//midPos,angle,length
    [SerializeField] ShipController player;
    [SerializeField] TurretController turret;
    [SerializeField] PowersManager powers;
    [SerializeField] Vector2 axisInput;
    [Tooltip("X=Min,Y=Max")] 
    [SerializeField] Vector2 lineAngleLimits;
    [SerializeField] Transform TEMPwall; //TEMP

    //Unity Events
    private void Start()
    {
        AxisInputReceived += player.OnNewDirection;
        CircleInputReceived += turret.OnNewTargetArea;
        WallInputReceived += powers.OnWallInputReceived;
    }

    //Methods
    Vector2 GetMidPosOfCircle(Vector2 firstTouch, Vector2 midTouch, Vector2 lastTouch)
    {
        //calculate midPos between first and mid touch
        Vector2 firstToMid = (firstTouch + midTouch) / 2;
     
        //calculate midPos between last and mid touch
        Vector2 LastToMid = (lastTouch + midTouch) / 2;

        //calculate final midPos
        return (firstToMid + LastToMid) / 2; 
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

        float lineLenght;
        Vector2 lineDirection = Vector2.Perpendicular(firstPos - lastPos).normalized;

        if (lineAngle < lineAngleLimits.x || lineAngle > lineAngleLimits.y)
        {
            Debug.Log("Vertical");
        }
        else
        {
            //set lineLength
            if (Mathf.Abs(midPos.x - shipPos.x) > Mathf.Abs(midPos.y - shipPos.y)) //if horizontal to player
            {
                lineLenght = Mathf.Abs(firstPos.y - lastPos.y);
            }
            else
            {
                lineLenght = Mathf.Abs(firstPos.x - lastPos.x);
            }

            WallInputReceived.Invoke(midPos, lineDirection, lineLenght);
        }
    }
}