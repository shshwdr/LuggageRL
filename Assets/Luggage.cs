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

    void SelectEnemyTarget()
    {

        Enemy enemy = EnemyManager.Instance.GetFrontEnemy();
        target = enemy;
        target.ClearDamage();
    }
    public IEnumerator PushForwardAttack()
    {
        SelectEnemyTarget();
        //
        //suitcaseInBattle.DORotate(new Vector3(0, 0, 90 * rotatedTime), animTime);
        transform.DOMove(target.transform.position, GridManager.animTime);
        GridManager.Instance.Move(1, 0);
        yield return new WaitForSeconds(GridManager.animTime);

        StartCoroutine(showDamage());
    }

    public IEnumerator UpsideDownAndDrop()
    {
        SelectEnemyTarget();

        GridManager.Instance.Rotate(2, false);
        transform.DOMove(target.transform.position + Vector3.up*5, GridManager.animTime*2);
        transform.DORotate(new Vector3(0, 0, 90 * GridManager.Instance.rotatedTime), GridManager.animTime * 2);
        yield return new WaitForSeconds(GridManager.animTime * 2);

        transform.DOMove(target.transform.position, GridManager.animTime);
        GridManager.Instance.Move(0, -1); ;
        yield return new WaitForSeconds(GridManager.animTime);

        StartCoroutine(showDamage());
    }
    public IEnumerator ThrowOutAndHitBack()
    {

        SelectEnemyTarget();


        GridManager.Instance.Rotate(1, false);
        transform.DORotate(new Vector3(0, 0, 90 * GridManager.Instance.rotatedTime), GridManager.animTime * 2);
        transform.DOMove(target.transform.position + Vector3.up * 5, GridManager.animTime );
        yield return new WaitForSeconds(GridManager.animTime);
        transform.DOMove(target.transform.position + Vector3.right * 2, GridManager.animTime);
        yield return new WaitForSeconds(GridManager.animTime );

        transform.DOMove(target.transform.position, GridManager.animTime);
        GridManager.Instance.Move(-1, 0);
        yield return new WaitForSeconds(GridManager.animTime);

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
