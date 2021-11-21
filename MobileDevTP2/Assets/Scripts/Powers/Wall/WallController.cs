using System;
using UnityEngine;

public class WallController : MonoBehaviour, IHittable
{
    public Action<GameObject> WallDestroyed;
	[SerializeField] WallData data;
	[SerializeField] float countdownUntilDestroy;
    SpriteRenderer spriteRenderer;
    Color wallColor;

    #region Unity Events
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        wallColor = spriteRenderer.color;
    }
    private void OnEnable()
    {
        wallColor.a = 1;
        countdownUntilDestroy = 0;
    }
    private void Update()
    {
        //Advance Countdown
        countdownUntilDestroy += Time.deltaTime;

        //Update fade out to be equal to countdown
        wallColor.a = 1 - countdownUntilDestroy / data.lifeTime;
        spriteRenderer.color = wallColor;

        //if countdown is over, despawn
        if (countdownUntilDestroy > data.lifeTime)
        {
            Despawn();
        }
    }
    #endregion

    //Methods
    public void SetData(WallData newData)
    {
        data = newData;
    }
    void Despawn()
    {
        WallDestroyed?.Invoke(gameObject);
        gameObject.SetActive(false);
    }

    //Implementation
    public void Hitted(int damage)
    {
        if (damage >= data.resistance)
        {
            Despawn();
        }
    }
}