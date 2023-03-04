using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : GridItem
{
    [SerializeField] private int baseAttackDamage = 1; 
    [SerializeField] private int bigAttackMultiplier = 2; 
        
    public override void hitBorder() { 
        
        FloatingTextManager.Instance.addText($"Attack! ({baseAttackDamage})", transform.position);
        Luggage.Instance.DoDamage(baseAttackDamage);
    }
    public override void bigHitBorder() {
        int finalDamage = baseAttackDamage * bigAttackMultiplier;
        FloatingTextManager.Instance.addText($"Big Attack! ({finalDamage})", transform.position);
        Luggage.Instance.DoDamage(finalDamage);
    }
    public override void beCrushed(GridItem item)
    {

    }
}
