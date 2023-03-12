using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public enum ItemType
{
    Stone, Potion, Arrow, Poison, Circuit,
    Pins,
    PiggyBank,
    Coke,
    Bomb,
    CreditCard,
    Umbrella,
    Slingshot,
    Balancer,
    Rocket,
    Pinata,

    Mud,
LiquidBomb,
FreezeBomb,

}
public class GridManager : Singleton<GridManager>
{

    static public float animTime = 0.5f;
    public float tileSizey = 2f;
    public float tileSizex = 2f;
    public int Rows = 2;
    public int Columns = 3;
    public GameObject EmptyGridPrefab;
    public GameObject itemPrefab;
    public Vector3 startPosition;
    public Transform bk;
    public Transform items;
    public Dictionary<Vector2Int, GridItem> GridItemDict = new Dictionary<Vector2Int, GridItem>();
    public List<GridEmptyCell> emptyGridList = new List<GridEmptyCell>();
    public Text itemViewText;

    int drawCounts = 3;
    int DrawCount => drawCounts + LuggageManager.Instance.UpgradedTime[UpgradeType.actionCount];
    public bool CanDraw(out string failedReason, int drawCount)
    {
        failedReason = "";
        List<Vector2Int> availableEmpty = new List<Vector2Int>();
        foreach (var key in emptyGridList)
        {
            if (!GridItemDict.ContainsKey(key.index))
            {
                availableEmpty.Add(key.index);
            }
        }

        if (availableEmpty.Count < drawCount)
        {
            failedReason = "Bag is Full!";
            return false;
        }
        return true;
    }

    public ItemType findMostItem()
    {
        if(GridItemDict.Count == 0)
        {
            return ItemType.Stone;
        }
        Dictionary<ItemType, int> dic = new Dictionary<ItemType, int>();
        foreach(var item in GridItemDict.Values)
        {
            if (dic.ContainsKey(item.type))
            {
                dic[item.type]++;
            }
            else
            {
                dic[item.type] = 1;
            }
        }
        var max = dic.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        return max;
    }
    public void addItemToDeck(ItemType type)
    {
        deckPool.Add(type);
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_take_new, new Vector3(0, 0, 0));

    }
    List<ItemType> deckPool = new List<ItemType>() { ItemType.Stone, ItemType.Stone, ItemType.Arrow};
    //{ ItemType.ore, ItemType.ore, ItemType.herb, ItemType.herb, ItemType.arrow, ItemType.poison, ItemType.poison };
    //{ ItemType.ore, ItemType.ore, ItemType.ore, ItemType.herb, ItemType.herb, ItemType.herb, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.poison, ItemType.poison, ItemType.poison };
    //{ ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.arrow, };
    //{ ItemType.ore, ItemType.ore, ItemType.ore, ItemType.herb, ItemType.herb, ItemType.herb, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.poison, ItemType.poison, ItemType.poison };
    public IEnumerator DrawItem(int drawCount, bool shouldDrop)
    {
        List<Vector2Int> availableEmpty = new List<Vector2Int>();
        foreach (var key in emptyGridList)
        {
            if (!GridItemDict.ContainsKey(key.index))
            {
                availableEmpty.Add(key.index);
            }
        }
        for (int i = 0; i < drawCount; i++)
        {
            if (availableEmpty.Count == 0)
            {
                FloatingTextManager.Instance.addText("Bag is Full!", Vector3.zero, Color.white);
                break;
            }
            if (deckPool.Count == 0)
            {
                break;
            }
            var picked = availableEmpty[Random.Range(0, availableEmpty.Count)];
            var pickedType = deckPool[Random.Range(0, deckPool.Count)];

            Debug.Log("draw " + pickedType.ToString());
            deckPool.Remove(pickedType);
            AddGrid(picked.x, picked.y, pickedType);
            availableEmpty.Remove(picked);
        }
        if (shouldDrop)
        {
            yield return StartCoroutine(MoveAfter(0, -1));
            BattleManager.Instance.PredictNextAttack();
        }
    }

    public GameObject AddItemRandomPosition(ItemType type)
    {
        List<Vector2Int> availableEmpty = new List<Vector2Int>();
        foreach (var key in emptyGridList)
        {
            if (!GridItemDict.ContainsKey(key.index))
            {
                availableEmpty.Add(key.index);
            }
        }
        if(availableEmpty .Count > 0)
        {
            var picked = availableEmpty[Random.Range(0, availableEmpty.Count)];

            return AddGrid(picked.x, picked.y, type);
        }
        else
        {
            return null;
        }
    }

    

    public List<GridItem> GetItemOfType(ItemType type)
    {
        List<GridItem> res = new List<GridItem>();
        foreach(var item in GridItemDict.Values)
        {
            if(item.type == type)
            {
                res.Add(item);
            }
        }
        return res;
    }

    public void RemoveAll()
    {
        foreach(var item in GridItemDict.Values.ToList())
        {
            RemoveGrid(item);
        }
        foreach(var key in GridItemDict.Keys.ToList())
        {
            GridItemDict.Remove(key);
        }
    }

    public IEnumerator DrawAllItemsFromPool()
    {
        yield return StartCoroutine(DrawItem(deckPool.Count,true));
    }


    public IEnumerator DrawItemsFromPool()
    {
        yield return StartCoroutine(DrawItem(DrawCount,true));
        //List<Vector2Int> availableEmpty = new List<Vector2Int>();
        //foreach (var key in emptyGridList)
        //{
        //    if (!GridItemDict.ContainsKey(key.index))
        //    {
        //        availableEmpty.Add(key.index);
        //    }
        //}
        //foreach(var item in deckPool)
        //{
        //    var picked = availableEmpty[Random.Range(0, availableEmpty.Count)];
        //    AddGrid(picked.x, picked.y, item);
        //    availableEmpty.Remove(picked);
        //}

        //deckPool.Clear();

        //yield return StartCoroutine(MoveAfter(0, -1));

        //BattleManager.Instance.PredictNextAttack();
    }
    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 5;
        if (GameManager.Instance.preselectedItems.Count > 0)
        {
            for (int i = 0;i< GameManager.Instance.preselectedItems.Count; i++)
            {
                deckPool[i] = GameManager.Instance.preselectedItems[i];
            }
        }


        GenerateGrid();

        //AddGrid(1, 1, ItemType.herb);
        //AddGrid(1, 2, ItemType.ore);
        //AddGrid(2, 0, ItemType.herb);
        //AddGrid(2, 2, ItemType.herb);


        //StartCoroutine(MoveAfter(0, -1));

    }
    public int rotatedTime = 0;
    public void Rotate(int time, bool applyGravity)
    {
        rotatedTime += time;
        rotatedTime %= 4;
        //transform.eulerAngles = new Vector3(0, 0, 90 * rotatedTime);
        transform.DORotate(new Vector3(0, 0, 90 * rotatedTime), animTime);
        if (applyGravity)
        {
            StartCoroutine(MoveAfter(0, -1));
        }
    }

    public IEnumerator MoveAfter(int x, int y)
    {
        //Debug.Log("move after");
        //yield return new WaitForSeconds(animTime);
        yield return MoveEnumerator(x, y, false);
    }
    List<Transform> sortEmptyCells()
    {
        var myList = new List<Transform>();
        foreach (var key in emptyGridList)
        {
            myList.Add(key.transform);
        }
        myList.Sort(delegate (Transform a, Transform b)
        {
            if (Mathf.Approximately(a.position.x, b.position.x))
            {
                return a.position.y.CompareTo(b.position.y);
            }
            else
            {
                return b.position.x.CompareTo(a.position.x);
            }
        });
        return myList;
    }

    public List<Vector2Int> getFrontCellsIndexFromBottomToTop()
    {
        var myList = sortEmptyCells();

        List<Vector2Int> res = new List<Vector2Int>();
        var x = myList[0].position.x;
        for (int i = 0; i < 10; i++)
        {
            if (Mathf.Approximately(myList[i].position.x, x))
            {
                res.Add(myList[i].GetComponent<GridEmptyCell>().index);
            }
        }
        return res;
    }

    public List<Transform> getFrontCellsFromBottomToTop()
    {
        var myList = sortEmptyCells();

        List<Transform> res = new List<Transform>();
        var x = myList[0].position.x;
        for (int i = 0; i < 10; i++)
        {
            if (Mathf.Approximately(myList[i].position.x, x))
            {
                res.Add(myList[i]);
            }
        }
        return res;
    }
    List<GameObject> previewCells = new List<GameObject>();
    public GameObject gridPreviewCell;
    public void showAttackPreviewOfEnemy(Enemy enemy)
    {


        if (enemy.Core.willAttacking)
        {
            var cells = getFrontCellsFromBottomToTop();
            Transform cell = null;
            if (enemy.attackFromBottom)
            {
                cell = cells[enemy.attackInd];
            }
            else
            {

                cell = cells[cells.Count - 1];
            }

            var attackIndex = cell.GetComponent<GridEmptyCell>().index;

            if (GridManager.Instance.GridItemDict.ContainsKey(attackIndex))
            {
                GridManager.Instance.GridItemDict[attackIndex].baseItem.WillBeAttacked();
            }
            else
            {
                var go = Instantiate(gridPreviewCell, cell.position, cell.rotation);
                previewCells.Add(go);
            }

        }
    }

    public void cleanAndShowAttackPreviewOfEnemy(Enemy enemy)
    {

        clearAttackPreview();
        showAttackPreviewOfEnemy(enemy);
    }

    public GridItem itemEnemyAttack(Enemy enemy)
    {
        List<GridItem> res = new List<GridItem>();
        var cells = getFrontCellsIndexFromBottomToTop();
        if (enemy.attackFromBottom)
        {
            var cell = cells[enemy.attackInd];
            var item = GridItemDict.ContainsKey(cell) ? GridItemDict[cell].GetComponent<GridItem>() : null;
            return item;
        }
        return null;
    }
    public void showAllAttackPreview()
    {
        clearAttackPreview();
        foreach(var enemy in EnemyManager.Instance.GetEnemies())
        {
            showAttackPreviewOfEnemy(enemy);
        }
    }
    public void clearAttackPreview()
    {
        foreach(var cell in GridItemDict.Values)
        {
            if(cell == null)
            {
                Debug.Log("error");
            }
            cell.baseItem.ClearWillBeAttacked();
        }
        foreach (var cell in previewCells)
        {
            Destroy(cell);
        }
        previewCells.Clear();
    }

    List<BattleMessage> messages = new List<BattleMessage>();

    Vector2Int rotateMoveVectorBasedOnRotateTime(int x, int y)
    {
        var moveVector = new Vector2Int(x, y);
        var tempVector = (transform.rotation * (Vector2)moveVector);
        if (rotatedTime == 1 || rotatedTime == 3)
        {
            tempVector = -tempVector;
        }
        moveVector = new Vector2Int((int)tempVector.x, (int)tempVector.y);
        return moveVector;
    }

    static List<Vector2Int> sortItemIndex<T>(Vector2Int moveVector, Dictionary<Vector2Int, T> gridItemDict) where T : IGridItem
    {

        var x = moveVector.x;
        var y = moveVector.y;

        var allItemIndex = new List<Vector2Int>();
        foreach (var key in gridItemDict.Keys)
        {
            allItemIndex.Add(key);
        }
        allItemIndex.Sort(delegate (Vector2Int a, Vector2Int b)
        {
            if (x == 1)
            {
                if(b.x == a.x)
                {
                    return b.y.CompareTo(a.y); //still try to follow an order
                }
                else
                {
                    return b.x.CompareTo(a.x); //have to follow this order
                }
            }
            else if (x == -1)
            {
                if (b.x == a.x)
                {
                    return b.y.CompareTo(a.y);
                }
                else
                {
                    return a.x.CompareTo(b.x);
                }
            }
            else if (y == 1)
            {
                if (b.y == a.y)
                {
                    return b.x.CompareTo(a.x);
                }
                else
                {
                    return b.y.CompareTo(a.y);
                }

            }
            else
            {

                if (b.y == a.y)
                {
                    return b.x.CompareTo(a.x);
                }
                else
                {
                    return a.y.CompareTo(b.y);
                }
            }
        });
        return allItemIndex;
    }
    public bool isPredict = false;

    public List<IGridItem> getItemsWithTypeAround(Vector2Int targetInput , bool isType, string typeName)
    {
        List<IGridItem> res = new List<IGridItem>();
        List<Vector2Int> checkList = new List<Vector2Int>() { targetInput };
        List<Vector2Int> visitedList = new List<Vector2Int>() { targetInput };
        while(checkList.Count>0)
        {
            var pop = checkList[checkList.Count - 1];
            checkList.RemoveAt(checkList.Count - 1);
            foreach (var dir in new List<Vector2Int>() { new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, -1), })
            {
                var target = pop+ dir;
                if (isPredict)
                {
                    if (HasItem(predictDict, target) && !visitedList.Contains(target))
                    {
                        var item = (IGridItem)GetItem(predictDict, target);
                        if (item.Core.info.Type == typeName)
                        {

                            res.Add(item);
                            visitedList.Add(target);
                            checkList.Add(target);
                        }
                    }
                }
                else
                {
                    if (HasItem(GridItemDict, target) && !visitedList.Contains(target))
                    {
                        var item = (IGridItem)GetItem(GridItemDict, target);
                        if (item.Core.info.Type == typeName)
                        {

                            res.Add(item);
                            visitedList.Add(target);
                            checkList.Add(target);
                        }
                    }
                }
            }
        }
        return res;
    }
    public List<IGridItem> getItemsBehind(Vector2Int target, Vector2Int dir)
    {
        List<IGridItem> res = new List<IGridItem>();
        for (int i = 0; i < 4; i++)
        {
            target += dir;
            if (isPredict)
            {
                if (HasItem(predictDict, target))
                    res.Add((IGridItem)GetItem(predictDict, target));
            }
            else
            {
                if (HasItem(GridItemDict, target))
                    res.Add((IGridItem)GetItem(GridItemDict, target));
            }
        }
        return res;
    }
    public int getEmptysBehind(Vector2Int target, Vector2Int dir)
    {
        int res = 0;
        for (int i = 0; i < 4; i++)
        {
            target += dir;
            if (isPredict)
            {
                if (!CanMoveTo(predictDict, target))
                {
                    return res;
                }
            }
            else
            {
                if (!CanMoveTo(GridItemDict, target))
                {
                    return res;
                }
            }
            res++;
        }
        return res;
    }
    public void MoveInternal<T>(int x, int y, bool isAttacking, Dictionary<Vector2Int, T> gridItemDict) where T : IGridItem
    {
        messages = new List<BattleMessage>();
        var moveVector = rotateMoveVectorBasedOnRotateTime(x, y);

        var sortedAllItemIndex = sortItemIndex(moveVector, gridItemDict);

        Dictionary<GridItemCore, Vector2Int> itemTargetIndex = new Dictionary<GridItemCore, Vector2Int>();
        if (!isAttacking)
        {
            messages.Add(new MessageMove { itemTargetIndex = itemTargetIndex });
        }
        foreach (var key in sortedAllItemIndex)
        {
            var newKey = key;
            int test = 10;
            var gridItem = GetItem(gridItemDict, key);
            if (gridItem == null)
            {
                Debug.Log("null obj");
            }
            while (true)
            {
                test--;
                if (test < 0)
                {
                    Debug.LogError("wrong");
                    break;
                }
                newKey += moveVector;
                if (!CanMoveTo(gridItemDict, newKey))
                {
                    if (IsHittingBoarder(newKey))
                    {
                        if (isAttacking)
                        {
                            gridItem.hitBorder(messages, newKey);
                            //messages.Add(new MessageItemHitBorder { item = gridItem });
                            //gridItem.hitBorder(true, newKey - moveVector - key, items.transform.TransformPoint(IndexToPosition(newKey)));
                        }
                    }
                    if (HasItem(gridItemDict, newKey) && !GetItem(gridItemDict, newKey).IsDestroyed)
                    {

                        if (isAttacking)
                        {
                            GetItem(gridItemDict, newKey).beCrushed(gridItem, messages);
                            //messages.Add(new MessageItemBeCrushed { item = gridItem });
                            //GetItem(newKey).GetComponent<GridItem>().BeHit(gridItem);
                        }
                    }

                    newKey -= moveVector;
                    break;
                }
                else
                {
                    gridItem.move(messages);
                }
                //when attack, only check one move
                if (isAttacking)
                {
                    break;
                }
            }
            if (newKey != key && !isAttacking)
            {
                //if item still existed, haven't been destroyed
                if (gridItemDict.ContainsKey(key))
                {
                    gridItemDict.Remove(key);
                    gridItemDict[newKey] = gridItem;
                }
                {
                    gridItem.index = newKey;
                    itemTargetIndex[gridItem.Core] = newKey;
                }
                //gridItem.move(messages);

                //Debug.Log($"move {obj.GetComponent<GridItem>().type.ToString()} from {key} to {newKey}");
            }

        }
        if (isAttacking)
        {
            foreach (var item in gridItemDict.Values)
            {
                if (!item.IsDestroyed)
                {

                    item.afterAttack(messages);
                }
            }
        }
    }

    public IEnumerator EndTurnCardBehaviorEnumerator()
    {
        messages.Clear();
        foreach (var item in GridItemDict.Values)
        {
            if (!item.IsDestroyed)
            {

                item.afterTurn(messages);
            }
        }
        yield return StartCoroutine(ParseMessages());
    }
    public void ParsePredictMessage(Dictionary<GridItemCore, GridItem> predictToOrigin)
    {
        for (int i = 0; i < messages.Count; i++)
        {
            var message = messages[i];
            if (message is MessageMove move)
            {
            }
            else if (message is MessageItemVisualEffect visualEffect)
            {

            }
            else if (message is MessageWait wait)
            {

            }
            
            else if (message is MessageItemAttack attack)
            {
                Debug.Log($"{attack.item.Name} Attack {attack.damage}");
                predictToOrigin[attack.item].baseItem.WillAttack(attack.damage);
                //Luggage.Instance.DoDamage(attack.damage);
                //Debug.Log($"{attack.item.Name} Attack {attack.damage} {attack.item.transform.position}");
                //if (attack.item.transform.position.magnitude > 10)
                //{
                //    Debug.Log("how");
                //}
                //FloatingTextManager.Instance.addText($"Attack {attack.damage}", attack.item.transform.position, Color.red);


            }
            else if (message is MessageItemApplyEffect applyEffect)
            {
                if (predictToOrigin[(GridItemCore)applyEffect.target].baseItem.GetComponent<GridItem>().core.isAttacker)
                {
                    predictToOrigin[(GridItemCore)applyEffect.target].baseItem.WillBeBuff();
                }

            }
            else if (message is MessageItemChangeCounter counterChanage)
            {

            }
            else if (message is MessageItemHeal heal)
            {
                Debug.Log($"{heal.item.Name} heal {heal.amount}");
                predictToOrigin[heal.item].baseItem.WillHeal(heal.amount);
                //heal.target.Heal(heal.amount);
                ////FloatingTextManager.Instance.addText($"Heal {heal.amount}", heal.target.transform.position,Color.green);
                //FloatingTextManager.Instance.addText($"Heal {heal.amount}", heal.item.transform.position, Color.green);

            }
            else if (message is MessageAttackPlayer attackPlayer)
            {
                Debug.Log($"{attackPlayer.item.Name} attack {attackPlayer.amount}");
                //predictToOrigin[attackPlayer.item].baseItem.WillHeal(heal.amount);
                //heal.target.Heal(heal.amount);
                ////FloatingTextManager.Instance.addText($"Heal {heal.amount}", heal.target.transform.position,Color.green);
                //FloatingTextManager.Instance.addText($"Heal {heal.amount}", heal.item.transform.position, Color.green);

            }
            else if (message is MessageDrawItem drawItem)
            {

                Debug.Log($"draw {drawItem.amount}");
            }

            else if (message is MessageDestroy destr)
            {
                Debug.Log($"{destr.item.Name} destroy");
                predictToOrigin[destr.item].baseItem.WillDestroy();
                //destr.item.destory();
                //FloatingTextManager.Instance.addText("Destroy!", destr.item.transform.position, Color.white);

            }
            else if (message is MessageItemMove itemMove)
            {
            }
        }


    }
    public IEnumerator ParseMessages()
    {
        for (int i = 0; i < messages.Count; i++)
        {

            var message = messages[i];
            if (message is MessageMove move)
            {
                if (move.itemTargetIndex.Count != 0)
                {

                    AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_slide, transform.position);
                    foreach (var pair in move.itemTargetIndex.Keys)
                    {
                        if(GridItemDict[pair.index] == null)
                        {
                            GridItemDict.Remove(pair.index);
                        }
                        UpdateItemPositionToIndexEnumerator(GridItemDict[pair.index]);
                    }

                    yield return new WaitForSeconds(animTime);
                }
            }
            else if (message is MessageItemChangeCounter counterChanage)
            {
                if (BattleManager.Instance.isBattleFinished)
                {
                    yield break;
                }

                if (!GridItemDict.ContainsKey(counterChanage.index))
                {
                    Debug.Log("?");
                }

                if (GridItemDict[counterChanage.index] == null)
                {
                    GridItemDict.Remove(counterChanage.index);
                }
                GridItemDict[counterChanage.index].UpdateCounter();
                FloatingTextManager.Instance.addText((counterChanage.amount > 0 ? "+" : "") + $"{counterChanage.amount.ToString()}", GridItemDict[counterChanage.index].transform.position, Color.yellow);
                if (!counterChanage.skipAnim)
                {
                    yield return new WaitForSeconds(animTime);
                }

            }
            else if (message is MessageItemApplyEffect applyEffect)
            {

                if (!GridItemDict.ContainsKey(applyEffect.targetIndex))
                {
                    Debug.Log("?");
                }
                if (GridItemDict[applyEffect.index] == null)
                {
                    GridItemDict.Remove(applyEffect.index);
                }
                if (GridItemDict[applyEffect.targetIndex].core.isAttacker)
                {
                    GridItemDict[applyEffect.targetIndex].ApplyBuff(applyEffect.type, applyEffect.value);
                    FloatingTextManager.Instance.addText($"Apply {applyEffect.type.ToString()}", GridItemDict[applyEffect.targetIndex].transform.position, Color.yellow);
                    //FloatingTextManager.Instance.addText($"{attack.damage}", heal.item.transform.position);
                }
                if (!applyEffect.skipAnim)
                {
                    yield return new WaitForSeconds(animTime);
                }
            }
            else if (message is MessageWait wait)
            {
                yield return new WaitForSeconds(wait.waitTime);
            }
            else if (message is MessageItemVisualEffect visualEffect)
            {

                if (!GridItemDict.ContainsKey(visualEffect.index))                {
                    Debug.Log("?");
                    continue;
                }


                if (GridItemDict[visualEffect.index] == null)
                {
                    GridItemDict.Remove(visualEffect.index);
                }
                string visualText = "";

                switch (visualEffect.effect)
                {
                    case VisualEffect.electric:
                        visualText = "Electric!";
                        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_break, Vector3.zero);
                        break;
                    case VisualEffect.explode:
                        visualText = "Boom!";
                        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_bomb_explode, Vector3.zero);
                        break;
                    case VisualEffect.crush:
                        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_break, Vector3.zero);
                        break;
                    case VisualEffect.potion:
                        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_effect_heal, Vector3.zero);
                        break;
                    case VisualEffect.arrow:
                        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_bow_and_arrow, Vector3.zero);
                        break;
                    case VisualEffect.piggy:
                        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_piggy_bank_break, Vector3.zero);
                        break;
                    case VisualEffect.surge:
                        break;
                    case VisualEffect.cash:
                        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_cash_register, Vector3.zero);
                        break;
                    case VisualEffect.heal:
                        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_effect_heal, Vector3.zero);
                        break;
                    case VisualEffect.rocket:
                        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_rocket, Vector3.zero);
                        break;
                    case VisualEffect.impact:
                        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_impact, Vector3.zero);
                        break;
                    default:
                        break;
                }

                FloatingTextManager.Instance.addText(visualText, GridItemDict[visualEffect.index].transform.position, Color.white);
                if (!visualEffect.skipAnim)
                {
                    yield return new WaitForSeconds(animTime);
                }
            }
            else if (message is MessageItemAttack attack)
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_impact, transform.position);
                
                Debug.Log("attack " + attack.item.Name);
                if (!GridItemDict.ContainsKey(attack.index))
                {
                    Debug.Log("?");
                }

                if (GridItemDict[attack.index] == null)
                {
                    GridItemDict.Remove(attack.index);
                }
                // Debug.Log($"{attack.item.Name} Attack {attack.damage} {GridItemDict[attack.index].transform.position}");

                FloatingTextManager.Instance.addText($"Attack {attack.damage}", GridItemDict[attack.index].transform.position, Color.red);
                //FloatingTextManager.Instance.addText($"{attack.damage}", heal.item.transform.position);
                if (!attack.skipAnim)
                {
                    yield return new WaitForSeconds(animTime);
                }
                yield return StartCoroutine(Luggage.Instance.DoDamage(attack.damage));

            }
            else if (message is MessageDrawItem drawItem)
            {

                if (!GridItemDict.ContainsKey(drawItem.index))
                {
                    Debug.Log("?");
                }
                if (GridItemDict[drawItem.index] == null)
                {
                    GridItemDict.Remove(drawItem.index);
                }
                //FloatingTextManager.Instance.addText($"Heal {heal.amount}", heal.target.transform.position,Color.green);
                FloatingTextManager.Instance.addText($"Draw {drawItem.amount} Items", GridItemDict[drawItem.index].transform.position, Color.blue);
                yield return new WaitForSeconds(animTime);
                yield return StartCoroutine(DrawItem(drawItem.amount,false));
            }
            else if (message is MessageAttackPlayer attackPlayer)
            {

                if (!GridItemDict.ContainsKey(attackPlayer.index))
                {
                    Debug.Log("?");
                }
                
                if (GridItemDict[attackPlayer.index] == null)
                {
                    GridItemDict.Remove(attackPlayer.index);
                }
                //FloatingTextManager.Instance.addText($"Heal {heal.amount}", heal.target.transform.position,Color.green);
                FloatingTextManager.Instance.addText($"Self Damage {attackPlayer.amount}", GridItemDict[attackPlayer.index].transform.position, Color.red);
                yield return new WaitForSeconds(animTime);
                yield return StartCoroutine(attackPlayer.target.ApplyDamage(attackPlayer.amount));

            }
            else if (message is MessageItemHeal heal)
            {
                if (!GridItemDict.ContainsKey(heal.index))
                {
                    Debug.Log("?");
                }
                if (GridItemDict[heal.index] == null)
                {
                    GridItemDict.Remove(heal.index);
                }
                //FloatingTextManager.Instance.addText($"Heal {heal.amount}", heal.target.transform.position,Color.green);
                FloatingTextManager.Instance.addText($"Heal {heal.amount}", GridItemDict[heal.index].transform.position, Color.green);
                yield return new WaitForSeconds(animTime);

                yield return StartCoroutine( heal.target.HealEnumerator(heal.amount));
            }
            else if (message is MessageDestroy destr)
            {
                if (!GridItemDict.ContainsKey(destr.index))
                {
                    Debug.Log("?");
                }
                if (GridItemDict[destr.index] == null)
                {
                    GridItemDict.Remove(destr.index);
                }
                FloatingTextManager.Instance.addText("Destroy!", GridItemDict[destr.index].transform.position, Color.white);
                GridItemDict[destr.index].destory();
                GridManager.Instance.RemoveGrid(destr.index, destr.item.type);
                if (!destr.skipAnim)
                {
                    yield return new WaitForSeconds(animTime);
                }
            }
            else if (message is MessageItemMove itemMove)
            {
                if (!GridItemDict.ContainsKey(itemMove.index))
                {
                    Debug.Log("?");
                }
                if (GridItemDict[itemMove.index] == null)
                {
                    GridItemDict.Remove(itemMove.index);
                }
                UpdateItemPositionToIndexEnumerator(GridItemDict[itemMove.index]);
                yield return new WaitForSeconds(animTime);
            }
        }
    }

    public IEnumerator EnemyAttackEnumerator(Enemy enemy)
    {
        messages.Clear();
        var damage = enemy.attack;
        var item = GridManager.Instance.itemEnemyAttack(enemy);
        if (item != null)
        {
            damage -= item.core.defense;
            damage = Mathf.Max(0, damage);
            item.addDestroyMessage(messages);

            yield return StartCoroutine(ParseMessages());

            yield return StartCoroutine(MoveAfter(0, -1));


            BattleManager.Instance.PredictNextAttack();
        }

        yield return StartCoroutine(BattleManager.Instance.player.ApplyDamage(damage));
    }
    public List<GameObject> attackingEdges;
    public void updateAttackEdge()
    {
        foreach (var edge in attackingEdges)
        {
            edge.SetActive(false);
        }
        var nextAttackEdge = (BattleManager.Instance.getCurrentAttackRotationId() - rotatedTime + 4) % 4;
        attackingEdges[nextAttackEdge].SetActive(true);
    }

    public int pinsCount = 0;

    void clearBeforeAttack()
    {
        pinsCount = 0;
        messages.Clear();
    }
    public IEnumerator MoveAndAttack(int x, int y)
    {
        clearBeforeAttack();
        foreach (var item in GridItemDict.Values)
        {
            item.GetComponent<GridItem>().finishedAttack();
        }

        //move all tiles to the edge

        MoveInternal(x, y, false, GridItemDict);
        yield return StartCoroutine(ParseMessages());
        //attack one step

        MoveInternal(x, y, true, GridItemDict);
        yield return StartCoroutine(ParseMessages());

        yield return StartCoroutine(MoveAfter(0, -1));

        updateAttackEdge();

        BattleManager.Instance.PredictNextAttack();

    }
    public Dictionary<Vector2Int, GridItemCore> predictDict;
    public Dictionary<GridItemCore, GridItem> originalItemToPredictItem;
    public void predict(int x, int y)
    {
        isPredict = true;
        clearBeforeAttack();
        // we need to copy a GridItemDict, create a map between original grid and new ones
        // move and attack using it
        predictDict = new Dictionary<Vector2Int, GridItemCore>();
        originalItemToPredictItem = new Dictionary<GridItemCore, GridItem>();
        foreach (var pair in GridItemDict)
        {
            pair.Value.baseItem.ClearOverlayes();
            predictDict[pair.Key] = Ut.DeepClone(pair.Value.core);
            predictDict[pair.Key].movedCount = 0;
            originalItemToPredictItem[predictDict[pair.Key]] = pair.Value;
        }
        MoveInternal(x, y, false, predictDict);
        //ParsePredictMessage(originalItemToPredictItem);
        MoveInternal(x, y, true, predictDict);
        //read message and update damage it would made on the item, show a red overlay to it?
        ParsePredictMessage(originalItemToPredictItem);
        isPredict = false;
    }
    public IEnumerator MoveEnumerator(int x, int y, bool isAttacking)
    {
        messages.Clear();
        MoveInternal(x, y, isAttacking, GridItemDict);
        yield return StartCoroutine(ParseMessages());
        //MoveInternal(x, y,true);
        if (isAttacking)
        {
            yield return StartCoroutine(MoveAfter(0, -1));
        }
    }

    bool IsHittingBoarder(Vector2Int pos)
    {
        return !(pos.x >= 0 && pos.x < Columns && pos.y >= 0 && pos.y < Rows);
    }
    bool CanMoveTo<T>(Dictionary<Vector2Int, T> dict, Vector2Int pos) where T : IGridItem
    {
        return !HasItem(dict, pos) && pos.x >= 0 && pos.x < Columns && pos.y >= 0 && pos.y < Rows;
    }

    public bool HasItem(Vector2Int pos)
    {
        return GridItemDict.ContainsKey(pos)/*&& GridItemDict[pos] !=null &&!GridItemDict[pos].GetComponent<GridItem>().isDestroyed*/;
    }
    public bool HasItem<T>(Dictionary<Vector2Int, T> gridItemDict, Vector2Int pos) where T : IGridItem
    {
        return gridItemDict.ContainsKey(pos)/*&& GridItemDict[pos] !=null &&!GridItemDict[pos].GetComponent<GridItem>().isDestroyed*/;
    }

    public T GetItem<T>(Dictionary<Vector2Int, T> gridItemDict, Vector2Int pos) where T : IGridItem
    {
        if (!HasItem(gridItemDict, pos))
        {
            return default(T);
        }
        return gridItemDict[pos];
    }

    public Vector3 IndexToPosition(Vector2Int ind)
    {
        return IndexToPosition(ind.x, ind.y);
    }

    Vector3 IndexToPosition(int i, int j)
    {
        return new Vector3(tileSizex * i, tileSizey * j);
    }
    public void updatePos(GridItem item)
    {
        item.StartCoroutine(item.move(IndexToPosition(item.core.index), animTime));
    }

    public void UpdateItemPositionToIndexEnumerator(GridItem item)
    {
        item.StartCoroutine(item.move(IndexToPosition(item.core.index), animTime));
    }
    public void MoveItemToPos(Vector2Int start, Vector2Int end, GameObject obj, float animTime)
    {

        obj.GetComponent<MonoBehaviour>().StartCoroutine(obj.GetComponent<GridItem>().move(IndexToPosition(end), animTime));

        // obj.transform.DOLocalMove(IndexToPosition(end), 0.3f);

        //obj.transform.localPosition = IndexToPosition(end);
    }

    public void MoveItemToIndex(GridItem item1, Vector2Int targetIndex)
    {
        var originIndex = item1.core.index;
        if (GridManager.Instance.HasItem(targetIndex))
        {
            var swapOb = GridManager.Instance.GetItem(GridItemDict, targetIndex).GetComponent<GridItem>();
            swapOb.core.index = originIndex;

            GridItemDict[originIndex] = swapOb;
        }
        else
        {

            GridItemDict.Remove(originIndex);
        }
        item1.core.index = targetIndex;
        GridItemDict[targetIndex] = item1;
        Debug.Log($"{targetIndex} {originIndex}");

    }
    //public void swapItem(GridItem item1, GridItem item2, float animTime)
    //{
    //    var ind1 = item1.index;
    //    var ind2 = item2.index;
    //    GridItemDict[ind1] = item2.gameObject;
    //    GridItemDict[ind2] = item1.gameObject;



    //    item1.index = ind2;

    //    item2.index = ind1;

    //    item1.GetComponent<MonoBehaviour>().StartCoroutine(item1.GetComponent<GridItem>().move(IndexToPosition(item1.index), animTime));
    //    item2.GetComponent<MonoBehaviour>().StartCoroutine(item2.GetComponent<GridItem>().move(IndexToPosition(item2.index), animTime));

    //}

    public void FinishItemMoving(GameObject obj)
    {

    }


    public void GenerateGrid()
    {

        // build our grid map for our level based on the map object values
        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                GameObject obj = Instantiate(EmptyGridPrefab);
                obj.name = $"grid-x{i}-y{j}";
                obj.transform.SetParent(bk);
                obj.transform.localPosition = IndexToPosition(i, j);
                obj.GetComponent<GridEmptyCell>().index = new Vector2Int(i, j);
                emptyGridList.Add(obj.GetComponent<GridEmptyCell>());
            }
        }
    }


    public GameObject AddGrid(int i, int j, ItemType type)
    {
        var obj = ItemManager.Instance.createItem(type, items, Vector3.zero, i, j);
        //GameObject obj = Instantiate(Resources.Load<GameObject>("items/"+type.ToString()));
        //obj.GetComponent<GridItem>().init(new Vector2Int(i, j), type);
        obj.name = $"grid-x{i}-y{j}";
        obj.transform.SetParent(items);

        obj.transform.localPosition = IndexToPosition(i, j);
        var render = obj.GetComponentInChildren<SpriteRenderer>();
        render.color = new Color(1, 1, 1, 0);
        DOTween.To(() => render.color, x => render.color = x, Color.white, animTime * 2);

        //obj.transform.position += new Vector3(0, 1, 0);
        // add to grid once instantiated
        GridItemDict[new Vector2Int(i, j)] = obj.GetComponent<GridItem>();
        return obj;
    }
    public void RemoveGrid(int i, int j, ItemType type)
    {
        GridItemDict.Remove(new Vector2Int(i, j));
        deckPool.Add(type);
    }

    public void RemoveGrid(Vector2Int ind, ItemType type)
    {
        if (!GridItemDict.ContainsKey(ind))
        {
            Debug.Log("no remove for index " + ind);
        }
        GridItemDict.Remove(ind);

        if(type is ItemType.Mud || type is ItemType.LiquidBomb)
        {
            return;
        }
        deckPool.Add(type);
    }
    public void RemoveDeck(GridItem item)
    {
        RemoveGrid(item.index, item.type);
        deckPool.Remove(item.type);
        item.destory();
    }
    public void RemoveGrid(GridItem item)
    {
        RemoveGrid(item.index, item.type);
    }
}
