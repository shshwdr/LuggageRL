using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luggage : Singleton<Luggage>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    Enemy target;
    public void PushForwardAttack()
    {
        Enemy enemy = EnemyManager.Instance.GetFrontEnemy();
        target = enemy;
        target.ClearDamage();
        //
        //suitcaseInBattle.DORotate(new Vector3(0, 0, 90 * rotatedTime), animTime);
        transform.DOMove(enemy.transform.position, GridManager.animTime);
        GridManager.Instance.Move(1, 0);

        StartCoroutine(showDamage());
    }
    IEnumerator showDamage()
    {
        yield return new WaitForSeconds(0.5f);
        target.ShowDamage();

        transform.DOMove(transform.parent.position, GridManager.animTime);
    }
    

    public void DoDamage(int dam)
    {
        target.GetDamage(dam);
    }
}
