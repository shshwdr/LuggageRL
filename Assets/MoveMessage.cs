using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMessage
{
    public bool skipAnim = false;
}

public class MessageMove : BattleMessage
{
    public Dictionary<GridItem, Vector2Int> itemTargetIndex = new Dictionary<GridItem, Vector2Int>();
}
public class MessageItemMove : BattleMessage
{
    public GridItem item;
}

public class MessageItemHitBorder : BattleMessage
{
    public GridItem item;
}
public class MessageItemBeCrushed : BattleMessage
{
    public GridItem item;
}
public class MessageItemAttack : BattleMessage
{
    public GridItem item;
    public int damage;
    public HPObject target;
}
public class MessageItemHeal : BattleMessage
{
    public GridItem item;
    public int amount;
    public HPObject target;
}
public class MessageShowPopup : BattleMessage {
    public string str;

}
public class MessageDestroy : BattleMessage {

    public GridItem item;
}

