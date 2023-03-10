using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior {
    public Enemy enemy;
    public virtual string Name { get; }
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
        actions = new EnemyAction[] { new EnemyActionIdle() };
    }
    public override string Name => "dummy";

}
public class SimpleAttackEnemy : EnemyBehavior
{
    public SimpleAttackEnemy()
    {
        actions = new EnemyAction[] { new EnemyActionAttack(),new EnemyActionIdle(),new EnemyActionAttack(), };
    }
    public override string Name => "simple";

}

