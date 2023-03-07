using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public enum ItemType { ore,herb, arrow, poison}
public class GridManager : Singleton<GridManager>
{

    static public float animTime = 0.4f;
    public float tileSize = 2f;
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
    List<ItemType> deckPool = new List<ItemType>() { ItemType.ore, ItemType.ore, ItemType.herb, ItemType.herb, ItemType.arrow, ItemType.poison, ItemType.poison };
    //{ ItemType.ore, ItemType.ore, ItemType.herb, ItemType.herb, ItemType.arrow, ItemType.poison, ItemType.poison };
    //{ ItemType.ore, ItemType.ore, ItemType.ore, ItemType.herb, ItemType.herb, ItemType.herb, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.poison, ItemType.poison, ItemType.poison };
    //{ ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.arrow, };
    //{ ItemType.ore, ItemType.ore, ItemType.ore, ItemType.herb, ItemType.herb, ItemType.herb, ItemType.arrow, ItemType.arrow, ItemType.arrow, ItemType.poison, ItemType.poison, ItemType.poison };
    public IEnumerator DrawItem(int drawCount)
    {
        List<Vector2Int> availableEmpty = new List<Vector2Int>();
        foreach(var key in emptyGridList)
        {
            if (!GridItemDict.ContainsKey(key.index))
            {
                availableEmpty.Add(key.index);
            }
        }
        for(int i = 0;i< drawCount; i++)
        {
            var picked = availableEmpty[Random.Range(0, availableEmpty.Count)];
            var pickedType = deckPool[Random.Range(0, deckPool.Count)];
            deckPool.Remove(pickedType);
            AddGrid(picked.x, picked.y, pickedType);
            availableEmpty.Remove(picked);
        }

        yield return StartCoroutine(MoveAfter(0, -1));
    }

    public IEnumerator DrawAllItemsFromPool()
    {
        List<Vector2Int> availableEmpty = new List<Vector2Int>();
        foreach (var key in emptyGridList)
        {
            if (!GridItemDict.ContainsKey(key.index))
            {
                availableEmpty.Add(key.index);
            }
        }
        foreach(var item in deckPool)
        {
            var picked = availableEmpty[Random.Range(0, availableEmpty.Count)];
            AddGrid(picked.x, picked.y, item);
            availableEmpty.Remove(picked);
        }

        deckPool.Clear();

        yield return StartCoroutine(MoveAfter(0, -1));
    }
    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();

        //AddGrid(1, 1, ItemType.herb);
        //AddGrid(1, 2, ItemType.ore);
        //AddGrid(2, 0, ItemType.herb);
        //AddGrid(2, 2, ItemType.herb);


        StartCoroutine(MoveAfter(0, -1));
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
    
    public IEnumerator MoveAfter(int x,int y)
    {
        //Debug.Log("move after");
        //yield return new WaitForSeconds(animTime);
        yield return MoveEnumerator(x,y,false);
        BattleManager.Instance.PredictNextAttack();
    }
    List<Transform> sortEmptyCells()
    {
        var myList = new List<Transform>();
        foreach (var key in emptyGridList)
        {
            myList.Add(key.transform);
        }
        myList.Sort(delegate (Transform a, Transform b) {
            if (Mathf.Approximately(a.position.x , b.position.x))
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
        for(int i= 0; i<10; i++){
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
            if (Mathf.Approximately( myList[i].position.x, x))
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
        clearAttackPreview();
        var cells = getFrontCellsFromBottomToTop();
        if (enemy.attackFromBottom)
        {
            var cell = cells[enemy.attackInd];
            var go= Instantiate(gridPreviewCell,cell.position,cell.rotation);
            previewCells.Add(go);
        }
    }

    public GridItem itemEnemyAttack(Enemy enemy)
    {

        var cells = getFrontCellsIndexFromBottomToTop();
        if (enemy.attackFromBottom)
        {
            var cell = cells[enemy.attackInd];
            var item = GridItemDict.ContainsKey(cell) ? GridItemDict[cell].GetComponent<GridItem>() : null;
            return item;
        }
        return null;
    }

    public void clearAttackPreview()
    {
        foreach(var cell in previewCells)
        {
            Destroy(cell);
        }
        previewCells.Clear();
    }

    List<BattleMessage> messages;

    Vector2Int rotateMoveVectorBasedOnRotateTime(int x,int y)
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

    static List<Vector2Int> sortItemIndex<T>(Vector2Int moveVector, Dictionary<Vector2Int, T> gridItemDict)where T : IGridItem
    {

        var x = moveVector.x;
        var y = moveVector.y;

        var allItemIndex = new List<Vector2Int>();
        foreach (var key in gridItemDict.Keys)
        {
            allItemIndex.Add(key);
        }
        allItemIndex.Sort(delegate (Vector2Int a, Vector2Int b) {
            if (x == 1)
            {
                return b.x.CompareTo(a.x);
            }
            else if (x == -1)
            {
                return a.x.CompareTo(b.x);
            }
            else if (y == 1)
            {

                return b.y.CompareTo(a.y);
            }
            else
            {

                return a.y.CompareTo(b.y);
            }
        });
        return allItemIndex;
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
                if (test <0)
                {
                    Debug.LogError("wrong");
                    break;
                }
                newKey += moveVector;
                if (!CanMoveTo(gridItemDict,newKey))
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
                    if (HasItem(gridItemDict, newKey))
                    {

                        if (isAttacking)
                        {
                            GetItem(gridItemDict,newKey).beCrushed(gridItem, messages);
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
    }
    public void ParsePredictMessage(Dictionary<GridItemCore, GridItem> predictToOrigin) {
        for (int i = 0; i < messages.Count; i++)
        {
            var message = messages[i];
            if (message is MessageMove move)
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
            else if (message is MessageItemHeal heal)
            {
                Debug.Log($"{heal.item.Name} heal {heal.amount}");
                predictToOrigin[heal.item].baseItem.WillHeal(heal.amount);
                //heal.target.Heal(heal.amount);
                ////FloatingTextManager.Instance.addText($"Heal {heal.amount}", heal.target.transform.position,Color.green);
                //FloatingTextManager.Instance.addText($"Heal {heal.amount}", heal.item.transform.position, Color.green);

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
        for(int i = 0;i<messages.Count;i++)
        {
            var message = messages[i];
            if (message is MessageMove move)
            {
                if(move.itemTargetIndex.Count != 0)
                {
                    foreach (var pair in move.itemTargetIndex.Keys)
                    {
                        UpdateItemPositionToIndexEnumerator(GridItemDict[pair.index]);
                    }

                    yield return new WaitForSeconds(animTime);
                }
            }
            else if(message is MessageItemAttack attack)
            {
                Luggage.Instance.DoDamage(attack.damage);
                if (!GridItemDict.ContainsKey(attack.index))
                {
                    Debug.Log("?");
                }
               // Debug.Log($"{attack.item.Name} Attack {attack.damage} {GridItemDict[attack.index].transform.position}");

                FloatingTextManager.Instance.addText($"Attack {attack.damage}", GridItemDict[attack.index].transform.position, Color.red);
                //FloatingTextManager.Instance.addText($"{attack.damage}", heal.item.transform.position);
                if (!attack.skipAnim)
                {
                    yield return new WaitForSeconds(animTime);
                }

            }
            else if (message is MessageItemHeal heal)
            {
                heal.target.Heal(heal.amount);
                if (!GridItemDict.ContainsKey(heal.index))
                {
                    Debug.Log("?");
                }
                //FloatingTextManager.Instance.addText($"Heal {heal.amount}", heal.target.transform.position,Color.green);
                FloatingTextManager.Instance.addText($"Heal {heal.amount}", GridItemDict[heal.index].transform.position, Color.green);
                yield return new WaitForSeconds(animTime);
            }
            else if (message is MessageDestroy destr)
            {
                if (!GridItemDict.ContainsKey(destr.index))
                {
                    Debug.Log("?");
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
                UpdateItemPositionToIndexEnumerator(GridItemDict[itemMove.index]);
                yield return new WaitForSeconds(animTime);
            }
        }
    }

    public IEnumerator EnemyAttackEnumerator(Enemy enemy)
    {
        messages.Clear();
        var damage = enemy. attack;
        var item = GridManager.Instance.itemEnemyAttack(enemy);
        if (item != null)
        {
            damage -= item.core.defense;
            item.addDestroyMessage(messages);

            yield return StartCoroutine(ParseMessages());

            yield return StartCoroutine(MoveAfter(0, -1));


        }

        yield return StartCoroutine( BattleManager.Instance.player.ApplyDamage(damage));
    }
    public List<GameObject> attackingEdges;
    public void updateAttackEdge()
    {
        foreach(var edge in attackingEdges)
        {
            edge.SetActive(false);
        }
        var nextAttackEdge = (BattleManager.Instance.getCurrentAttackRotationId() - rotatedTime + 4) % 4;
        attackingEdges[nextAttackEdge].SetActive(true);
    }
    public IEnumerator MoveAndAttack(int x,int y)
    {
        foreach(var item in GridItemDict.Values)
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

    }

    public void predict(int x,int y)
    {
        // we need to copy a GridItemDict, create a map between original grid and new ones
        // move and attack using it
        var predictDict = new Dictionary<Vector2Int, GridItemCore>();
        var originalItemToPredictItem = new Dictionary<GridItemCore, GridItem>();
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
    }
    public IEnumerator MoveEnumerator(int x, int y, bool isAttacking)
    {
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
    bool CanMoveTo<T>(Dictionary<Vector2Int, T> dict,Vector2Int pos) where T : IGridItem
    {
        return !HasItem(dict,pos) && pos.x >= 0 && pos.x < Columns && pos.y >= 0 && pos.y < Rows;
    }

    public bool HasItem(Vector2Int pos)
    {
        return GridItemDict.ContainsKey(pos)/*&& GridItemDict[pos] !=null &&!GridItemDict[pos].GetComponent<GridItem>().isDestroyed*/;
    }
    public bool HasItem<T>(Dictionary<Vector2Int, T> gridItemDict, Vector2Int pos) where T : IGridItem
    {
        return gridItemDict.ContainsKey(pos)/*&& GridItemDict[pos] !=null &&!GridItemDict[pos].GetComponent<GridItem>().isDestroyed*/;
    }

    public T GetItem<T>(Dictionary<Vector2Int, T> gridItemDict,Vector2Int pos) where T : IGridItem
    {
        if (!HasItem(gridItemDict,pos))
        {
            return default(T);
        }
        return gridItemDict[pos];
    }

    public Vector3 IndexToPosition(Vector2Int ind)
    {
        return IndexToPosition(ind.x, ind.y);
    }

    Vector3 IndexToPosition(int i,int j)
    {
        return new Vector3(tileSize * i,  tileSize * j);
    }
    public void updatePos(GridItem item)
    {
        item.StartCoroutine(item.move(IndexToPosition(item.core.index), animTime));
    }

    public void UpdateItemPositionToIndexEnumerator(GridItem item)
    {
        item.StartCoroutine(item.move(IndexToPosition(item.core.index),animTime));
    }
    public void MoveItemToPos(Vector2Int start, Vector2Int end, GameObject obj, float animTime)
    {

        obj.GetComponent<MonoBehaviour>().StartCoroutine(obj.GetComponent<GridItem>().move(IndexToPosition(end), animTime));

        // obj.transform.DOLocalMove(IndexToPosition(end), 0.3f);

        //obj.transform.localPosition = IndexToPosition(end);
    }

    public void MoveItemToIndex(GridItem item1,Vector2Int targetIndex)
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


    public void AddGrid(int i, int j, ItemType type)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("items/"+type.ToString()));
        obj.GetComponent<GridItem>().init(new Vector2Int(i, j), type);
        obj.name = $"grid-x{i}-y{j}";
        obj.transform.SetParent(items);

        obj.transform.localPosition = IndexToPosition(i, j);
        var render = obj.GetComponentInChildren<SpriteRenderer>();
        render.color = new Color(1, 1, 1, 0);
        DOTween.To(() => render.color, x => render.color = x, Color.white, animTime*2);

        //obj.transform.position += new Vector3(0, 1, 0);
        // add to grid once instantiated
        GridItemDict[new Vector2Int(i,j)] = obj.GetComponent< GridItem>();
    }
    public void RemoveGrid(int i, int j,ItemType type)
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
        deckPool.Add(type);
    }
}
