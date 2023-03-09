using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovable : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    protected bool isMoving = false;
    [SerializeField] protected MoreMountains.Feedbacks.MMF_Player player;
    public void startMove()
    {
        if (player == null)
        {
            //player = FindObjectOfType<MoreMountains.Feedbacks.MMF_Player>();
        }
        isMoving = true;
        //player.PlayFeedbacks();
    }
    public void stopMove()
    {
        isMoving = false;
    }

    protected virtual void Update()
    {
        if (isMoving)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
    }
}
