using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BaseItem : MonoBehaviour
{
    //public Text AttackText;

    //public GameObject attackAndDefendOb;
    public Text combinedAttack;
    public Text combinedDefense;

    public Text beAttackedText;
    
    public Text HealText;
    public GameObject DestroyOverlay;
    public GameObject buffOverlay;
    public GameObject poisonBuff;
    public SpriteRenderer spriteRender;
    public Text Count;

    public GameObject itemBK;
    public GameObject glassBK;
    public GameObject itemBrokenBK;
    public GameObject glassBrokenBK;
    public GridItem item;

    public GameObject willBeAttackedObj;
    public GameObject willBeStokenObj;

    public GameObject previewAttack;
    public GameObject previewSteal;
    bool isBreakable;
    public bool willBeDestroyed = false;
    public bool willAttack = false;
    public bool willDefend = false;
    public void updateBK()
    {
        itemBK.SetActive(false);
        glassBK.SetActive(false);
        itemBrokenBK.SetActive(false);
        glassBrokenBK.SetActive(false);
        isBreakable = GetComponent<GridItem>().core.info.IsBreakable;
        if (isBreakable)
        {
            if (willBeDestroyed)
            {
                glassBrokenBK.SetActive(true);
            }
            else
            {
                glassBK.SetActive(true);
            }
        }
        else
        {

            if (willBeDestroyed)
            {
                itemBrokenBK.SetActive(true);
            }
            else
            {
                itemBK.SetActive(true);
            }
        }
    }
    void Start()
    {
        item = GetComponent<GridItem>();
    }
    public void WillAttack(int damage)
    {
        willAttack = true;

        combinedAttack.text = damage.ToString();
        //AttackText.text = damage.ToString();
        //AttackText.gameObject.SetActive(true);
        combinedAttack.GetComponent<RectTransform>().sizeDelta = largeSize;
        combinedAttack.GetComponent<Outline>().effectColor = outlineColorTarget;
        //DOTween.To(()=> combinedAttack.GetComponent<Outline>().effectColor, x=> combinedAttack.GetComponent<Outline>().effectColor = x, outlineColorTarget, 1).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.Linear);
        updateAttack(true);
        updateBK();
    }
    public void WillBeBuff()
    {
        //buffOverlay.SetActive(true);
    }
    public void WillHeal(int damage)
    {
        HealText.text = damage.ToString();
        HealText.gameObject.SetActive(true);
        updateBK();
    }
    public void WillDestroy()
    {
        willBeDestroyed = true;
        updateBK();
        //DestroyOverlay.SetActive(true);
    }
    public void ClearOverlayes()
    {
        HealText.gameObject.SetActive(false);
        //DestroyOverlay.SetActive(false);
        willAttack = false;
        willBeDestroyed = false;
        //AttackText.gameObject.SetActive(false);
        
        combinedAttack.GetComponent<RectTransform>().sizeDelta = normalSize;
        
        //.GetComponent<Outline>().DOKill();
        combinedAttack.GetComponent<Outline>().effectColor = outlineColorOrigin;
        
        buffOverlay.SetActive(false);
        updateAttack(false);


        updateBK();
    }

    private Color outlineColorOrigin = new Color(0,0,0,0f);
    private Color outlineColorTarget = Color.black;
    private float outlineChangeTime = 0.3f;
    private Vector2 smallSize = new Vector2(70, 70);
    private Vector2 largeSize = new Vector2(140, 140);
    private Vector2 normalSize = new Vector2(90, 90);
    
    public void ClearWillBeAttacked()
    {
        //willBeAttackedObj.SetActive(false);
        willDefend = false;
        combinedDefense.GetComponent<RectTransform>().sizeDelta = normalSize;
        //combinedDefense.GetComponent<Outline>().DOKill();
        combinedDefense.GetComponent<Outline>().effectColor = outlineColorOrigin;
        beAttackedText.gameObject.SetActive(false);
    }
    public void WillBeAttacked(int attack)
    {
        beAttackedText.gameObject.SetActive(true);
        beAttackedText.text = attack.ToString();
        willDefend = true;
        //willBeAttackedObj.SetActive(true);
        combinedDefense.GetComponent<RectTransform>().sizeDelta = largeSize;
        combinedDefense.GetComponent<Outline>().effectColor = outlineColorTarget;
        //DOTween.To(()=> combinedDefense.GetComponent<Outline>().effectColor, x=> combinedDefense.GetComponent<Outline>().effectColor = x, outlineColorTarget, 1).SetLoops(-1,LoopType.Yoyo);

    }

    void updateAttack(bool willAttack)
    {
        //if (willAttack)
        //{
        //    Debug.Log("will attack");
        //}
        //AttackText.gameObject.SetActive(willAttack);
        //attackAndDefendOb.SetActive(!willAttack && !isBreakable);
        //combinedAttack.text = item.core.Attack.ToString();
        if (item == null) 
        {
            
            item = GetComponent<GridItem>();
        }
        combinedDefense.text = item.core.defense.ToString();


        willBeStokenObj.SetActive(false);
        foreach (var enemy in EnemyManager.Instance.GetEnemies())
        {
            if(enemy.Core.currentAction is EnemyActionSteal stealAction&&stealAction.stealItem == GetComponent<GridItem>().core.type)
            {

                willBeStokenObj.SetActive(true);
            }
        }
    }
    public void updateBuff()
    {
        ////poisonBuff.SetActive(false);
        //foreach (var buff in GetComponent<GridItem>().buffs)
        //{
        //    if (buff.Key is BuffType.poison)
        //    {
        //        poisonBuff.SetActive(true);
        //    }
        //}
    }
    public void updateCounter(int count)
    {
        if(count > 0)
        {

            Count.gameObject.SetActive(true);
            Count.text = count.ToString();
        }
    }
}
