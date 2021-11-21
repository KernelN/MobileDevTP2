using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour, IHittable
{
    [SerializeField] float speed;
    Vector2 sizeOfShip;
    bool firstDirectionRecieved = false;

    //Unity Events
    private void Start()
    {
        sizeOfShip = GetComponent<SpriteRenderer>().size;
    }
    void Update()
    {
        if (!firstDirectionRecieved) return;

        MoveShip();
    }

    //Methods
    void MoveShip()
    {
        if (ObstaclesInFront()) return;
        transform.localPosition += transform.up * speed;
    }
    bool ObstaclesInFront()
    {
        //Get size of box
        Vector2 sizeOfBox = sizeOfShip * 0.8f;
        sizeOfBox.y = 0.5f;

        //Check for collision
        RaycastHit2D hittedObstacle;
        hittedObstacle = Physics2D.BoxCast(transform.position, sizeOfBox, 0, transform.up, 0.5f, LayerMask.GetMask("Obstacles"));
        
        //If boxcast collides, there are obstacles in front of ship
        if (hittedObstacle)
            return true;

        return false;
    }

    //Implementations
    public void Hitted(int damage)
    {
        throw new System.NotImplementedException();
    }

    //Event Receivers
    public void OnNewDirection(Vector2 direction)
    {
        firstDirectionRecieved = true;
        transform.up = direction;
    }

}
