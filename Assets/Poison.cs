using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Poison : GridItemCore
{
    public int value = 1;
    public override string Desc => $@"{base.Desc}
Apply {value} poison on item that hit it
{BuffDesc}";
    public override void beCrushed(IGridItem item, List<BattleMessage> messages)
    {
        messages.Add(new MessageItemApplyEffect { item = this, index = index, target = item, type = BuffType.poison, value = value , targetIndex  = item.index});
        //messages.Add(new MessageItemAttack { item = this, damage = attack, index = index });
        this.addDestroyMessage(messages);
    }
}
