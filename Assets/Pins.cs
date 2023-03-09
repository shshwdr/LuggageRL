using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class Circuit : GridItemCore { }
public class Coke : GridItemCore { }
public class Bomb : GridItemCore { }
public class CreditCard : GridItemCore { }
public class Umbrella : GridItemCore { }
public class Slingshot : GridItemCore { }
public class Balancer : GridItemCore { }
public class Rocket : GridItemCore { }
public class Pinata : GridItemCore { }

