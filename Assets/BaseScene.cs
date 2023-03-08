using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : BaseMovable
{
    protected bool hasStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (hasStarted)
        {
            if (transform.position.x < -50)
            {
                Destroy(gameObject);
            }
        }
    }
}
