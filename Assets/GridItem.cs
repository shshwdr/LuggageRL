using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem : MonoBehaviour
{
    public ItemType type;
    public Vector2Int index;
    public int defense = 2;
    public string Name;
    public virtual string Desc => $@"{Name}
defense: {defense}";
    public virtual void finishedAttack()
    {
        movedCount = 0;
    }
    public virtual void hitBorder(List<BattleMessage> messages, Vector2Int borderIndex) { }
    public virtual void move(List<BattleMessage> messages) { movedCount++; }
    public virtual void beCrushed(GridItem item, List<BattleMessage> messages) { }
    bool willHitBorder = false;
    bool wasMoving = false;
    protected int movedCount = 0;
    protected Vector3 borderPosition;
    public bool isDestroyed = false;

    bool beHit = false;
    GridItem beHitItem;

    public void addDestroyMessage(List<BattleMessage> messages)
    {
        isDestroyed = true;
        messages.Add(new MessageDestroy { item = this });
        GridManager.Instance.RemoveGrid(index, type);
        Debug.Log($"addDestroyMessage {index} {type}");
    }

    public void addDestroyMessageWithIndex(List<BattleMessage> messages,Vector2Int ind, bool skipAnim = false)
    {
        isDestroyed = true;
        messages.Add(new MessageDestroy { item = this ,skipAnim = skipAnim});
        GridManager.Instance.RemoveGrid(ind, type);
        Debug.Log($"addDestroyMessage {ind} {type}");
    }
    public void destory()
    {
        transform.DOShakeScale(GridManager.animTime);
        //Debug.Log($"destroy {index} {type}");
        Destroy(gameObject, GridManager.animTime);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnMouseEnter()
    {
        GridManager.Instance.itemViewText.text = Desc;
    }
    private void OnMouseExit()
    {

        GridManager.Instance.itemViewText.text = "";
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
