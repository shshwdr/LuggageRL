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
        messages.Add(new MessageItemAttack { item = this, damage = CalculateDamage(damage) + pinCountDamageIncrease * GridManager.Instance.pinsCount, index = index });
        messages.Add(new MessageItemMove { item = this, index = index });
        this.addDestroyMessageWithIndex(messages, originIndex, true);
        buffs.Clear();
        GridManager.Instance.pinsCount++;
        index = borderIndex + dir * 10;
    }
}


[System.Serializable]
public class Circuit : GridItemCore { }

[System.Serializable]
public class Coke : GridItemCore {

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
public class Bomb : GridItemCore {
    public override void init()
    {
        count = info.Param2;
    }
    public override void hitBorder(List<BattleMessage> messages, Vector2Int borderIndex)
    {
        int damage = info.Param1;
        var originIndex = index;
        int diff = (int)(borderIndex - originIndex).magnitude;
            messages.Add(new MessageItemAttack { item = this, damage = CalculateDamage(damage), index = index });
        this.addDestroyMessage(messages);
    }

    public override void afterTurn(List<BattleMessage> messages)
    {
        count -= 1;
        int damage = info.Param1;
        messages.Add(new MessageItemChangeCounter { item = this, index = index, amount = -1 });

        if(count == 0)
        {
            messages.Add(new MessageAttackPlayer { item = this, index = index, amount = CalculateDamage(damage), target = BattleManager.Instance.player });

            this.addDestroyMessage(messages);
        }
    }

}

[System.Serializable]
public class CreditCard : GridItemCore { }

[System.Serializable]
public class Umbrella : GridItemCore { }

[System.Serializable]
public class Slingshot : GridItemCore { }

[System.Serializable]
public class Balancer : GridItemCore { }

[System.Serializable]
public class Rocket : GridItemCore { }

[System.Serializable] 
public class Pinata : GridItemCore { }

