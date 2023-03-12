using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnSlider : MonoBehaviour
{
    public Transform slider;
    public Transform textTrans;
    public Text text;
    // Start is called before the first frame update
    public IEnumerator ShowSlider(string t)
    {
        
        text.text = t;
        var pos = textTrans.position;
        pos .x=1440;
        textTrans.position = pos;
        slider.DOScaleY(1, GridManager.animTime);
        textTrans.DOLocalMoveX(0, GridManager.animTime);

        yield return new WaitForSeconds(GridManager.animTime*2);

        slider.DOScaleY(0, GridManager.animTime);
        textTrans.DOLocalMoveX(-1440, GridManager.animTime);
        yield return new WaitForSeconds(GridManager.animTime);

    }
}
