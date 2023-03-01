using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem : MonoBehaviour
{
    public Vector2Int index;
    public virtual void hitBorder() { }
    public virtual void bigHitBorder() { }
    public virtual void beCrushed(GridItem item) { }
    bool willHitBorder = false;
    bool wasMoving = false;
    protected Vector3 borderPosition;

    bool beHit = false;
    GridItem beHitItem;

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
    public void BeHit(GridItem item)
    {
        beHit = true;
        beHitItem = item;
    }


    public IEnumerator move(Vector3 targetPos, float animTime) {

        transform.DOLocalMove(targetPos, animTime);
        yield return new WaitForSeconds(animTime);

    }

    public void calculateHit()
    {
        var str = "";
        if (willHitBorder)
        {
            if (wasMoving)
            {
                str += " big ";
                bigHitBorder();
            }
            else
            {
                hitBorder();
            }
            str += " hit ";
           // hitBorder();
           // FloatingTextManager.Instance.addText(str, borderPosition);
        }

        if (beHit)
        {
            beCrushed(beHitItem);
        }

        willHitBorder = false;
        wasMoving = false;
        beHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
