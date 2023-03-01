using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : GridItem
{
    public override void hitBorder() { FloatingTextManager.Instance.addText("Attack!", borderPosition); }
    public override void bigHitBorder() { FloatingTextManager.Instance.addText("Big Attack!", borderPosition); }
    public override void beCrushed(GridItem item)
    {

    }
}
