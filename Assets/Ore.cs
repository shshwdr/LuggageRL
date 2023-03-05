using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : GridItem
{
    public override void hitBorder(List<BattleMessage> messages, Vector2Int borderIndex) {
        if (movedCount > 0)
        {
            messages.Add(new MessageItemAttack { item = this, damage = 2 });
        }
        else
        {
            messages.Add(new MessageItemAttack { item = this, damage = 1 });
        }
        //FloatingTextManager.Instance.addText("Attack!", transform.position);
        //Luggage.Instance.DoDamage(1);
    }
}
