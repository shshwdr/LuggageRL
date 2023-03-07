using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public struct SerializedDictionary<T, U>
{
    public List<T> Keys;
    public List<U> Values;
    public void Clear()
    {
        Keys.Clear();
        Values.Clear();
    }
    public U this[T key]
    {
        get { var index = Keys.IndexOf(key); return Values[index]; }
        set { var index = Keys.IndexOf(key); if (index >= 0) { Values[index] = value; }
            else
            {
                Keys.Add(key);
                Values.Add(value);
            }
        }
    }
    public bool ContainsKey(T key)
    {
        return Keys.Contains(key);
    }
    public Dictionary<T, U> getDictionary()
    {
        Dictionary<T, U> res = new Dictionary<T, U>();
        for (int i = 0; i < Keys.Count; i++)
        {
            res[Keys[i]] = Values[i];
        }
        return res;
    }
    public SerializedDictionary(Dictionary<T, U> dict)
    {
        Keys = dict.Keys.ToList();
        Values = dict.Values.ToList();
    }

    public static implicit operator Dictionary<T, U>(SerializedDictionary<T, U> test)
    {
        return test.getDictionary();
    }
    public static implicit operator SerializedDictionary<T, U>(Dictionary<T, U> test)
    {
        return new SerializedDictionary<T, U>(test);
    }
}
[Serializable]
public enum BuffType { poison, coin}
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

    public string BuffDesc
    {
        get
        {
            var str = "";
            foreach(var b in buffs.Keys)
            {
                str += $"{b.ToString()}: {buffs[b]}\n";
            }
            return str;
        }
    }

    public int CalculateDamage(int rawDamage)
    {
        if (buffs.ContainsKey(BuffType.poison))
        {
            rawDamage += buffs[BuffType.poison];
        }
        return rawDamage;
    }

    public int movedCount = 0;
    //protected Vector3 borderPosition;
    public bool isDestroyed = false;

    bool beHit = false;
    //GridItemCore beHitItem;

    public List<BuffType> buffsKey = new List<BuffType>();
    public List<int> buffsValue = new List<int>();

    public SerializedDictionary<BuffType, int> buffs = new SerializedDictionary<BuffType, int>() { Keys = new List<BuffType>(), Values = new List<int>() };

    public void ApplyBuff(BuffType type, int v)
    {
        if (buffs.ContainsKey(type))
        {
            buffs[type] += v;
        }
        buffs[type] = v;


    }
    public void ClearBuff()
    {
        buffs.Clear();
    }
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
        messages.Add(new MessageDestroy {index = index, item = this });
        //GridManager.Instance.RemoveGrid(index, type);
        Debug.Log($"addDestroyMessage {index} {type}");
    }

    public void addDestroyMessageWithIndex(List<BattleMessage> messages, Vector2Int ind, bool skipAnim = false)
    {
        isDestroyed = true;
        messages.Add(new MessageDestroy { index = index, item = this, skipAnim = skipAnim });
        //GridManager.Instance.RemoveGrid(ind, type);
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
    public void ApplyBuff(BuffType type, int v);
    public void ClearBuff();
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

    public Dictionary<BuffType, int> buffs => core.buffs;
    public void ApplyBuff(BuffType type, int v) { core.ApplyBuff(type, v); baseItem. updateBuff(); }
    public void ClearBuff(){
        core.ClearBuff(); baseItem.updateBuff();
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
