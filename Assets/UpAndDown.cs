using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDown : MonoBehaviour
{
    float originalY ;
    public float moveHeight = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        originalY = transform.position.y;
        transform.DOMoveY(originalY + moveHeight, 0.3f).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
