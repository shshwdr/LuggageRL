using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour
{
    Color additiveDarkenColor = new Color(.75f, .75f, .75f, 1f);
    Color shadowColor = new Color(0, 0, 0, .2f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool mouseDown = false;
    private void OnMouseDown()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (!BattleManager.Instance.isInControl || !BattleManager.Instance.CanPlayerControl || BabySittingTutorial.Instance.shouldDisableAction(PlayerActionType.Move))
        {
            return;
        }

        mouseDown = true;
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_drag_pickup, transform.position);
    }
    GridEmptyCell swapOb;
    private void OnMouseDrag()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (!mouseDown)
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
            item.GetComponentInChildren<SpriteRenderer>().color = Color.clear;
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
                swapOb.GetComponentInChildren<SpriteRenderer>().color = shadowColor;
            }
            else
            {
                GridManager.Instance.GetItem(GridManager.Instance.GridItemDict, swapOb.index).GetComponentInChildren<SpriteRenderer>().color = additiveDarkenColor;
            }
        }
    }
    private void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_item_drag_putdown, transform.position);
        if (swapOb)
        {
            StartCoroutine(mouseUp());
        }
        else
        {
            GridManager.Instance.updatePos(GetComponent<GridItem>());

        }
        
        
        GridManager.Instance.showAllAttackPreview();

        mouseDown = false;
    }

    IEnumerator mouseUp()
    {


        foreach (var item in GridManager.Instance.GridItemDict.Values)
        {
            item.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
        foreach (var item in GridManager.Instance.emptyGridList)
        {
            item.GetComponentInChildren<SpriteRenderer>().color = Color.clear;
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
