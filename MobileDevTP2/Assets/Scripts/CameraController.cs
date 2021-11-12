using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector2 followRange;
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
        if (Mathf.Abs(player.position.y - transform.position.y) > followRange.y)
            transform.position = new Vector3(transform.position.x, GetNewYAxis(), transform.position.z);

        if (Mathf.Abs(player.position.x - transform.position.x) > followRange.x)
            transform.position = new Vector3(GetNewXAxis(), transform.position.y, transform.position.z);
    }
    float GetNewXAxis()
    {
        if (player.position.x > transform.position.x + followRange.x) //move to the right
        {
            return player.position.x - followRange.x;
        }
        else if (player.position.x < transform.position.x - followRange.x) //move to the left
        {         
            return player.position.x + followRange.x;
        }

        return transform.position.x;
    }
    float GetNewYAxis()
    {
        if (player.position.y > transform.position.y + followRange.y)
        {
            return player.position.y - followRange.y;
        }
        else if (player.position.y < transform.position.y - followRange.y)
        {
            return player.position.y + followRange.y;
        }

        return transform.position.y;
    }
}