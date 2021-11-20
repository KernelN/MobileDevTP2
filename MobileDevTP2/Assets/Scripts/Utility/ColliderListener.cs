using System;
using UnityEngine;

public class ColliderListener : MonoBehaviour
{
    public Action<Collision2D> CollisionEnter2D;
    public Action<Collision2D> CollisionExit2D;

    void Awake()
    {
        // Check if Colider is in another GameObject
        Collider2D collider = GetComponentInChildren<Collider2D>();
        if (collider.gameObject != gameObject)
        {
            ColliderBridge cb = collider.gameObject.AddComponent<ColliderBridge>();
            cb.Initialize(this);
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionEnter2D?.Invoke(collision);
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        CollisionExit2D?.Invoke(collision);
    }
}