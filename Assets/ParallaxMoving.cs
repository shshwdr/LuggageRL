 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMoving : BaseMovable
{
    [SerializeField] float swapOffeset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override  void  Update()
    {
        base.Update();
        if (isMoving)
        {
            if (transform.position.x <= -swapOffeset)
            {
                var pos = transform.position;
                pos.x += swapOffeset;
                transform.position = pos;
            }
        }
    }
}
