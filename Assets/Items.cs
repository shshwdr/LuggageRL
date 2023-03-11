using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Stone : GridItemCore
{
    public override void hitBorder(List<BattleMessage> messages, Vector2Int borderIndex)
    {
        int damage = info.Param1;
        int moveDamageScale = info.Param2;
        var originIndex = index;
        int diff = (int)(borderIndex - originIndex).magnitude;
        if (movedCount > 0)
        {
            messages.Add(new MessageItemAttack { item = this, damage = CalculateDamage(damage) * moveDamageScale, index = index });
        }
        else
        {
            messages.Add(new MessageItemAttack { item = this, damage = CalculateDamage(damage), index = index });
        }
        buffs.Clear();
        //FloatingTextManager.Instance.addText("Attack!", transform.position);
        //Luggage.Instance.DoDamage(1);
    }
}
[System.Serializable]
public class Potion : GridItemCore
{
    public override void beCrushed(IGridItem item, List<BattleMessage> messages)
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_break, Vector3.zero);
        var healAmount = info.Param1;
        messages.Add(new MessageItemHeal { item = this, amount = healAmount, target = BattleManager.Instance.player, index = index });
        this.addDestroyMessage(messages);
        //BattleManager.Instance.player.Heal(3);
        //FloatingTextManager.Instance.addText("Heal!", transform.position);
        //destory();
    }
}
[System.Serializable]
public class Arrow : GridItemCore
{
    public override void hitBorder(List<BattleMessage> messages, Vector2Int borderIndex)
    {
        //FloatingTextManager.Instance.addText("Arrowwww!", transform.position,Color.red);

        //Luggage.Instance.DoDamage(attack * movedCount);
        var attack = info.Param1;
        var originIndex = index;
        int diff = (int)(borderIndex - originIndex).magnitude;

        var dir = (borderIndex - originIndex) / diff;
        //Debug.Log("diff " + diff);
        messages.Add(new MessageItemAttack { item = this, damage = CalculateDamage(attack) * (movedCount + 1), skipAnim = true, index = index });
        messages.Add(new MessageItemMove { item = this, index = index });
        this.addDestroyMessageWithIndex(messages, originIndex, true);
        index = borderIndex + dir * 10; ;
    }

    public override void move(List<BattleMessage> messages)
    {
        base.move(messages);

    }
}
[System.Serializable]
public class Poison : GridItemCore
{
    public override void beCrushed(IGridItem item, List<BattleMessage> messages)
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_break, Vector3.zero);
        var value = info.Param1;
        messages.Add(new MessageItemApplyEffect { item = this, index = index, target = item, type = BuffType.poison, value = value, targetIndex = item.index });
        //messages.Add(new MessageItemAttack { item = this, damage = attack, index = index });
        this.addDestroyMessage(messages);
    }
}

[System.Serializable]
public class PiggyBank : GridItemCore
{
    public override void beCrushed(IGridItem item, List<BattleMessage> messages)
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_break, Vector3.zero);
        messages.Add(new MessageItemApplyEffect { item = this, index = index, target = item, type = BuffType.piggyBank, value = count, targetIndex = item.index });
        //messages.Add(new MessageItemAttack { item = this, damage = attack, index = index });
        this.addDestroyMessage(messages);
    }
    public override void afterTurn(List<BattleMessage> messages)
    {
        var value = info.Param1;
        count += value;
        messages.Add(new MessageItemChangeCounter { item = this, index = index, amount = value });
    }


}


[System.Serializable]
public class Pins : GridItemCore
{
    public override void hitBorder(List<BattleMessage> messages, Vector2Int borderIndex)
    {

        int damage = info.Param1;
        int pinCountDamageIncrease = info.Param2;
        var originIndex = index;
        int diff = (int)(borderIndex - originIndex).magnitude;

        var dir = (borderIndex - originIndex) / diff;
        messages.Add(new MessageItemAttack { item = this, damage = CalculateDamage(damage) ^ GridManager.Instance.pinsCount, index = index });
        messages.Add(new MessageItemMove { item = this, index = index });
        this.addDestroyMessageWithIndex(messages, originIndex, true);
        buffs.Clear();
        GridManager.Instance.pinsCount++;
        index = borderIndex + dir * 10;
    }
}

[System.Serializable]
public class Circuit : GridItemCore {
    public override void hitBorder(List<BattleMessage> messages, Vector2Int borderIndex)
    {
        
        int damage = info.Param1;
        int damage2 = info.Param2;
        var originIndex = index;
        int diff = (int)(borderIndex - originIndex).magnitude; 
        var itemConnected = GridManager.Instance.getItemsWithTypeAround(index,true, "Breakable");
        messages.Add(new MessageItemAttack { item = this, damage = CalculateDamage(damage), index = index });
        foreach (var item in itemConnected)
        {

            messages.Add(new MessageItemAttack { item = item.Core, damage = CalculateDamage(damage), index = index,skipAnim = true });
            messages.Add(new MessageItemVisualEffect { item = item.Core, index = item.index,effect = VisualEffect.electric });
            //item.addDestroyMessageWithIndex(messages, originIndex, true);
        }
        this.addDestroyMessage(messages);
    }

}

[System.Serializable]
public class Coke : GridItemCore
{

    public override void hitBorder(List<BattleMessage> messages, Vector2Int borderIndex)
    {
        int damage = info.Param1;
        int healAmount = info.Param2;
        var originIndex = index;
        int diff = (int)(borderIndex - originIndex).magnitude;
        if (movedCount > 0)
        {
            messages.Add(new MessageItemAttack { item = this, damage = CalculateDamage(damage), index = index });
        }
        else
        {

            messages.Add(new MessageItemHeal { item = this, amount = healAmount, target = BattleManager.Instance.player, index = index });
        }
        this.addDestroyMessage(messages);
        //FloatingTextManager.Instance.addText("Attack!", transform.position);
        //Luggage.Instance.DoDamage(1);
    }
}

[System.Serializable]
public class Bomb : GridItemCore
{
    public override void init()
    {
        count = info.Param2;
    }
    public override void hitBorder(List<BattleMessage> messages, Vector2Int borderIndex)
    {
        int damage = info.Param1;
        var originIndex = index;
        int diff = (int)(borderIndex - originIndex).magnitude;
        messages.Add(new MessageItemAttack { item = this, damage = CalculateDamage(damage) * 5, index = index });
        this.addDestroyMessage(messages);
    }

    public override void afterTurn(List<BattleMessage> messages)
    {
        count -= 1;
        int damage = info.Param1;
        messages.Add(new MessageItemChangeCounter { item = this, index = index, amount = -1 });

        if (count == 0)
        {

            messages.Add(new MessageItemVisualEffect { item = this, index = index, effect = VisualEffect.explode });
            messages.Add(new MessageAttackPlayer { item = this, index = index, amount = CalculateDamage(damage) * 5, target = BattleManager.Instance.player });

            this.addDestroyMessage(messages);
        }
    }

}

[System.Serializable]
public class CreditCard : GridItemCore
{
    public override void afterAttack(List<BattleMessage> messages)
    {
        int amount = info.Param1;
        int count = 0;
        foreach (var item in GridManager.Instance.GridItemDict.Values)
        {
            if (!item.IsDestroyed)
            {
                count++;
            }
        }
        if (count == 1)
        {
            messages.Add(new MessageDrawItem { index = index, amount = amount });
            this.addDestroyMessage(messages);

        }
    }
}

[System.Serializable]
public class Umbrella : GridItemCore
{

    public override void hitBorder(List<BattleMessage> messages, Vector2Int borderIndex)
    {
        int damage = info.Param1;
        var originIndex = index;
        int diff = (int)(borderIndex - originIndex).magnitude;
        var emptySlotCount = 12 - GridManager.Instance.GridItemDict.Count;
        messages.Add(new MessageItemAttack { item = this, damage = CalculateDamage(emptySlotCount * damage), index = index });
        this.addDestroyMessage(messages);
    }
}

[System.Serializable]
public class Slingshot : GridItemCore
{
    public override void hitBorder(List<BattleMessage> messages, Vector2Int borderIndex)
    {
        int damage = info.Param1;
        int damage2 = info.Param2;
        var originIndex = index;
        int diff = (int)(borderIndex - originIndex).magnitude;
        var itemCountBehind = GridManager.Instance.getEmptysBehind(index, index - borderIndex);
        messages.Add(new MessageItemAttack { item = this, damage = CalculateDamage(damage) + damage2 * CalculateDamage(damage) * itemCountBehind, index = index });
        this.addDestroyMessage(messages);
    }
}

[System.Serializable]
public class Balancer : GridItemCore {
    public override void afterAttack(List<BattleMessage> messages)
    {
        if(movedCount == 0)
        {
            var originIndex = index;
                if (GridManager.Instance.isPredict)
                {
                    foreach (var item in GridManager.Instance.predictDict.Values)
                    {
                        if (!item.IsDestroyed)
                        {
                            messages.Add(new MessageItemApplyEffect { item = this, index = index, target = item, type = BuffType.balancer, value = info.Param1, targetIndex = GridManager.Instance.originalItemToPredictItem[  item].index });
                        }
                    }
                }
                else
            {
                foreach (var item in GridManager.Instance.GridItemDict.Values)
                {
                    if (!item.IsDestroyed)
                    {
                        messages.Add(new MessageItemApplyEffect { item = this, index = index, target = item, type = BuffType.balancer, value = info.Param1, targetIndex = item.index });
                    }
                }

            }
            addDestroyMessageWithIndex(messages, originIndex, true);
        }
    }
}

[System.Serializable]
public class Rocket : GridItemCore
{
    public override void hitBorder(List<BattleMessage> messages, Vector2Int borderIndex)
    {
        int damage = info.Param1;
        int damage2 = info.Param2;
        var originIndex = index;
        int diff = (int)(borderIndex - originIndex).magnitude;
        var dir = (borderIndex - originIndex) / diff;
        var itemBehind = GridManager.Instance.getItemsBehind(index, index - borderIndex);
        foreach(var item in itemBehind)
        {
            item.addDestroyMessageWithIndex(messages, originIndex, true);
        }
        messages.Add(new MessageItemAttack
        {
            item = this,
            damage = CalculateDamage(damage) + damage2 * CalculateDamage(damage) * itemBehind.Count,
            index = index
        });

        messages.Add(new MessageItemMove { item = this, index = index });
        this.addDestroyMessageWithIndex(messages, originIndex, true);
        index = borderIndex + dir * 10; ;
    }
}

[System.Serializable]
public class Pinata : GridItemCore
{

    public override void beCrushed(IGridItem item, List<BattleMessage> messages)
    {

        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_break, Vector3.zero);
        var drawAmount = info.Param1;

        messages.Add(new MessageDrawItem { index = index, amount = drawAmount });
        this.addDestroyMessage(messages);
        //BattleManager.Instance.player.Heal(3);
        //FloatingTextManager.Instance.addText("Heal!", transform.position);
        //destory();
    }
}

[System.Serializable]
public class Mud : GridItemCore
{

    public override void beCrushed(IGridItem item, List<BattleMessage> messages)
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_break, Vector3.zero);
        this.addDestroyMessage(messages);
    }
}
[System.Serializable]
public class LiquidBomb : GridItemCore
{

    public override void beCrushed(IGridItem item, List<BattleMessage> messages)
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_break, Vector3.zero);
        messages.Add(new MessageItemVisualEffect { item = this, index = index, effect = VisualEffect.explode });
        if (GridManager.Instance.isPredict)
        {
            foreach (var i in GridManager.Instance.predictDict.Values)
            {
                i.addDestroyMessage(messages);
            }
        }
        else
        {
            foreach (var i in  GridManager.Instance.GridItemDict.Values)
            {
                i.addDestroyMessage(messages);

            }
        }
        messages.Add(new MessageWait { waitTime = GridManager.animTime });

        //this.addDestroyMessage(messages);
    }
}


[System.Serializable]
public class FreezeBomb : GridItemCore
{

    public override void beCrushed(IGridItem item, List<BattleMessage> messages)
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_break, Vector3.zero);
        messages.Add(new MessageItemVisualEffect { item = this, index = index, effect = VisualEffect.explode });
        if (GridManager.Instance.isPredict)
        {
            foreach (var i in GridManager.Instance.predictDict.Values)
            {
                i.addDestroyMessage(messages);
            }
        }
        else
        {
            foreach (var i in GridManager.Instance.GridItemDict.Values)
            {
                i.addDestroyMessage(messages);

            }
        }
        messages.Add(new MessageWait { waitTime = GridManager.animTime });

        //this.addDestroyMessage(messages);
    }
}
