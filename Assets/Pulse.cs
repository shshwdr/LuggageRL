using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    public float time = 1;
    public float moveDistance = 0.3f;
    public float offset = 0;

    public bool isVerticle = false;
    // Start is called before the first frame update
    void Start()
    {
Invoke("move",offset);
    }

    void move()
    {
        
        var rend = GetComponent<SpriteRenderer>();
        rend.color = new Color(1, 1, 1, 0f);
        DOTween.To(() => rend.color, x => rend.color = x, new Color(1, 1, 1, 1f), time / 2).SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
        if (isVerticle)
        {
            
            transform.DOLocalMoveY(moveDistance, time).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        }
        else
        {
            
            transform.DOLocalMoveX(moveDistance, time).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
