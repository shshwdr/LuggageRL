using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : HPObject
{
    protected override void DieInteral()
    {
        base.DieInteral();
        FloatingTextManager.Instance.addText("Game Over", Vector3.zero, Color.red);
    }
}
