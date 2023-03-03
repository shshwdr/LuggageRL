using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : HPObject
{
    public int attack = 3;
    Vector3 originalPosition;
    // Start is called before the first frame update
    protected  override void Start()
    {
        hp = maxHP;
        base.Start();
        EnemyManager.Instance.AddEnemy(this);
        originalPosition = transform.position;
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

    public IEnumerator Attack()
    {
        transform.DOMove(Luggage.Instance.transform.position, GridManager.animTime);
        yield return new WaitForSeconds(GridManager.animTime);

        BattleManager.Instance.player.ApplyDamage(attack);

        transform.DOMove(originalPosition, GridManager.animTime);
        yield return new WaitForSeconds(GridManager.animTime);
    }

}
