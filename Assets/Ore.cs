using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : GridItem
{
    public override void hitBorder() { 
        
        FloatingTextManager.Instance.addText("Attack!", transform.position);
        Luggage.Instance.DoDamage(1);
    }
    public override void bigHitBorder() { FloatingTextManager.Instance.addText("Big Attack!", transform.position);

        Luggage.Instance.DoDamage(2);
    }
    public override void beCrushed(GridItem item)
    {

    }
}
