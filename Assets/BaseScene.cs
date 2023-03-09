using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : BaseMovable
{
    public bool hasStarted = false;
    public bool hasFinished = false;


    public Transform itemPositionsParent;
    protected Transform[] itemPositions;
    public GameObject[] itemsToActivate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (hasFinished)
        {
            if (transform.position.x < -50)
            {
                Destroy(gameObject);
            }
        }
    }
}
