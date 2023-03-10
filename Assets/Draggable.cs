using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnMouseDown()
    {

        if (!BattleManager.Instance.isInControl || !BattleManager.Instance.CanPlayerControl)
        {
            return;
        }
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_drag_pickup, transform.position);
    }
    GridEmptyCell swapOb;
    private void OnMouseDrag()
    {

        if (!BattleManager.Instance.isInControl || !BattleManager.Instance.CanPlayerControl)
        {
            return;
        }

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); worldPosition.z = 0;
        transform.position = worldPosition;
        swapOb = null;
        float distance = 3;
        foreach(var item in GridManager.Instance.GridItemDict.Values)
        {
            item.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
        foreach (var item in GridManager.Instance.emptyGridList)
        {
            item.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            var magnitude = (item.transform.position - transform.position).magnitude;
            if (magnitude < distance)
            {
                swapOb = item;
                    distance = magnitude;
            }
        }

        if (swapOb)
        {

            if (swapOb.index == GetComponent<GridItem>().core.index)
            {
                swapOb = null;
                return;
            }

            if (!GridManager.Instance.HasItem(swapOb.index))
            {
                swapOb.GetComponentInChildren<SpriteRenderer>().color = Color.green;
            }
            else
            {
                GridManager.Instance.GetItem(GridManager.Instance.GridItemDict, swapOb.index).GetComponentInChildren<SpriteRenderer>().color = Color.green;
            }
        }
    }
    private void OnMouseUp()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_drag_putdown, transform.position);
        if (swapOb)
        {
            StartCoroutine(mouseUp());
        }
        else
        {
            GridManager.Instance.updatePos(GetComponent<GridItem>());

        }
    }

    IEnumerator mouseUp()
    {


        foreach (var item in GridManager.Instance.GridItemDict.Values)
        {
            item.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
        foreach (var item in GridManager.Instance.emptyGridList)
        {
            item.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }

        if (swapOb.index == GetComponent<GridItem>().core.index)
        {
            yield break;
        }

        BattleManager.Instance.hideButtonCanvas();

        //FloatingTextManager.Instance.addText("Move!", Vector3.zero,Color.white);

        var targetItem = GridManager.Instance.GetItem(GridManager.Instance.GridItemDict, swapOb.index);
        GridManager.Instance.MoveItemToIndex(GetComponent<GridItem>(), swapOb.index);

        GridManager.Instance.UpdateItemPositionToIndexEnumerator(GetComponent<GridItem>());
        if (targetItem!=null)
        {
            GridManager.Instance.UpdateItemPositionToIndexEnumerator(targetItem.GetComponent<GridItem>());
        }

        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(GridManager.Instance.MoveAfter(0, -1));

        BattleManager.Instance.PredictNextAttack();

        swapOb = null;

        StartCoroutine( BattleManager.Instance.MoveTile());
    }
}
