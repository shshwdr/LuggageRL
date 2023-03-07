using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Poison : GridItemCore
{
    public int attack = 3;
    public override string Desc => $@"{base.Desc}
Deal {attack} damage when hit the border";
    public override void beCrushed(IGridItem item, List<BattleMessage> messages)
    {
        messages.Add(new MessageItemAttack { item = this, damage = attack, index = index });
        this.addDestroyMessage(messages);
    }
}
