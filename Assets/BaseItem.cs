using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseItem : MonoBehaviour
{
    public Text AttackText;
    public Text HealText;
    public GameObject DestroyOverlay;
    public GameObject buffOverlay;
    public GameObject poisonBuff;
    public SpriteRenderer spriteRender;
    public Text Count;

    public void WillAttack(int damage)
    {
        AttackText.text = damage.ToString();
        AttackText.gameObject.SetActive(true);
    }
    public void WillBeBuff()
    {
        buffOverlay.SetActive(true);
    }
    public void WillHeal(int damage)
    {
        HealText.text = damage.ToString();
        HealText.gameObject.SetActive(true);
    }
    public void WillDestroy()
    {
        DestroyOverlay.SetActive(true);
    }
    public void ClearOverlayes()
    {
        HealText.gameObject.SetActive(false);
        DestroyOverlay.SetActive(false);
        AttackText.gameObject.SetActive(false);
        buffOverlay.SetActive(false);
    }

    public void updateBuff()
    {
        //poisonBuff.SetActive(false);
        foreach (var buff in GetComponent<GridItem>().buffs)
        {
            if (buff.Key is BuffType.poison)
            {
                poisonBuff.SetActive(true);
            }
        }
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
