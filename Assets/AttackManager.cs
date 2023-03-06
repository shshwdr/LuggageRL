using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{

    void Move(int x, int y)
    {
        StartCoroutine(GridManager.Instance.MoveAfter(x, y));
    }

    void Rotate(int times)
    {

        GridManager.Instance.Rotate(times,true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Rotate(1);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            //push forward and attack first enemy
            StartCoroutine(Luggage.Instance.PushForwardAttack());
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            //push forward and attack first enemy
            StartCoroutine(Luggage.Instance.UpsideDownAndDrop());
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            //push forward and attack first enemy
            StartCoroutine(Luggage.Instance.ThrowOutAndHitBack());
        }

    }
}
