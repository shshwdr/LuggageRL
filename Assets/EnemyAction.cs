using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAction
{
    public virtual string Desc =>EnemyManager.Instance.getEnemyActionInfo(GetType().ToString()).Description;
    public virtual void Preview(Enemy enemy) { }
    public abstract IEnumerator TakeAction(Enemy enemy);
}

public class EnemyActionIdle : EnemyAction
{
    public bool attackFromBottom = true;
    public int attackHeight = 0;
    public int attackDamage = 3;
    public override IEnumerator TakeAction(Enemy enemy)
    {
        FloatingTextManager.Instance.addText(enemy.DisplayName + " is looking at you.",enemy.transform.position,Color.white);
        yield return new WaitForSeconds(GridManager.animTime);
    }

    public override void Preview(Enemy enemy)
    {
        base.Preview(enemy);
        enemy.attackPreview.UpdateOtherPreview("idle", "");
    }
}

public class EnemyActionAttack : EnemyAction
{
    public bool attackFromBottom = true;
    public int attackHeight = 0;
    public int attackDamage = 3;
    public override IEnumerator TakeAction(Enemy enemy)
    {
        yield return enemy.StartCoroutine(enemy.Attack());
        //yield return new WaitForSeconds(GridManager.animTime);
    }
    public override void Preview(Enemy enemy)
    {
        base.Preview(enemy);
        enemy.attackPreview.UpdateAttackPreview(attackHeight,attackFromBottom,attackDamage);
    }
}

public class EnemyActionShield : EnemyAction
{
    public int shieldAmount = 5;
    public override IEnumerator TakeAction(Enemy enemy)
    {
        yield return enemy.StartCoroutine(enemy.AddShield(shieldAmount));
        //yield return new WaitForSeconds(GridManager.animTime);
    }
    public override void Preview(Enemy enemy)
    {
        base.Preview(enemy);
        enemy.attackPreview.UpdateOtherPreview("shield", shieldAmount.ToString());
    }
}
public class EnemyActionHeal : EnemyAction
{
    public int healAmount = 5;
    public override IEnumerator TakeAction(Enemy enemy)
    {
        yield return enemy.StartCoroutine(enemy.HealMinHP(healAmount));
        //yield return new WaitForSeconds(GridManager.animTime);
    }
    public override void Preview(Enemy enemy)
    {
        base.Preview(enemy);
        enemy.attackPreview.UpdateOtherPreview("heal", healAmount.ToString());
    }
}
public class EnemyActionSteal : EnemyAction
{
    ItemType stealItem;
    public override IEnumerator TakeAction(Enemy enemy)
    {

        yield return enemy.StartCoroutine(enemy.StealItem(stealItem));
        //yield return new WaitForSeconds(GridManager.animTime);
    }
    public override void Preview(Enemy enemy)
    {
        base.Preview(enemy);

        //find the most item
        stealItem = GridManager.Instance.findMostItem();

        enemy.attackPreview.UpdateOtherPreview(stealItem.ToString(), "steal",false);
    }
}

public class EnemyActionAdd : EnemyAction
{
    public ItemType addItem;
    public override IEnumerator TakeAction(Enemy enemy)
    {

        yield return enemy.StartCoroutine(enemy.AddItem(addItem));
        //yield return new WaitForSeconds(GridManager.animTime);
    }
    public override void Preview(Enemy enemy)
    {
        base.Preview(enemy);


        enemy.attackPreview.UpdateOtherPreview(addItem.ToString(), "add", false);
    }
}

public class EnemyActionStealMax : EnemyAction
{
    public override IEnumerator TakeAction(Enemy enemy)
    {

        var stealItem = GridManager.Instance.findMostItem();
        yield return enemy.StartCoroutine(enemy.StealItem(stealItem));
        //yield return new WaitForSeconds(GridManager.animTime);
    }
    public override void Preview(Enemy enemy)
    {
        base.Preview(enemy);

        //find the most item

        enemy.attackPreview.UpdateOtherPreview("max", "steal");
    }
}