using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Herb : GridItemCore
{
    public int healAmount = 3;
    public override string Desc => $@"{base.Desc}
Heal {healAmount} when being hit by another item.";
    public override void beCrushed(IGridItem item, List<BattleMessage> messages) {
        messages.Add(new MessageItemHeal { item = this, amount = healAmount, target = BattleManager.Instance.player, index = index });
        this.addDestroyMessage(messages);
        //BattleManager.Instance.player.Heal(3);
        //FloatingTextManager.Instance.addText("Heal!", transform.position);
        //destory();
    }
}
