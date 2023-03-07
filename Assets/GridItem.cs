using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public static class Ut
{
    public static T DeepClone<T>(this T obj)
    {
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;

            return (T)formatter.Deserialize(ms);
        }
    }
}
[System.Serializable]
public class GridItemCore: IGridItem
{
    public ItemType type;
    public int indexx;
    public int indexy;
    public GridItemCore Core => this;
    public Vector2Int index
    {
        get { return new Vector2Int(indexx, indexy); }
        set { indexx = value.x; indexy = value.y; }
    }
    public int defense = 2;
    public string Name;
    public virtual string Desc => $@"{Name}
defense: {defense}";

    protected int movedCount = 0;
    //protected Vector3 borderPosition;
    public bool isDestroyed = false;

    bool beHit = false;
    //GridItemCore beHitItem;


    public virtual void finishedAttack()
    {
        movedCount = 0;
    }
    public virtual void hitBorder(List<BattleMessage> messages, Vector2Int borderIndex) { }
    public virtual void move(List<BattleMessage> messages) { movedCount++; }
    public virtual void beCrushed(IGridItem item, List<BattleMessage> messages) { }

    public void addDestroyMessage(List<BattleMessage> messages)
    {
        isDestroyed = true;
        messages.Add(new MessageDestroy { item = this });
        GridManager.Instance.RemoveGrid(index, type);
        Debug.Log($"addDestroyMessage {index} {type}");
    }

    public void addDestroyMessageWithIndex(List<BattleMessage> messages, Vector2Int ind, bool skipAnim = false)
    {
        isDestroyed = true;
        messages.Add(new MessageDestroy { item = this, skipAnim = skipAnim });
        GridManager.Instance.RemoveGrid(ind, type);
        Debug.Log($"addDestroyMessage {ind} {type}");
    }
}

public interface IGridItem
{
    public void hitBorder(List<BattleMessage> messages, Vector2Int borderIndex);
    public void move(List<BattleMessage> messages);
    public void beCrushed(IGridItem item, List<BattleMessage> messages);
    public void addDestroyMessage(List<BattleMessage> messages);
    public void addDestroyMessageWithIndex(List<BattleMessage> messages, Vector2Int ind, bool skipAnim = false);
    public Vector2Int index { get; set; }
    public GridItemCore Core { get; }
}

public class GridItem : MonoBehaviour, IGridItem
{
    public GridItemCore Core => core;
    public Vector2Int index { get { return core.index; } set { core.index = value; } }
    public GridItemCore core;
    public virtual void finishedAttack()
    {
        core.finishedAttack();
    }
    public virtual void hitBorder(List<BattleMessage> messages, Vector2Int borderIndex) { core.hitBorder(messages, borderIndex); }
    public virtual void move(List<BattleMessage> messages) { core.move(messages); }
    public virtual void beCrushed(IGridItem item, List<BattleMessage> messages) { core.beCrushed(item,messages); }
    public BaseItem baseItem;
    public ItemType type { get { return core.type; } }
    public void init(Vector2Int ind,ItemType t)
    {

        switch (t)
        {
            case ItemType.ore:
                core = new Ore();
                break;
            case ItemType.arrow:
                core = new Arrow();
                break;
            case ItemType.herb:
                core = new Herb();
                break;
            case ItemType.poison:
                core = new Poison();
                break;
        }
        core.type = t;
        core.index = ind;
    }
    public void addDestroyMessage(List<BattleMessage> messages)
    {
        core.addDestroyMessage(messages);
    }

    public void addDestroyMessageWithIndex(List<BattleMessage> messages,Vector2Int ind, bool skipAnim = false)
    {
        core.addDestroyMessageWithIndex(messages,ind,skipAnim);
    }
    public void destory()
    {
        transform.DOShakeScale(GridManager.animTime);
        //Debug.Log($"destroy {index} {type}");
        Destroy(gameObject, GridManager.animTime);
    }
    // Start is called before the first frame update
    void Awake()
    {
        baseItem = GetComponent<BaseItem>();

    }
    private void OnMouseEnter()
    {
        GridManager.Instance.itemViewText.text = core.Desc;
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
