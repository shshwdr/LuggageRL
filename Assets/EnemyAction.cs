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
    public override IEnumerator TakeAction(Enemy enemy)
    {
        FloatingTextManager.Instance.addText(enemy.DisplayName + " is looking at you.", enemy.transform.position, Color.white);
        yield return new WaitForSeconds(GridManager.animTime);
    }

    public override void Preview(Enemy enemy)
    {
        base.Preview(enemy);
        enemy.attackPreview.UpdateOtherPreview("idle", "");
    }
}
public class EnemyActionRotate : EnemyAction
{
    public override IEnumerator TakeAction(Enemy enemy)
    {
        yield return enemy.StartCoroutine(enemy.RotateBag());

        GridManager.Instance.updateAttackEdge();
    }

    public override void Preview(Enemy enemy)
    {
        base.Preview(enemy);
        enemy.attackPreview.UpdateOtherPreview("rotate", "");
    }
}

public class EnemyActionAttack : EnemyAction
{
    public bool attackFromBottom = true;
    public int attackHeight = 0;
    public int attackRange = 1;
    public int attackRangeVerticle = 1;
    public int attackDamage = 0;
    public override IEnumerator TakeAction(Enemy enemy)
    {
        yield return enemy.StartCoroutine(enemy.Attack());
        //yield return new WaitForSeconds(GridManager.animTime);
    }
    public override void Preview(Enemy enemy)
    {
        base.Preview(enemy);
        enemy.attackPreview.UpdateAttackPreview(attackHeight,attackFromBottom,enemy.attack);
    }
}

public class EnemyActionShield : EnemyAction
{
    public int shieldAmount = 5;
    public override IEnumerator TakeAction(Enemy enemy)
    {
        yield return enemy.StartCoroutine(enemy.AddShield(enemy.defense));
        //yield return new WaitForSeconds(GridManager.animTime);
    }
    public override void Preview(Enemy enemy)
    {
        base.Preview(enemy);
        enemy.attackPreview.UpdateOtherPreview("shield", enemy.defense.ToString());
    }
}
public class EnemyActionHeal : EnemyAction
{
    public int healAmount = 5;
    public override IEnumerator TakeAction(Enemy enemy)
    {
        yield return enemy.StartCoroutine(enemy.HealMinHP(enemy.heal));
        //yield return new WaitForSeconds(GridManager.animTime);
    }
    public override void Preview(Enemy enemy)
    {
        base.Preview(enemy);
        enemy.attackPreview.UpdateOtherPreview("heal", enemy.heal.ToString());
    }
}
public class EnemyActionSteal : EnemyAction
{
    public ItemType stealItem;
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

        enemy.attackPreview.UpdateOtherPreviewTwoImage("steal", stealItem.ToString());

    }
}

public class EnemyActionAdd : EnemyAction
{
    public ItemType addItem;
    public int amount = 1;
    public override IEnumerator TakeAction(Enemy enemy)
    {

        yield return enemy.StartCoroutine(enemy.AddItem(addItem, amount));
        //yield return new WaitForSeconds(GridManager.animTime);
    }
    public override void Preview(Enemy enemy)
    {
        base.Preview(enemy);


        enemy.attackPreview.UpdateOtherPreviewTwoImage("add", addItem.ToString());
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

        enemy.attackPreview.UpdateOtherPreview("steal","");
    }
}