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
public class GridItemCore
{

}
public class GridItem : MonoBehaviour
{
   
    
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
    public BaseItem baseItem;


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
        baseItem = GetComponent<BaseItem>();
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
