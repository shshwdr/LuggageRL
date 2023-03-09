using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Luggage : Singleton<Luggage>
{

    [SerializeField] MoreMountains.Feedbacks.MMF_Player pushForwardAttackAnimationPlayer;
    public Transform idleTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    Enemy target;

    void SetTarget() //EnemyManager will handle target
    {
        target = EnemyManager.Instance.GetCurrentTargetedEnemy();
        //target.ClearDamage()
        foreach(MMF_Position feedback in pushForwardAttackAnimationPlayer.GetFeedbacksOfType<MMF_Position>())
        {
            if(feedback.Mode == MMF_Position.Modes.ToDestination)
            {
                if (feedback.DestinationPositionTransform != idleTransform) {
                    feedback.DestinationPositionTransform = target.leftTargetTransform;
                }
            }
        }
    }

    public IEnumerator Rotate(int ind)
    {
        GridManager.Instance.Rotate(ind, false);
        transform.DORotate(new Vector3(0, 0, 90 * GridManager.Instance.rotatedTime), GridManager.animTime * 2);
        yield return new WaitForSeconds(GridManager.animTime * 2);
        yield return StartCoroutine(GridManager.Instance.MoveAfter(0, -1));
    }

    public IEnumerator MoveForward()
    {
        yield return GridManager.Instance.MoveEnumerator(1, 0, false);
    }

    public IEnumerator LiftAndDownAttack()
    {
        SetTarget();
        //
        
        transform.DOMove(target.transform.position + Vector3.up * 5, GridManager.animTime * 2);
        yield return new WaitForSeconds(GridManager.animTime * 2);
        //suitcaseInBattle.DORotate(new Vector3(0, 0, 90 * rotatedTime), animTime);
        transform.DOMove(target.transform.position, GridManager.animTime);
        yield return GridManager.Instance.MoveAndAttack(0, -1);
        //yield return new WaitForSeconds(GridManager.animTime);

        yield return StartCoroutine(showDamage());
    }
    public IEnumerator PushForwardAttack()
    {
        SetTarget();

        yield return StartCoroutine(pushForwardAttackAnimationPlayer.PlayFeedbacksCoroutine(this.transform.position, 1.0f, false));
        
        //transform.DOMove(target.transform.position, GridManager.animTime);
        yield return GridManager.Instance.MoveAndAttack(1, 0);

        yield return StartCoroutine(showDamage());
    }

    public IEnumerator UpsideDownAndDrop()
    {
        SetTarget();

        GridManager.Instance.Rotate(2, false);
        transform.DOMove(target.transform.position + Vector3.up*5, GridManager.animTime*2);
        transform.DORotate(new Vector3(0, 0, 90 * GridManager.Instance.rotatedTime), GridManager.animTime * 2);
        yield return new WaitForSeconds(GridManager.animTime * 2);

        transform.DOMove(target.transform.position, GridManager.animTime);
        //GridManager.Instance.Move(0, -1); ;

        yield return GridManager.Instance.MoveAndAttack(0, -1);
        //yield return new WaitForSeconds(GridManager.animTime);

        yield return StartCoroutine(showDamage());
    }
    public IEnumerator ThrowOutAndHitBack()
    {

        SetTarget();


        GridManager.Instance.Rotate(1, false);
        transform.DORotate(new Vector3(0, 0, 90 * GridManager.Instance.rotatedTime), GridManager.animTime * 2);
        transform.DOMove(target.transform.position + Vector3.up * 5, GridManager.animTime );
        yield return new WaitForSeconds(GridManager.animTime);
        transform.DOMove(target.transform.position + Vector3.right * 2, GridManager.animTime);
        yield return new WaitForSeconds(GridManager.animTime );

        transform.DOMove(target.transform.position, GridManager.animTime);
        //GridManager.Instance.Move(-1, 0);
        yield return GridManager.Instance.MoveAndAttack(-1,0);
       // yield return new WaitForSeconds(GridManager.animTime);

        yield return StartCoroutine(showDamage());
    }
    IEnumerator showDamage()
    {
        //yield return new WaitForSeconds(GridManager.animTime * 1.1f);
        yield return StartCoroutine( target.ShowDamage());

        transform.DOMove(transform.parent.position, GridManager.animTime);

        yield return new WaitForSeconds(GridManager.animTime);
    }
    

    public void DoDamage(int dam)
    {
        target.GetDamage(dam);
    }
}
