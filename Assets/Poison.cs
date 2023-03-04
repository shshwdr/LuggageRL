using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : GridItem
{
    int attack = 3;
    public override void hitBorder() { }
    public override void bigHitBorder() { }
    public override void beCrushed(GridItem item)
    {
        Luggage.Instance.DoDamage(attack);
        FloatingTextManager.Instance.addText("Poison!", transform.position);
        destory();
    }
}
