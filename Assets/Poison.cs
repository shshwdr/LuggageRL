using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : GridItem
{
    int attack = 3;
    public override void beCrushed(GridItem item, List<BattleMessage> messages)
    {
        messages.Add(new MessageItemAttack { item = this, damage = 3 });
        this.addDestroyMessage(messages);
    }
}
