using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseItem : MonoBehaviour
{
    public Text AttackText;

    public GameObject attackAndDefendOb;
    public Text combinedAttack;
    public Text combinedDefense;

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

    public GameObject previewAttack;
    public GameObject previewSteal;
    bool isBreakable;
    bool willBeDestroyed = false;
    bool willAttack = false;
    public void updateBK()
    {
        itemBK.SetActive(false);
        glassBK.SetActive(false);
        itemBrokenBK.SetActive(false);
        glassBrokenBK.SetActive(false);
        isBreakable = item.core.info.IsBreakable;
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
        updateBK();
    }
    public void WillAttack(int damage)
    {
        willAttack = true;


        AttackText.text = damage.ToString();
        AttackText.gameObject.SetActive(true);
        updateAttack(false);
    }
    public void WillBeBuff()
    {
        //buffOverlay.SetActive(true);
    }
    public void WillHeal(int damage)
    {
        HealText.text = damage.ToString();
        HealText.gameObject.SetActive(true);
    }
    public void WillDestroy()
    {
        willBeDestroyed = true;
        //DestroyOverlay.SetActive(true);
    }
    public void ClearOverlayes()
    {
        HealText.gameObject.SetActive(false);
        //DestroyOverlay.SetActive(false);
        willAttack = false;
        willBeDestroyed = false;
        AttackText.gameObject.SetActive(false);
        buffOverlay.SetActive(false);
        updateAttack(false);
    }
    void updateAttack(bool willAttack)
    {
        AttackText.gameObject.SetActive(willAttack);
        attackAndDefendOb.SetActive(!willAttack);
        combinedAttack.text = item.core.Attack.ToString();
        combinedDefense.text = item.core.defense.ToString();
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
