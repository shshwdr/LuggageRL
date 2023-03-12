using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : HPObject
{
    protected override IEnumerator DieInteral()
    {
        yield return StartCoroutine(base.DieInteral());
        Luggage.Instance.playDeathSequence();
        //FloatingTextManager.Instance.addText("Game Over", Vector3.zero, Color.red);
        yield return new WaitForSeconds(GridManager.animTime);
        GameOver.Instance.startGameOver(false);

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
