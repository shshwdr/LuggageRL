using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : BaseScene
{

    public Transform enemyPositionsParent;
    Transform[] enemyPositions;
    // Start is called before the first frame update
    void Start()
    {
        enemyPositions = enemyPositionsParent.GetComponentsInChildren<Transform>();
        BattleManager.Instance.enemyPositions = enemyPositions;
        BattleManager.Instance.AddEnemies();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!hasStarted)
        {
            if (transform.position.x <= 0)
            {
                hasStarted = true;
                //start battle
                BattleManager.Instance.takeControl();
                StageManager.Instance.outControl();
            }
        }
    }
}
