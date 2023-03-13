using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    public float time = 1;
    // Start is called before the first frame update
    void Start()
    {
        var rend = GetComponent<SpriteRenderer>();
        DOTween.To(() => rend.color, x => rend.color = x, new Color(1,1,1,0.5f), time).SetLoops(-1,LoopType.Yoyo);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
