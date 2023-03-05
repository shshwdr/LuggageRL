using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleMessage { }

public class MessageMove : IBattleMessage
{
    public Dictionary<GridItem, Vector2Int> itemTargetIndex = new Dictionary<GridItem, Vector2Int>();
}
public class MessageItemMove : IBattleMessage
{
    public Dictionary<GridItem, Vector2Int> itemTargetIndex = new Dictionary<GridItem, Vector2Int>();
}

public class MessageItemHitBorder : IBattleMessage
{
    public GridItem item;
}
public class MessageItemBigHitBorder : IBattleMessage
{
    public GridItem item;
}
public class MessageItemBeCrushed : IBattleMessage
{
    public GridItem item;
}
public class MessageItemAttack : IBattleMessage
{
    public GridItem item;
    public int damage;
    public HPObject target;
}
public class MessageItemHeal : IBattleMessage
{
    public GridItem item;
    public int amount;
    public HPObject target;
}
public class MessageShowPopup : IBattleMessage {
    public string str;

}
public class MessageDestroy : IBattleMessage {

    public GridItem item;
}

