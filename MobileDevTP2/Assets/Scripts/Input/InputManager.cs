using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Action<Vector2> AxisInputReceived;
    [SerializeField] Vector2 axisInput;
    [SerializeField] ShipController player;

    #region Unity Events
    private void Start()
    {
        AxisInputReceived += player.OnNewDirection;
    }
    #endregion Unity Events

    public void OnAxisInputReceived(Vector2 newInput)
    {
        axisInput = newInput;
        AxisInputReceived?.Invoke(newInput);
    }
}