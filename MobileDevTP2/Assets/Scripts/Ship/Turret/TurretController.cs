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
	[SerializeField] float rotationTimerMax;
    IHittable currentHittableTarget;
    Vector2 originalRotation;
    Vector2 newDirection;
    float rotateTimer;
    //bool rotating = true;

    //Unity Events
    private void Start()
    {
        //Set Data
        if (data == null)
        {
            data = new TurretData();
        }

        //Link Actions
        fieldOfView.CollisionEnter2D += OnCollisionEnter2D;
        fieldOfView.CollisionExit2D += OnCollisionExit2D;

        //Set Shooting
        float shotsPerSecond = 1 / data.shootSpeed * data.shootSpeedMod;
        InvokeRepeating("ShootTarget", shotsPerSecond, shotsPerSecond);

        //Set Rotation
        originalRotation = transform.up;
        rotateTimer = rotationTimerMax;
    }
    private void Update()
    {
        LookAtTarget();
    }

    //Methods
    public void SetData(TurretData newData)
    {
        data = newData;
    }
    void LookAtTarget()
    {
        //Lerp the UP rotation of the transform until it becomes the new direction
        transform.up = Vector2.Lerp(originalRotation, newDirection, rotateTimer / rotationTimerMax);
        if (rotateTimer <= rotationTimerMax)
            rotateTimer += Time.deltaTime;
    }
    void UpdateTargets()
    {
        //Remove Target if not inside list
        if (!targets.Contains(currentTarget))
        {
            currentTarget = null;
        }

        if (targets.Count < 1) return; //return if there are no targets 

        //Order List
        targets.Sort(Compare);
        
        //Add new target if current is empty
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
        currentHittableTarget?.Hitted((int)(data.shootDamage * data.shootDamageMod));
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
        originalRotation = transform.up;
        newDirection = (targetPosition - (Vector2)transform.position); //distance between turret and target
        rotateTimer = 0;
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