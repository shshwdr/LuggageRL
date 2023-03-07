using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Ore : GridItemCore
{
    public int damage = 1;
    public int moveDamageScale = 2;

    public override string Desc => $@"{base.Desc}
Deal {damage} damage when hit the border
Deal x{moveDamageScale} when moved in the attack";
    public override void hitBorder(List<BattleMessage> messages, Vector2Int borderIndex)
    {
        var originIndex = index;
        int diff = (int)(borderIndex - originIndex).magnitude;
        if (diff > 0)
        {
            messages.Add(new MessageItemAttack { item = this, damage = damage* moveDamageScale, index = index });
        }
        else
        {
            messages.Add(new MessageItemAttack { item = this, damage = damage, index = index });
        }
        //FloatingTextManager.Instance.addText("Attack!", transform.position);
        //Luggage.Instance.DoDamage(1);
    }
}
