using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : HPObject
{
    EnemyAttackPreview attackPreview;
    public int attack = 3;
    Vector3 originalPosition;

    int attackInd;
    bool attackFromBottom = true;

    // Start is called before the first frame update
    protected  override void Awake()
    {
        hp = maxHP;
        base.Awake();
        EnemyManager.Instance.AddEnemy(this);
        attackPreview = GetComponentInChildren<EnemyAttackPreview>();
    }
    int damage = 0;
    public void GetDamage(int dam)
    {
        damage += dam;
    }
    public void ClearDamage()
    {
        damage = 0;
    }
    protected override void DieInteral()
    {
        base.DieInteral();

        EnemyManager.Instance.RemoveEnemy(this);
        Destroy(gameObject);
    }
    public void ShowDamage()
    {
        ApplyDamage(damage);
    }
    public void SelectAttack()
    {
        if (attackFromBottom)
        {
            attackInd = Random.Range(0, 3);
            attackPreview.UpdatePreview(attackInd, attackFromBottom);
        }
    }
    public IEnumerator Attack()
    {
        originalPosition = transform.position;
        transform.DOMove(Luggage.Instance.transform.position, GridManager.animTime);
        yield return new WaitForSeconds(GridManager.animTime);

        BattleManager.Instance.player.ApplyDamage(attack);

        transform.DOMove(originalPosition, GridManager.animTime);
        yield return new WaitForSeconds(GridManager.animTime);
    }

}
