using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerActionType
{
    Attack,
    Move,
    EndTurn
};

public class BabySittingTutorial : Singleton<BabySittingTutorial>
{

    public bool isOn = true;

    public void finishTutorial()
    {
        isOn = false;
    }
    void Start()
    {
        overlayButton.GetComponent<Button>().onClick.AddListener(hideOverlay);
        DontDestroyOnLoad(this.gameObject);
    }
    public GameObject attackOverlay;
    public GameObject attackOverlay2;
    public GameObject endTurnOverlay;
    public GameObject moveOverlay;
    public GameObject overlayButton;
    public GameObject overlayParent;
    private GameObject currentOverlay;
    private HashSet<GameObject> visitedOverlay = new HashSet<GameObject>();
    public void showOverlay(GameObject overlay)
    {
        if (!isOn)
        {
            return;
        }

        if (visitedOverlay.Contains(overlay))
        {
            return;
        }
        visitedOverlay.Add(overlay);
        overlayParent.SetActive(true);
        overlay.SetActive(true);
        overlayButton.SetActive(true);
        currentOverlay = overlay;
    }

    public void hideOverlay()
    {
        currentOverlay.SetActive(false);
        overlayParent.SetActive(false);
        overlayButton.SetActive(false);

        if (currentOverlay == attackOverlay)
        {
            showOverlay(attackOverlay2);
        }
    }
    
    public GameObject battle1Turn0LineHint;
    public GameObject battle1Turn1LineHint;
    public GameObject battle2Turn0LineHint;

    public bool hijackDrawItem()
    {
        if (!isOn)
        {
            return false;
        }
        var battleId = BattleManager.Instance.battleCount;
        var turnId = BattleManager.Instance.turnCount;
        if (battleId == 1 && turnId < 3)
        {
            return true;
        }

        if (battleId == 2 && turnId == 0)
        {
            return true;
        }

        return false;
    }

    public int selectAttack()
    {
        if (!isOn)
        {
            return -1;
        }
        var battleId = BattleManager.Instance.battleCount;
        var turnId = BattleManager.Instance.turnCount;
        if (battleId == 2 && turnId == 0)
        {
            return 1;
        }

        return -1;
    }

    public IEnumerator DrawItem(int drawCount, bool shouldDrop, bool shouldShow)
    {
        if (!isOn)
        {
            yield break;
        }
        var battleId = BattleManager.Instance.battleCount;
        var turnId = BattleManager.Instance.turnCount;

        if (battleId == 1)
        {
            drawCount = turnId+1;
        }
        else if (battleId == 2)
        {
            drawCount = 3;
        }


        List<Vector2Int> availableEmpty = new List<Vector2Int>();
        foreach (var key in GridManager.Instance.emptyGridList)
        {
            if (!GridManager.Instance.GridItemDict.ContainsKey(key.index))
            {
                availableEmpty.Add(key.index);
            }
        }

        for (int i = 0; i < drawCount; i++)
        {
            var picked = availableEmpty[Random.Range(0, availableEmpty.Count)];
            var pickedType = GridManager.Instance.DeckPool[Random.Range(0, GridManager.Instance.DeckPool.Count)];
            if (battleId == 1)
            {
                if (turnId == 0)
                {
                    pickedType = ItemType.Marble;
                    picked = new Vector2Int(0, 2);
                }
                else if (turnId == 1)
                {

                    if (i == 0)
                    {
                        pickedType = ItemType.Marble;
                        picked = new Vector2Int(1, 2);
                    }else if (i == 1)
                    {
                        
                        pickedType = ItemType.Marble;
                        picked = new Vector2Int(3, 2);
                    }
                }
                else if (turnId == 2)
                {
                    
                    if (i == 0)
                    {
                        pickedType = ItemType.Marble;
                        picked = new Vector2Int(0, 2);
                    }else if (i == 1)
                    {
                        
                        pickedType = ItemType.Marble;
                        picked = new Vector2Int(3, 2);
                    }
                    else
                    {
                        pickedType = ItemType.Arrow;
                        picked = new Vector2Int(2, 2);
                    }
                    
                }
            }
            else if (battleId == 2)
            {
                if (turnId == 0)
                {
                    if (i == 0)
                    {
                        pickedType = ItemType.Marble;
                        picked = new Vector2Int(2, 0);
                    }
                    else if (i == 1)
                    {
                        pickedType = ItemType.Marble;
                        picked = new Vector2Int(3, 1);
                    }
                    else if (i == 2)
                    {
                        pickedType = ItemType.Arrow;
                        picked = new Vector2Int(2, 2);
                    }
                }
            }


            Debug.Log("draw " + pickedType.ToString());
            GridManager.Instance.DeckPool.Remove(pickedType);
            GridManager.Instance.AddGrid(picked.x, picked.y, pickedType);
            availableEmpty.Remove(picked);
        }

        if (shouldDrop)
        {
            yield return StartCoroutine(GridManager.Instance.MoveAfter(0, -1));
            BattleManager.Instance.PredictNextAttack();
        }
    }

    public void hideLines()
    {
        if (!isOn)
        {
            return;
        }
        foreach (Transform tran in battle1Turn0LineHint.transform.parent)
        {
            tran.gameObject.SetActive(false);
        }
    }

    public void showLines()
    {
        if (!isOn)
        {
            return;
        }
        hideLines();

        var battleId = BattleManager.Instance.battleCount;
        var turnId = BattleManager.Instance.turnCount;
        if (battleId == 1 && turnId == 1)
        {
            battle1Turn0LineHint.SetActive(true);
        }
        else if (battleId == 1 && turnId == 2)
        {
            battle1Turn1LineHint.SetActive(true);
        }
        else if (battleId == 2 && turnId == 0)
        {
            battle2Turn0LineHint.SetActive(true);
        }
    }

    public bool shouldDisableAction(PlayerActionType act)
    {
        if (!isOn)
        {
            return false;
        }
        var battleId = BattleManager.Instance.battleCount;
        var turnId = BattleManager.Instance.turnCount;
        var hasAttacked = BattleManager.Instance.hasAttacked;
        if (act == PlayerActionType.EndTurn)
        {
            if (battleId <= 1 && turnId == 0 && !hasAttacked)
            {
                return true;
            }
            
            if (battleId == 2 && turnId == 0&& !hasAttacked)
            {
                FloatingTextManager.Instance.addText("Please Follow The Tutorial And Attack First!",Vector3.zero,Color.white);
                return true;
            }
        }
        else if (act == PlayerActionType.Move)
        {
            if (battleId == 1 && turnId == 0)
            {
                return true;
            }

            if (battleId == 2 && turnId == 0&& !hasAttacked)
            {
                FloatingTextManager.Instance.addText("Please Follow The Tutorial And Attack First!",Vector3.zero,Color.white);
                return true;
            }
        }

        return false;
    }
}