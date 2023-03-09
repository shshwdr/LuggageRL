using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Herb : GridItemCore
{
    public override void beCrushed(IGridItem item, List<BattleMessage> messages) {
        var healAmount = info.Param1;
        messages.Add(new MessageItemHeal { item = this, amount = healAmount, target = BattleManager.Instance.player, index = index });
        this.addDestroyMessage(messages);
        //BattleManager.Instance.player.Heal(3);
        //FloatingTextManager.Instance.addText("Heal!", transform.position);
        //destory();
    }
}
