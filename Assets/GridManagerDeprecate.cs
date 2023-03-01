using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    right,
    up,
    left,
    down
}
public class GridManagerDeprecate : Singleton<GridManagerDeprecate>
{
    float tileSize = 0.32f;
    public enum GridType
    {
        passable,
        nonPassable
    }

    public int Rows = 0;
    public int Columns = 0;
    public float Scale = 0.32f;
    public GameObject GridPrefab;
    public Dictionary<(int, int), GameObject> GridArray = new Dictionary<(int, int), GameObject>();

    //void SetDistance(int fromX, int fromY)
    //{
    //    // reset the current grid to make them all 'unvisited'
    //    foreach (GameObject obj in GridArray.Values)
    //    {
    //        if (obj)
    //        {
    //            obj.GetComponent<GridStats>().Visited = -1;
    //        }
    //    }

    //    // set the start position as steps to get there == 0
    //    GridArray[(fromX, fromY)].GetComponent<GridStats>().Visited = 0;

    //    // work out the number of steps to get to each grid position object
    //    for (int step = 1; step < Rows * Columns; step++)
    //    {
    //        foreach (GameObject obj in GridArray.Values)
    //        {
    //            if (obj && obj.GetComponent<GridStats>().Visited == step - 1)
    //            {
    //                TestFourDirections(obj.GetComponent<GridStats>().X, obj.GetComponent<GridStats>().Y, step);
    //            }
    //        }
    //    }
    //}

    //void SetVisited(int x, int y, int step)
    //{
    //    if (GridArray[(x, y)])
    //    {
    //        GridArray[(x, y)].GetComponent<GridStats>().Visited = step;
    //    }
    //}

    //void TestFourDirections(int x, int y, int step)
    //{
    //    if (TestDirection(x, y, -1, Direction.up))
    //    {
    //        SetVisited(x, y + 1, step);
    //    }

    //    if (TestDirection(x, y, -1, Direction.right))
    //    {
    //        SetVisited(x + 1, y, step);
    //    }

    //    if (TestDirection(x, y, -1, Direction.down))
    //    {
    //        SetVisited(x, y - 1, step);
    //    }

    //    if (TestDirection(x, y, -1, Direction.left))
    //    {
    //        SetVisited(x - 1, y, step);
    //    }
    //}
    public static Vector3 indexToPosition((int, int) ind)
    {
        return new Vector2(ind.Item1 + 0.5f, ind.Item2 + 0.5f) * 0.32f;
    }


    public static Vector3 indexToPosition(Vector3 ind)
    {
        return new Vector2(ind.x + 0.5f, ind.y + 0.5f) * 0.32f;
    }

    public static Vector3 PositionToIndex((float, float) pos)
    {
        return new Vector2((int)(pos.Item1 / 0.32f - 0.5f), (int)(pos.Item2 / 0.32f - 0.5f));

    }
    public static Vector3 PositionToIndex(Vector3 pos)
    {
        return new Vector2((int)(pos.x / 0.32f - 0.5f), (int)(pos.y / 0.32f - 0.5f));

    }
    public static (int, int) PositionToIndexPair(Vector3 pos)
    {
        return (Mathf.RoundToInt(pos.x / 0.32f - 0.5f), Mathf.RoundToInt(pos.y / 0.32f - 0.5f));

    }

    //public bool hasAffectedOnDir(int x, int y, Direction direction)
    //{
    //    var i = 0;
    //    AffectableItem hitItem;
    //    while (i < 100)
    //    {
    //        i++;
    //        var res = ShootDirection(x, y, direction, out hitItem, false, false, true);
    //        if (hitItem)
    //        {
    //            return true;
    //        }
    //        if (res == null)
    //        {
    //            return false;
    //        }
    //        x = res.X;
    //        y = res.Y;
    //    }
    //    return false;
    //}

    //public GridStats ShootDirection(int x, int y, Direction direction, out AffectableItem hitItem, bool throughCorpse, bool hitHuman, bool throughWindow)
    //{
    //    hitItem = null;

    //    var target = (x, y);
    //    switch (direction)
    //    {
    //        case Direction.up:
    //            target = (x, y + 1);
    //            break;
    //        case Direction.right:
    //            target = (x + 1, y);
    //            break;
    //        case Direction.down:
    //            target = (x, y - 1);
    //            break;
    //        case Direction.left:
    //            target = (x - 1, y);
    //            break;
    //    }
    //    if (!GridArray.ContainsKey(target))
    //    {
    //        return null;
    //    }
    //    foreach (var item in affectableItems)
    //    {
    //        if (item.pos == target)
    //        {
    //            if (item is Dog dog || !(item is Human))
    //            {

    //            }
    //            else if (item.isDead)
    //            {
    //                if (throughCorpse)
    //                {

    //                }
    //                else
    //                {
    //                    return null;
    //                }
    //            }
    //            else if (!hitHuman && !item.isAffected)
    //            {
    //                return null;
    //            }
    //            else
    //            {

    //                hitItem = item;
    //                return null;
    //            }
    //        }
    //    }
    //    var gridStatus = GridArray[target];
    //    if (gridStatus)
    //    {
    //        if (!throughWindow && gridStatus.GetComponent<GridStats>().type == "Window")
    //        {
    //            return null;
    //        }

    //        return gridStatus.GetComponent<GridStats>();
    //    }
    //    return null;
    //}
    static public (int, int) directionToPair(Direction direction)
    {
        var x = 0; var y = 0;

        var target = (x, y);
        switch (direction)
        {
            case Direction.up:
                target = (x, y + 1);
                break;
            case Direction.right:
                target = (x + 1, y);
                break;
            case Direction.down:
                target = (x, y - 1);
                break;
            case Direction.left:
                target = (x - 1, y);
                break;

        }
        return target;
    }
    static public Direction pairToDirection((int, int) pair)
    {
        var x = 0; var y = 0;

        switch (pair)
        {
            case (0, 1):
                return Direction.up;
            case (1, 0):
                return Direction.right;
            case (0, -1):
                return Direction.down;
            case (-1, 0):
                return Direction.left;



        }
        Debug.LogError("wrong direction " + pair);
        return Direction.right;
    }
    //public GridStats MoveDirection(int x, int y, Direction direction)
    //{
    //    var target = (x, y);
    //    switch (direction)
    //    {
    //        case Direction.up:
    //            target = (x, y + 1);
    //            break;
    //        case Direction.right:
    //            target = (x + 1, y);
    //            break;
    //        case Direction.down:
    //            target = (x, y - 1);
    //            break;
    //        case Direction.left:
    //            target = (x - 1, y);
    //            break;
    //    }
    //    if (!GridArray.ContainsKey(target))
    //    {
    //        return null;
    //    }
    //    foreach (var item in affectableItems)
    //    {
    //        if (item.pos == target)
    //        {
    //            return null;
    //        }
    //    }
    //    var gridStatus = GridArray[target];
    //    if (gridStatus)
    //    {
    //        return gridStatus.GetComponent<GridStats>();
    //    }
    //    return null;
    //}
    public bool in3x3Area((int, int) pos1, (int, int) pos2)
    {
        return Mathf.Abs(pos1.Item1 - pos2.Item1) <= 1 && Mathf.Abs(pos1.Item2 - pos2.Item2) <= 1;
    }
    public bool inCrossArea((int, int) pos1, (int, int) pos2)
    {
        return Mathf.Abs(pos1.Item1 - pos2.Item1) + Mathf.Abs(pos1.Item2 - pos2.Item2) <= 1;
    }
    //public List<AffectableItem> affectableItems;
    //public void AddAffectable(AffectableItem item)
    //{
    //    affectableItems.Add(item);
    //}

    //bool TestDirection(int x, int y, int step, Direction direction)
    //{
    //    switch (direction)
    //    {
    //        case Direction.up:
    //            return (y + 1 < Rows && GridArray[(x, y + 1)] && GridArray[(x, y + 1)].GetComponent<GridStats>().Visited == step);
    //        case Direction.right:
    //            return (x + 1 < Columns && GridArray[(x + 1, y)] && GridArray[(x + 1, y)].GetComponent<GridStats>().Visited == step);
    //        case Direction.down:
    //            return (y - 1 > -1 && GridArray[(x, y - 1)] && GridArray[(x, y - 1)].GetComponent<GridStats>().Visited == step);
    //        case Direction.left:
    //            return (x - 1 > -1 && GridArray[(x - 1, y)] && GridArray[(x - 1, y)].GetComponent<GridStats>().Visited == step);
    //        default:
    //            return false;
    //    }
    //}

    GameObject FindClosest(Transform targetLocation, List<GameObject> list)
    {
        float currentDistance = Scale * Rows * Columns;
        int indexNumber = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                indexNumber = i;
            }
        }

        return list[indexNumber];
    }

    //List<GameObject> GetPath(int toX, int toY)
    //{
    //    int step;
    //    int x = toX;
    //    int y = toY;

    //    var path = new List<GameObject>();
    //    var tempList = new List<GameObject>();

    //    if (GridArray[(toX, toY)] && GridArray[(toX, toY)].GetComponent<GridStats>().Visited > 0)
    //    {
    //        path.Add(GridArray[(x, y)]);
    //        step = GridArray[(x, y)].GetComponent<GridStats>().Visited - 1;
    //    }
    //    else
    //    {
    //        print("Can't reach the desired location.");
    //        return null;
    //    }

    //    for (int i = step; step > -1; step--)
    //    {
    //        if (TestDirection(x, y, step, Direction.up))
    //        {
    //            tempList.Add(GridArray[(x, y + 1)]);
    //        }
    //        if (TestDirection(x, y, step, Direction.right))
    //        {
    //            tempList.Add(GridArray[(x + 1, y)]);
    //        }
    //        if (TestDirection(x, y, step, Direction.down))
    //        {
    //            tempList.Add(GridArray[(x, y - 1)]);
    //        }
    //        if (TestDirection(x, y, step, Direction.left))
    //        {
    //            tempList.Add(GridArray[(x - 1, y)]);
    //        }

    //        GameObject tempObj = FindClosest(GridArray[(toX, toY)].transform, tempList);
    //        path.Add(tempObj);

    //        x = tempObj.GetComponent<GridStats>().X;
    //        y = tempObj.GetComponent<GridStats>().Y;

    //        tempList.Clear();
    //    }

    //    return path;
    //}

    //List<GameObject> GetPath(int fromX, int fromY, int toX, int toY)
    //{
    //    SetDistance(fromX, fromY);
    //    return GetPath(toX, toY);
    //}

    private void Start()
    {
        //Wall.Instance.init();
        //Trap.Instance.init();
        //Target.Instance.init();
        //Window.Instance.init();
    }
    Vector3 startPosition = new Vector3(0.5f, 0.5f, 0);
    public void AddGrid(int i, int j, string type)
    {
        //if (GridArray.ContainsKey((i, j)))
        //{
        //    GridArray[(i, j)].GetComponent<GridStats>().type = type;
        //    return;
        //}
        GameObject obj = Instantiate(GridPrefab, new Vector3(startPosition.x + Scale * i, startPosition.y + Scale * j, startPosition.z), Quaternion.identity);
        obj.name = $"grid-x{i}-y{j}";
        obj.transform.SetParent(gameObject.transform);
        //obj.GetComponent<GridStats>().X = i;
        //obj.GetComponent<GridStats>().Y = j;

        // add to grid once instantiated
        GridArray[(i, j)] = obj;
    }
    public void GenerateGrid(int[,] map, Vector3 startPosition)
    {
        // check that we can instantiate a grid object
        if (!GridPrefab)
        {
            print("Missing GridPrefab, please assign.");
            return;
        }

        // assign the rows & columns properties based on our grid map size
        Rows = map.GetLength(1);
        Columns = map.GetLength(0);


        // build our grid map for our level based on the map object values
        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                // find out what should be at this position
                int mapObj = map[i, j];
                switch (mapObj)
                {
                    case (int)GridType.passable:
                        // instantiate our grid object & assign position
                        GameObject obj = Instantiate(GridPrefab, new Vector3(startPosition.x + Scale * i, startPosition.y, startPosition.z + Scale * j), Quaternion.identity);
                        obj.name = $"grid-x{i}-y{j}";
                        obj.transform.SetParent(gameObject.transform);
                        //obj.GetComponent<GridStats>().X = i;
                        //obj.GetComponent<GridStats>().Y = j;

                        // add to grid once instantiated
                        GridArray[(i, j)] = obj;
                        break;
                    case (int)GridType.nonPassable:
                    default:
                        break;
                }
            }
        }
    }

    //public List<GameObject> GetPathToPosition(Transform from, Transform to, int maximumSteps)
    //{
    //    var startX = (int)from.position.x;
    //    var startY = (int)from.position.z;
    //    var endX = (int)to.position.x;
    //    var endY = (int)to.position.y;

    //    return GetPath(startX, startY, endX, endY);
    //}

    //public List<GameObject> GetPathToPosition(Transform from, int toX, int toY, int maximumSteps)
    //{
    //    var startX = (int)from.position.x;
    //    var startY = (int)from.position.z;

    //    return GetPath(startX, startY, toX, toY);
    //}

    //public List<GameObject> GetAvailablePositions(Transform currentPosition, int maximumSteps)
    //{
    //    var startX = (int)currentPosition.position.x;
    //    var startY = (int)currentPosition.position.z;

    //    SetDistance(startX, startY);

    //    return null; // todo;
    //}
}
