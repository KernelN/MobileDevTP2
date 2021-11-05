using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float followRadius;
    [SerializeField] Transform player;

    #region Unity Events
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        if (!player) return;
        FollowPlayer();
    }
    #endregion Unity Events

    //void FollowPlayer()
    //{
    //    transform.position = player.position;
    //    transform.position -= offset;
    //}
    void FollowPlayer()
    {
        if (Mathf.Abs(player.position.y - transform.position.y) > followRadius)
            transform.position = new Vector3(transform.position.x, GetNewYAxis(), transform.position.z);

        if (Mathf.Abs(player.position.x - transform.position.x) > followRadius)
            transform.position = new Vector3(GetNewXAxis(), transform.position.y, transform.position.z);
    }
    float GetNewXAxis()
    {
        if (player.position.x > transform.position.x + followRadius) //move to the right
        {
            return player.position.x - followRadius;
        }
        else if (player.position.x < transform.position.x - followRadius) //move to the left
        {         
            return player.position.x + followRadius;
        }

        return transform.position.x;
    }
    float GetNewYAxis()
    {
        if (player.position.y > transform.position.y + followRadius)
        {
            return player.position.y - followRadius;
        }
        else if (player.position.y < transform.position.y - followRadius)
        {
            return player.position.y + followRadius;
        }

        return transform.position.y;
    }
}