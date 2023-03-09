using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : HPObject
{
    EnemyAttackPreview attackPreview;
    public int attack = 3;
    Vector3 originalPosition;
    public Transform leftTargetTransform; //where player should impact
    public Transform topTargetTransform;
    public Collider2D enemyCollider;
    public GameObject targetedIndicator;

    public int attackInd;
    public bool attackFromBottom = true;

    // Start is called before the first frame update
    protected  override void Awake()
    {
        hp = maxHP;
        base.Awake();
        EnemyManager.Instance.AddEnemy(this);
        attackPreview = GetComponentInChildren<EnemyAttackPreview>();
    }
    int damage = 0;
    private bool isTargeted;

    public void GetDamage(int dam)
    {
        damage += dam;
    }
    public void ClearDamage()
    {
        damage = 0;
    }
    protected override IEnumerator DieInteral()
    {
        yield return StartCoroutine( base.DieInteral());

        transform.DOShakeScale(GridManager.animTime);
        yield return new WaitForSeconds(GridManager.animTime);
        transform.DOLocalMoveY(-10, GridManager.animTime);
        //Destroy(gameObject,GridManager.animTime);
        yield return StartCoroutine(EnemyManager.Instance.RemoveEnemy(this));
    }
    public IEnumerator ShowDamage()
    {
        yield return  StartCoroutine( ApplyDamage(damage));
    }
    public void SelectAttack()
    {
        if (attackFromBottom)
        {
            attackInd = Random.Range(0, 3);
            attackPreview.UpdatePreview(attackInd, attackFromBottom);
        }
    }

    internal void setIsTargeted(bool isBeingTargeted)
    {
        if(isBeingTargeted)
        {
            targetedIndicator.SetActive(true);
        }
        else
        {
            targetedIndicator.SetActive(false);
        }
        isTargeted = isBeingTargeted;
    }

    public IEnumerator Attack()
    {

        GridManager.Instance.showAttackPreviewOfEnemy(this);
        GridManager.Instance.showAttackPreviewOfEnemy(this);
        originalPosition = transform.position;
        transform.DOMove(Luggage.Instance.transform.position, GridManager.animTime);
        yield return new WaitForSeconds(GridManager.animTime);



        //attack item
        yield return StartCoroutine( GridManager.Instance.EnemyAttackEnumerator(this));





        GridManager.Instance.clearAttackPreview();
        transform.DOMove(originalPosition, GridManager.animTime);
        yield return new WaitForSeconds(GridManager.animTime);
    }

    private void OnMouseEnter()
    {
        //show attack preview
        GridManager.Instance.showAttackPreviewOfEnemy(this);
    }

    private void OnMouseExit()
    {
        GridManager.Instance.clearAttackPreview();
    }
    private void OnMouseDown()
    {
        EnemyManager.Instance.setCurrentTargetedEnemy(this);
        setIsTargeted(true);
    }
}
