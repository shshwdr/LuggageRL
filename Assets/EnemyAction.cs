using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction
{
    public virtual void Preview(Enemy enemy) { }
}

public class EnemyActionIdle : EnemyAction
{
    public bool attackFromBottom = true;
    public int attackHeight = 0;
    public int attackDamage = 3;
    public IEnumerator TakeAction(Enemy enemy)
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
    public IEnumerator TakeAction()
    {

        yield return new WaitForSeconds(GridManager.animTime);
    }
}

