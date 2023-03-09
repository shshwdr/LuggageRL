using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Ore : GridItemCore
{
    public override void hitBorder(List<BattleMessage> messages, Vector2Int borderIndex)
    {
        int damage = info.Param1;
        int moveDamageScale = info.Param2;
        var originIndex = index;
        int diff = (int)(borderIndex - originIndex).magnitude;
        if (diff > 0)
        {
            messages.Add(new MessageItemAttack { item = this, damage = CalculateDamage (damage)* moveDamageScale, index = index });
        }
        else
        {
            messages.Add(new MessageItemAttack { item = this, damage = CalculateDamage(damage), index = index });
        }
        buffs.Clear();
        //FloatingTextManager.Instance.addText("Attack!", transform.position);
        //Luggage.Instance.DoDamage(1);
    }
}
