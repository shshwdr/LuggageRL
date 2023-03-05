using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem : MonoBehaviour
{
    public ItemType type;
    public Vector2Int index;
    public int defense = 2;
    public virtual void hitBorder(List<IBattleMessage> messages) { }
    public virtual void move(List<IBattleMessage> messages) { }
    public virtual void bigHitBorder(List<IBattleMessage> messages) { }
    public virtual void beCrushed(GridItem item, List<IBattleMessage> messages) { }
    bool willHitBorder = false;
    bool wasMoving = false;
    protected int movedCount = 0;
    protected Vector3 borderPosition;
    public bool isDestroyed = false;

    bool beHit = false;
    GridItem beHitItem;

    public void addDestroyMessage(List<IBattleMessage> messages)
    {
        isDestroyed = true;
        messages.Add(new MessageDestroy { item = this });
        GridManager.Instance.RemoveGrid(index, type);
        Debug.Log($"addDestroyMessage {index} {type}");
    }
    public void destory()
    {
        Debug.Log($"destroy {index} {type}");
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void hitBorder(bool hit, Vector2Int movedDistance, Vector3 hitPos)
    {
        willHitBorder = hit;
        wasMoving = movedDistance!=Vector2Int.zero;
        movedCount = (int)movedDistance.magnitude;
        borderPosition = hitPos;
    }
    public void BeHit(GridItem item)
    {
        beHit = true;
        beHitItem = item;
    }


    public IEnumerator move(Vector3 targetPos, float animTime) {

        transform.DOLocalMove(targetPos, animTime);
        yield return new WaitForSeconds(animTime);

    }

    //public void calculateHit()
    //{
    //    var str = "";
    //    if (willHitBorder)
    //    {
    //        if (wasMoving)
    //        {
    //            str += " big ";
    //            bigHitBorder();
    //        }
    //        else
    //        {
    //            hitBorder();
    //        }
    //        str += " hit ";
    //       // hitBorder();
    //       // FloatingTextManager.Instance.addText(str, borderPosition);
    //    }

    //    if (beHit)
    //    {
    //        beCrushed(beHitItem);
    //    }

    //    willHitBorder = false;
    //    wasMoving = false;
    //    movedCount = 0;
    //    beHit = false;
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
