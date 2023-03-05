using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : GridItem
{
    public override void hitBorder(List<IBattleMessage> messages) {
        messages.Add(new MessageItemAttack { item = this, damage = 1 });
        //FloatingTextManager.Instance.addText("Attack!", transform.position);
        //Luggage.Instance.DoDamage(1);
    }
    public override void bigHitBorder(List<IBattleMessage> messages) {
        //FloatingTextManager.Instance.addText("Big Attack!", transform.position);
        //Luggage.Instance.DoDamage(2);
        messages.Add(new MessageItemAttack { item = this, damage = 2 });
    }
}
