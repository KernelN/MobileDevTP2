using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour, IComparer<Transform>
{
    [SerializeField] TurretData data;
	[SerializeField] ColliderListener fieldOfView;
	[SerializeField] Transform currentTarget;
	[SerializeField] List<Transform> targets;
	[SerializeField] float rotationTimer;
    IHittable currentHittableTarget;
    Vector2 newDirection;
    bool rotating = true;

    //Unity Events
    private void Start()
    {
        if (data == null)
        {
            data = new TurretData();
        }

        fieldOfView.CollisionEnter2D += OnCollisionEnter2D;
        fieldOfView.CollisionExit2D += OnCollisionExit2D;

        float shotsPerSecond = 1 / data.shootSpeed * data.shootSpeedMod;
        InvokeRepeating("ShootTarget", shotsPerSecond, shotsPerSecond);
    }
    private void Update()
    {
        if (rotating) return;
        transform.up = newDirection;
    }

    //Methods
    public void SetData(TurretData newData)
    {
        data = newData;
    }
    IEnumerator LookAtTarget(Vector2 targetPos)
    {
        float rotateCountdown = 0;
        newDirection = (targetPos - (Vector2)transform.position); //distance between turret and target
        Vector2 originalRotation = transform.up;
        
        rotating = true;

        //Lerp the UP rotation of the transform until it becomes the new direction
        do
        {
            transform.up = Vector2.Lerp(originalRotation, newDirection, rotateCountdown / rotationTimer);
            rotateCountdown += Time.deltaTime;
            yield return null;
        } while (rotateCountdown <= rotationTimer);

        rotating = false;

        yield break;
    }
    void UpdateTargets()
    {
        targets.Sort(Compare);
        if(!currentTarget)
        {
            currentTarget = targets[0];
            currentHittableTarget = currentTarget.GetComponent<IHittable>();
        }
    }
    void ShootTarget()
    {
        if (!currentTarget) return; //return if there is no target

        //if target is not active, remove it, search for another one and lose the shot
        if (!currentTarget.gameObject.activeSelf)
        {
            targets.Remove(currentTarget);
            UpdateTargets();
            return;
        }

        //Shoot
        currentHittableTarget?.Hitted();
    }

    //Implementations
    public int Compare(Transform x, Transform y)
    {
        //if any transform is null, return false
        if (x == null || y == null)
        {
            return 0;
        }

        //Get Distance against player
        float xDistanceToPlayer = (x.position - transform.position).sqrMagnitude;
        float yDistanceToPlayer = (y.position - transform.position).sqrMagnitude;

        // CompareTo() method
        return xDistanceToPlayer.CompareTo(yDistanceToPlayer);
    }

    //Event Receivers
    public	void OnNewTargetArea(Vector2 targetPosition)
    {
        //StopCoroutine(LookAtTarget(targetPosition)); //NOT SURE IF IT WORKS
        StopAllCoroutines();
		transform.LookAt(targetPosition);
        StartCoroutine(LookAtTarget(targetPosition));
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemies")) return;
        if (!targets.Contains(collision.transform))
        {
            targets.Add(collision.transform);
            UpdateTargets();
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemies")) return;
        if (targets.Contains(collision.transform))
        {
            targets.Remove(collision.transform);
            UpdateTargets();
        }
    }    
    
}