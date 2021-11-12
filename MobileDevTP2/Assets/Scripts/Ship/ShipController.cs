using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] float speed;
    bool firstDirectionRecieved = false;

    // Update is called once per frame
    void Update()
    {
        if (!firstDirectionRecieved) return;
        
        transform.localPosition += transform.up * speed;
    }

    public void OnNewDirection(Vector2 direction)
    {
        firstDirectionRecieved = true;
        transform.up = direction;
    }
}
