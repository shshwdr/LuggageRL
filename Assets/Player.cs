using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : HPObject
{
    protected override IEnumerator DieInteral()
    {
        yield return StartCoroutine(base.DieInteral());
        FloatingTextManager.Instance.addText("Game Over", Vector3.zero, Color.red);
        yield return new WaitForSeconds(GridManager.animTime);

    }
}
