using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : GridItem
{
    int attack = 2;
    public override void hitBorder()
    {
    }
    public override void bigHitBorder()
    {
        FloatingTextManager.Instance.addText("Arrowwww!", transform.position);

        Luggage.Instance.DoDamage(attack * movedCount);
        destory();
    }
    public override void beCrushed(GridItem item)
    {

    }
}
