using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herb : GridItem
{
    public override void beCrushed(GridItem item, List<BattleMessage> messages) {
        messages.Add(new MessageItemHeal { item = this, amount = 3, target = BattleManager.Instance.player });
        this.addDestroyMessage(messages);
        //BattleManager.Instance.player.Heal(3);
        //FloatingTextManager.Instance.addText("Heal!", transform.position);
        //destory();
    }
}
