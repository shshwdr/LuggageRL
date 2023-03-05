using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : GridItem
{
    int attack = 3;
    public override void beCrushed(GridItem item, List<IBattleMessage> messages)
    {
        Luggage.Instance.DoDamage(attack);
        FloatingTextManager.Instance.addText("Poison!", transform.position,Color.red);
        destory();
    }
}
