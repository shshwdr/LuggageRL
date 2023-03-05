using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : GridItem
{
    int attack = 2;
    public override void hitBorder(List<BattleMessage> messages,Vector2Int borderIndex)
    {
        //FloatingTextManager.Instance.addText("Arrowwww!", transform.position,Color.red);

        //Luggage.Instance.DoDamage(attack * movedCount);
        var originIndex = index;
        int diff = (int)(borderIndex - originIndex).magnitude;

        var  dir = (borderIndex - originIndex) / diff;
        Debug.Log("diff " + diff);
        GridManager.Instance.attackBeforeMoveMessage.Add(new MessageItemAttack { item = this, damage = attack * diff, skipAnim = true });
        index = borderIndex + dir * 10; ;
        GridManager.Instance.attackBeforeMoveMessage.Add(new MessageItemMove { item = this });
        this.addDestroyMessageWithIndex(messages, originIndex, true);
    }

}
