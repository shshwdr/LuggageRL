using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PiggyBank : GridItemCore
{
    public override void beCrushed(IGridItem item, List<BattleMessage> messages)
    {
        messages.Add(new MessageItemApplyEffect { item = this, index = index, target = item, type = BuffType.piggyBank, value = count, targetIndex = item.index });
        //messages.Add(new MessageItemAttack { item = this, damage = attack, index = index });
        this.addDestroyMessage(messages);
    }
    public override void afterTurn(List<BattleMessage> messages)
    {
        var value = info.Param1;
        count+= value;
        messages.Add(new MessageItemChangeCounter { item = this, index = index,amount = value });
    }


}
