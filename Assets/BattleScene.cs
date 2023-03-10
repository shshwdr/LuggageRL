using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : BaseScene
{
    public BattleType battleType;
    // Start is called before the first frame update
    void Start()
    {
        itemPositions = itemPositionsParent.GetComponentsInChildren<Transform>();
        BattleManager.Instance.enemyPositions = itemPositions;
        BattleManager.Instance.itemsToActivate = itemsToActivate;
        BattleManager.Instance.AddEnemies(battleType);
        BattleManager.Instance.baseScene = this;
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
                StageManager.Instance.outControl();
                BattleManager.Instance.takeControl();
            }
        }
    }
}
