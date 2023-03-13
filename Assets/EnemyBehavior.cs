using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior {
    public Enemy enemy;

    public EnemyAction currentAction => actions[i];
    public bool willAttacking => currentAction is EnemyActionAttack;
    public virtual void SelectAction() {
        actions[i].Preview(enemy);
    }
    public virtual IEnumerator TakeAction()
    {
        yield return enemy.StartCoroutine(actions[i].TakeAction(enemy));

        i++;
        if (i >= actions.Length)
        {
            i = 0;
        }
    }

    public EnemyAction[] actions;
    int i = 0;

}


public class DummyEnemy : EnemyBehavior
{
    public DummyEnemy()
    {
        actions = new EnemyAction[] { new EnemyActionIdle() }; ///I'm changing this around for debugging purposes. 
    }

}
public class SimpleAttackEnemy : EnemyBehavior
{
    public SimpleAttackEnemy()
    {
        actions = new EnemyAction[] { new EnemyActionAttack() { attackDamage = 0 }, new EnemyActionIdle(), new EnemyActionAttack(), };
    }

}
public class AttackShieldEnemy : EnemyBehavior
{
    public AttackShieldEnemy()
    {
        actions = new EnemyAction[] { new EnemyActionShield() { shieldAmount = 0 }, new EnemyActionAttack() { attackDamage = 0, attackHeight = 0 }, new EnemyActionIdle() };
    }

}
public class AttackFlyEnemy : EnemyBehavior
{
    public AttackFlyEnemy()
    {
        actions = new EnemyAction[] { new EnemyActionAttack() { attackDamage = 0, attackFromBottom = false }, new EnemyActionIdle() ,new EnemyActionAttack() { attackDamage = 2, attackFromBottom = false },};
    }
}
public class AttackGrowEnemy : EnemyBehavior
{
    public AttackGrowEnemy()
    {
        actions = new EnemyAction[] { new EnemyActionAttack() { attackDamage =0, attackHeight = 0 }, new EnemyActionAttack() { attackDamage = 1, attackHeight = 1 }, new EnemyActionAttack() { attackDamage =2, attackHeight = 2 }, new EnemyActionIdle() };
    }
}
public class AttackHealEnemy : EnemyBehavior
{
    public AttackHealEnemy()
    {
        actions = new EnemyAction[] { new EnemyActionHeal() { healAmount = -1 }, new EnemyActionAttack() { attackDamage = 0, attackHeight = 1 }, new EnemyActionIdle() };
    }
}
public class ShieldHealEnemy : EnemyBehavior
{
    public ShieldHealEnemy()
    {
        actions = new EnemyAction[] { new EnemyActionShield() { shieldAmount = 0 }, new EnemyActionHeal() { healAmount = -1 }, new EnemyActionIdle() };
    }
}
public class StealAttackEnemy : EnemyBehavior
{
    public StealAttackEnemy()
    {
        actions = new EnemyAction[] { new EnemyActionSteal() { }, new EnemyActionIdle(), new EnemyActionAttack() { attackDamage = 0 }, new EnemyActionShield() { shieldAmount = 2} };
    }
}

public class AttackStealEnemy : EnemyBehavior
{
    public AttackStealEnemy()
    {
        actions = new EnemyAction[] { new EnemyActionAttack() { attackDamage =0 }, new EnemyActionStealMax() { }, new EnemyActionHeal() { healAmount = -1 } ,new EnemyActionIdle()};
    }
}
public class AttackRotateEnemy : EnemyBehavior
{
    public AttackRotateEnemy()
    {
        actions = new EnemyAction[] { new EnemyActionAttack() { attackDamage = 0 }, new EnemyActionRotate() { }, };
    }
}

public class EliteCap : EnemyBehavior {

    public EliteCap()
    {
        actions = new EnemyAction[] { new EnemyActionAttack() { attackDamage = -2, attackHeight = 1 }, new EnemyActionShield() { shieldAmount = 5}, new EnemyActionAttack() { attackDamage = -1 }, new EnemyActionSteal(), new EnemyActionIdle() };
    }
}
public class EliteThorn : EnemyBehavior {

    public EliteThorn()
    {
        actions = new EnemyAction[] { new EnemyActionAttack() { attackDamage = 0, attackHeight = 1 },new EnemyActionRotate(), new EnemyActionAttack() { attackDamage = +2, attackHeight = 2 }, new EnemyActionShield() { shieldAmount = 5} };
    }
}
public class BossPinata : EnemyBehavior
{
    public BossPinata()
    {
        actions = new EnemyAction[] { new EnemyActionAdd() { addItem = ItemType.Mud },new EnemyActionShield() { shieldAmount = 7}, new EnemyActionAdd() { addItem = ItemType.Mud, amount = 2 }, new EnemyActionAttack() { attackDamage =  2, attackHeight = 1 }, new EnemyActionAdd() { addItem = ItemType.Mud, amount = 3} };
    }
}
public class BossExplode : EnemyBehavior
{
    public BossExplode()
    {
        actions = new EnemyAction[] { new EnemyActionAdd() { addItem = ItemType.LiquidBomb } ,new EnemyActionAttack() { attackDamage =2, attackHeight = 1 }, new EnemyActionSteal(), new EnemyActionIdle(), new EnemyActionAttack() { attackDamage = 0, attackHeight = 2 } };
    }
}
public class BossFreeze : EnemyBehavior
{
    public BossFreeze()
    {
        actions = new EnemyAction[] { new EnemyActionAdd() { addItem = ItemType.FreezeBomb } };
    }
}

