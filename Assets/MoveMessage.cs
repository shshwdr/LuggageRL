using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMessage
{
    public bool skipAnim = false;
}

public class MessageMove : BattleMessage
{
    public Dictionary<GridItemCore, Vector2Int> itemTargetIndex = new Dictionary<GridItemCore, Vector2Int>();
}
public class MessageItemMove : BattleMessage
{
    public Vector2Int index;
    public GridItemCore item;
}

public class MessageItemAttack : BattleMessage
{
    public Vector2Int index;
    public GridItemCore item;
    public int damage;
    public HPObject target;
}

public class MessageItemApplyEffect : BattleMessage
{
    public Vector2Int index;
    public Vector2Int targetIndex;
    public IGridItem item;
    public BuffType type;
    public int value;
    public IGridItem target;
}
public class MessageItemChangeCounter : BattleMessage
{
    public Vector2Int index;
    public GridItemCore item;
    public int amount;
}
public class MessageItemHeal : BattleMessage
{
    public Vector2Int index;
    public GridItemCore item;
    public int amount;
    public HPObject target;
}
public class MessageShowPopup : BattleMessage {
    public string str;

}
public class MessageDestroy : BattleMessage {
    public Vector2Int index;
    public GridItemCore item;
}

