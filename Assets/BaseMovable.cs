using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovable : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    protected bool isMoving = false;
    public void startMove()
    {
        isMoving = true;
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
