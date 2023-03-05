using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : GridItem
{
    public int attack = 3;
    public override string Desc => $@"{base.Desc}
Deal {attack} damage when hit the border";
    public override void beCrushed(GridItem item, List<BattleMessage> messages)
    {
        messages.Add(new MessageItemAttack { item = this, damage = attack });
        this.addDestroyMessage(messages);
    }
}
