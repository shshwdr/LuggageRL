using System;
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

    private void Start()
    {
        
        textTrans.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    public IEnumerator ShowSlider(string t)
    {
        
        textTrans.gameObject.SetActive(true);
        text.text = t;
        var pos = textTrans.position;
        pos .x=2040;
        textTrans.position = pos;
        slider.DOScaleY(1, GridManager.animTime);
        textTrans.DOLocalMoveX(0, GridManager.animTime);

        yield return new WaitForSeconds(GridManager.animTime*2);

        slider.DOScaleY(0, GridManager.animTime);
        textTrans.DOLocalMoveX(-2040, GridManager.animTime);
        yield return new WaitForSeconds(GridManager.animTime);
        textTrans.gameObject.SetActive(false);
    }
}
