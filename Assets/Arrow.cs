using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Arrow : GridItemCore
{
    int attack = 1;
    public override string Desc => $@"{base.Desc}
Attack {attack} * moved distance when hit the border";
    public override void hitBorder(List<BattleMessage> messages,Vector2Int borderIndex)
    {
        //FloatingTextManager.Instance.addText("Arrowwww!", transform.position,Color.red);

        //Luggage.Instance.DoDamage(attack * movedCount);
        var originIndex = index;
        int diff = (int)(borderIndex - originIndex).magnitude;

        var  dir = (borderIndex - originIndex) / diff;
        //Debug.Log("diff " + diff);
        messages.Add(new MessageItemAttack { item = this, damage = attack *( movedCount+1), skipAnim = true });
        index = borderIndex + dir * 10; ;
        messages.Add(new MessageItemMove { item = this });
        this.addDestroyMessageWithIndex(messages, originIndex, true);
    }

    public override void move(List<BattleMessage> messages)
    {
        base.move(messages);

    }
}
