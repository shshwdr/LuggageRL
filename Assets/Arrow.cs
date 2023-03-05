using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : GridItem
{
    int attack = 2;
    public override void bigHitBorder(List<IBattleMessage> messages)
    {
        FloatingTextManager.Instance.addText("Arrowwww!", transform.position,Color.red);

        Luggage.Instance.DoDamage(attack * movedCount);
        destory();
    }
}
