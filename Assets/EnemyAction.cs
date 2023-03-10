using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction
{

}

public class EnemyActionIdle : EnemyAction
{
    public bool attackFromBottom = true;
    public int attackHeight = 0;
    public int attackDamage = 3;
    public IEnumerator TakeAction()
    {

        yield return new WaitForSeconds(GridManager.animTime);
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

