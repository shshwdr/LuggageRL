using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herb : GridItem
{
    public override void hitBorder() { }
    public override void bigHitBorder() { }
    public override void beCrushed(GridItem item) {
        BattleManager.Instance.player.Heal(3);
        FloatingTextManager.Instance.addText("Heal!", transform.position);
    }
}
