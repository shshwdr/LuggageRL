using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem : MonoBehaviour
{
    bool willHitBorder = false;
    bool wasMoving = false;
    Vector3 borderPosition;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void hitBorder(bool hit, bool moved, Vector3 hitPos)
    {
        willHitBorder = hit;
        wasMoving = moved;
        borderPosition = hitPos;
    }
    public IEnumerator move(Vector3 targetPos, float animTime) {

        transform.DOLocalMove(targetPos, animTime);
        yield return new WaitForSeconds(animTime);
        calculateHit();

    }

    public void calculateHit()
    {
        var str = "";
        if (willHitBorder)
        {
            if (wasMoving)
            {
                str += " big ";
            }
            str += " hit ";
            FloatingTextManager.Instance.addText(str, borderPosition);
        }
        willHitBorder = false;
        wasMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
