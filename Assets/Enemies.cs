using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{

}

public class EnemyBehavior {
    public Enemy enemy;
    public virtual string Name { get; }
    public virtual void SelectAction() {
        actions[i].Preview(enemy);
    }

    public EnemyAction[] actions;
    int i = 0;

}

//if (attackFromBottom)
//{
//    attackInd = Random.Range(0, 3);
//    attackPreview.UpdatePreview(attackInd, attackFromBottom);
//}

public class DummyEnemy : EnemyBehavior
{
    public DummyEnemy()
    {
        actions = new EnemyAction[] { new EnemyActionIdle() };
    }
    public override string Name => "dummy";

}

