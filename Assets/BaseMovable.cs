using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovable : MonoBehaviour
{
    protected bool isMoving = true;
    public void startMove()
    {
        isMoving = true;
    }
    public void stopMove()
    {
        isMoving = false;
    }
}
