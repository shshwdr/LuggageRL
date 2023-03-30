using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : HPObject
{
    public GameObject attackOb;
    public Text attackText;
    protected override IEnumerator DieInteral()
    {
        yield return StartCoroutine(base.DieInteral());
        Luggage.Instance.playDeathSequence();
        //FloatingTextManager.Instance.addText("Game Over", Vector3.zero, Color.red);
        yield return new WaitForSeconds(GridManager.animTime);
        GameOver.Instance.startGameOver(false);

    }

    public override IEnumerator ApplyDamage(int damage, bool isFinalAttack = true)
    {
        yield return StartCoroutine( base.ApplyDamage(damage,isFinalAttack));

        if (damage > 0)
        {
            
            bool hasRevenge = false;
            foreach (var gridItem in GridManager.Instance.GridItemDict.Values)
            {
                if (gridItem.type == ItemType.HolyGrail)
                {
                
                    FloatingTextManager.Instance.addText($"Apply {BuffType. revenge.ToString()}", gridItem.transform.position, Color.red);
                    gridItem.core.ApplyBuff(BuffType. revenge,1);
                    hasRevenge = true;
                }
            }

            if (hasRevenge == true)
            {
                yield return new WaitForSeconds(GridManager.animTime);
            }
        }
    }

    public void updateHPFromUpgrade()
    {
        if (!LuggageManager.Instance.UpgradedTime.ContainsKey(UpgradeType.hp))
        {
            return;
        }
        for (int i = 0;i< LuggageManager.Instance.UpgradedTime[UpgradeType.hp]; i++)
        {
            addMaxHP(10);
        } 
    }

    public void addMaxHP(int value)
    {
        maxHP += value;
        hp += value;

        hpbar.updateHPBar(hp, maxHP);
    }

    public override void reactToDamage()
    {
        Luggage.Instance.playHurtAnimation();
    }
}
