using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EnemyInfo
{
    public string Name;
    public string DisplayName;
    public string Description;
    public string Type;
    public int Difficulty;
    public int Biome;
    public int HP;
}
public class Enemy : HPObject
{
    public EnemyAttackPreview attackPreview;

    public int attack = 3;
    Vector3 originalPosition;
    public Transform leftTargetTransform; //where player should impact
    public Transform topTargetTransform;
    public Transform rightTargetTransform;
    public Collider2D enemyCollider;
    public GameObject targetedIndicator;

    public int attackInd;
    public bool attackFromBottom = true;
    public EnemyInfo info;

    public string DisplayName => info.DisplayName;
    public virtual string Desc => info.Description;

    public EnemyBehavior Core;

    public void Init(EnemyBehavior core)
    {
        Core = core;
        core.enemy = this;
        info = EnemyManager.Instance.getEnemyInfo(Core.Name);
        maxHP = info.HP;
        hp = maxHP;
        base.Awake();
        EnemyManager.Instance.AddEnemy(this);
        attackPreview = GetComponentInChildren<EnemyAttackPreview>();
        hpbar.updateHPBar(hp, maxHP);
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
        yield return new WaitForSeconds(0.3f);
        //yield return  StartCoroutine( ApplyDamage(damage));
    }


    public void SelectAction()
    {


        Core.SelectAction();

        //if (attackFromBottom)
        //{
        //    attackInd = Random.Range(0, 3);
        //    attackPreview.UpdatePreview(attackInd, attackFromBottom);
        //}
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

        yield return new WaitForSeconds(GridManager.animTime);
        //GridManager.Instance.showAttackPreviewOfEnemy(this);

        //originalPosition = transform.position;
        //transform.DOMove(Luggage.Instance.transform.position, GridManager.animTime);
        //yield return new WaitForSeconds(GridManager.animTime);



        ////attack item
        //yield return StartCoroutine( GridManager.Instance.EnemyAttackEnumerator(this));


        //GridManager.Instance.clearAttackPreview();
        //transform.DOMove(originalPosition, GridManager.animTime);
        //yield return new WaitForSeconds(GridManager.animTime);
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
