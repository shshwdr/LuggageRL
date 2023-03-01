using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType { ore,herb}
public class GridManager : Singleton<GridManager>
{
    float tileSize = 2f;
    public int Rows = 2;
    public int Columns = 3;
    public GameObject EmptyGridPrefab;
    public GameObject itemPrefab;
    public Vector3 startPosition;
    public Transform bk;
    public Transform items;
    public Dictionary<Vector2Int, GameObject> GridArray = new Dictionary<Vector2Int, GameObject>();
    public List<GridEmptyCell> emptyGridList = new List<GridEmptyCell>();
    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();

        AddGrid(0, 0, ItemType.ore);
        AddGrid(1, 0, ItemType.herb);
        AddGrid(1, 1, ItemType.ore);


        StartCoroutine(MoveAfter(0, -1));
    }
    float animTime = 0.3f;
    int rotatedTime = 0;
    public void Rotate(int time)
    {
        rotatedTime += time;
        rotatedTime %= 4;
        //transform.eulerAngles = new Vector3(0, 0, 90 * rotatedTime);
        transform.DORotate(new Vector3(0, 0, 90 * rotatedTime), animTime);
        StartCoroutine(MoveAfter(0, -1));
    }

    public IEnumerator MoveAfter(int x,int y)
    {
        yield return new WaitForSeconds(animTime);
        MoveInternal(x,y,false);
    }

    public void MoveInternal(int x, int y, bool isAttacking )
    {
        var moveVector = new Vector2Int(x, y);
        var tempVector = (transform.rotation * (Vector2)moveVector);
        if (rotatedTime == 1 || rotatedTime == 3)
        {
            tempVector = -tempVector;
        }
        moveVector = new Vector2Int((int)tempVector.x, (int)tempVector.y);
        x = moveVector.x;
        y = moveVector.y;
        Debug.Log("move " + moveVector);

        var myList = new List<Vector2Int>();
        foreach (var key in GridArray.Keys)
        {
            myList.Add(key);
        }
        myList.Sort(delegate (Vector2Int a, Vector2Int b) {
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

        foreach (var key in myList)
        {
            var newKey = key;
            int test = 0;
            var obj = GetItem(key);
            var gridItem = obj.GetComponent<GridItem>();
            while (true)
            {
                test++;
                if (test > 10)
                {
                    break;
                }
                newKey += moveVector;
                if (!CanMoveTo(newKey))
                {
                    if (IsHittingBoarder(newKey))
                    {
                        if (isAttacking)
                        {
                            gridItem.hitBorder(true, newKey - moveVector != key, items.transform.TransformPoint(IndexToPosition(newKey)) );
                        }
                    }
                    if (HasItem(newKey))
                    {

                        if (isAttacking)
                        {
                            GetItem(newKey).GetComponent<GridItem>().BeHit(gridItem);
                        }
                    }

                    newKey -= moveVector;
                    break;
                }
            }
            if (newKey != key)
            {
                MoveItemToPos(key, newKey, obj,animTime);

                obj.GetComponent<GridItem>().calculateHit();
            }
            else
            {
                obj.GetComponent<GridItem>().calculateHit();
            }
        }
    }

    public void Move(int x, int y)
    {
        MoveInternal(x, y,true);

        StartCoroutine(MoveAfter(0, -1));
    }

    bool IsHittingBoarder(Vector2Int pos)
    {
        return !(pos.x >= 0 && pos.x < Columns && pos.y >= 0 && pos.y < Rows);
    }
    bool CanMoveTo(Vector2Int pos)
    {
        return !HasItem(pos) && pos.x >= 0 && pos.x < Columns && pos.y >= 0 && pos.y < Rows;
    }

    public bool HasItem(Vector2Int pos)
    {
        return GridArray.ContainsKey(pos);
    }

    public GameObject GetItem(Vector2Int pos)
    {
        return GridArray[pos];
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
        item.StartCoroutine(item.move(IndexToPosition(item.index), animTime));
    }
    public void MoveItemToPos(Vector2Int start, Vector2Int end, GameObject obj, float animTime)
    {

        GridArray[end] = obj;

        GridArray.Remove(start);
        obj.GetComponent<GridItem>().index = end;
        obj.GetComponent<MonoBehaviour>().StartCoroutine(obj.GetComponent<GridItem>().move(IndexToPosition(end), animTime));

        // obj.transform.DOLocalMove(IndexToPosition(end), 0.3f);

        //obj.transform.localPosition = IndexToPosition(end);
    }

    public void swapItem(GridItem item1, GridItem item2, float animTime)
    {
        var ind1 = item1.index;
        var ind2 = item2.index;
        GridArray[ind1] = item2.gameObject;
        GridArray[ind2] = item1.gameObject;



        item1.index = ind2;

        item2.index = ind1;

        item1.GetComponent<MonoBehaviour>().StartCoroutine(item1.GetComponent<GridItem>().move(IndexToPosition(item1.index), animTime));
        item2.GetComponent<MonoBehaviour>().StartCoroutine(item2.GetComponent<GridItem>().move(IndexToPosition(item2.index), animTime));

    }

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
        obj.GetComponent<GridItem>().index = new Vector2Int(i, j);
        obj.name = $"grid-x{i}-y{j}";
        obj.transform.SetParent(items);

        obj.transform.localPosition = IndexToPosition(i, j);
        // add to grid once instantiated
        GridArray[new Vector2Int(i,j)] = obj;
    }
}
