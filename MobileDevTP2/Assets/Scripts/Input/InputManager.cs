using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Action<Vector2> AxisInputReceived;
    public Action<Vector2> CircleInputReceived;
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
        }
    }
}