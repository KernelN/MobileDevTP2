using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Action<Vector2> AxisInputReceived;
    [SerializeField] Vector2 axisInput;

#region Unity Events
        
#endregion Unity Events

    public void OnAxisInputReceived(Vector2 newInput)
    {
        axisInput = newInput;
        AxisInputReceived?.Invoke(newInput);
    }
}