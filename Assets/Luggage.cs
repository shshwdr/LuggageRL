using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using System;

public class Luggage : Singleton<Luggage>
{
    [Header("Animations")]
    [SerializeField] MMF_Player pushForwardAttackAnimationPlayer;
    [SerializeField] MMF_Player throwOutAndHitBackAttackAnimationPlayer;

    [SerializeField] MMF_Player upsideDownAndDropAttackAnimationPlayer;
    [SerializeField] MMF_Player walkingAnimationPlayer;
    [SerializeField] MMF_Player bagRotatedAnimationPlayer;
    [SerializeField] public Transform luggageRightTargetTransform;

    [SerializeField] MMF_Player returnToIdleAnimationPlayer;


    [SerializeField] MMF_Player hurtAnimationPlayer;
    List<MMF_Player> attackAnimationPlayers = new List<MMF_Player>();
    public Transform idleTransform;

    private static float JUMP_ATTACK_HEIGHT = 2.5F; //how high the luggage will go to ground pound
    public static float BEHIND_ATTACK_DISTANCE = 2.5F; //how far away right luggage will land for attack.

    // Start is called before the first frame update
    void Start()
    {
        attackAnimationPlayers.Add(pushForwardAttackAnimationPlayer);
        attackAnimationPlayers.Add(throwOutAndHitBackAttackAnimationPlayer);
        attackAnimationPlayers.Add(upsideDownAndDropAttackAnimationPlayer);
    }

    // Update is called once per frame  
    void Update()
    {

    }
    Enemy target;


    enum ENEMY_ATTACK_LOCATION { TOP, LEFT, RIGHT };

    void SetTarget() //EnemyManager will handle target
    {
        target = EnemyManager.Instance.GetCurrentTargetedEnemy();
            
        MMF_Player[] animations = GetComponentsInChildren<MMF_Player>();
        //target.ClearDamage()
        foreach (MMF_Player animationPlayerList in animations)
        {
            foreach (MMF_Position feedback in animationPlayerList.GetFeedbacksOfType<MMF_Position>())
            {
                Transform targetTransform;

                if (feedback.Mode == MMF_Position.Modes.ToDestination)
                {
                    switch (feedback.Label)
                    {
                        case "PositionLeftEnemyTarget":
                            feedback.DestinationPositionTransform = target.leftTargetTransform;
                            break;
                        case "PositionRightEnemyTarget":
                            feedback.DestinationPositionTransform = target.rightTargetTransform;
                            break;  
                        case "PositionTopEnemyTarget":
                            feedback.DestinationPositionTransform = target.topTargetTransform;
                            break;
                        case "PositionAboveEnemyTarget": //not used right now
                            targetTransform = target.topTargetTransform;
                            feedback.DestinationPosition = new Vector3(targetTransform.TransformPoint(targetTransform.position).x, JUMP_ATTACK_HEIGHT, 0);
                            break;
                        case "PositionBehindEnemyTarget":
                            targetTransform = target.rightTargetTransform;
                            feedback.DestinationPosition = new Vector3(targetTransform.TransformPoint(targetTransform.position).x + BEHIND_ATTACK_DISTANCE, target.rightTargetTransform.position.y, 0);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public IEnumerator RotateBackToOrigin()
    {
        if (GridManager.Instance.rotatedTime != 0)
        {
            yield return StartCoroutine(Rotate((4 - GridManager.Instance.rotatedTime) % 4));
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

    //public IEnumerator LiftAndDownAttack()
    //{
    //    SetTarget();
        
    //    transform.DOMove(target.transform.position + Vector3.up * 5, GridManager.animTime * 2);
    //    yield return new WaitForSeconds(GridManager.animTime * 2);
    //    transform.DOMove(target.transform.position, GridManager.animTime);
    //    yield return GridManager.Instance.MoveAndAttack(0, -1);
    //    //yield return new WaitForSeconds(GridManager.animTime);

    //    yield return StartCoroutine(showDamage());
    //}
    public IEnumerator PushForwardAttack()
    {
        SetTarget();

        yield return StartCoroutine(pushForwardAttackAnimationPlayer.PlayFeedbacksCoroutine(this.transform.position, 1.0f, false));

        /*//transform.DOMove(target.transform.position, GridManager.animTime);
        yield return GridManager.Instance.MoveAndAttack(1, 0);

        yield return StartCoroutine(showDamage());*/
    }
    public void PushForwardAttackImpact() //use to call through unity events
    {
        //transform.DOMove(target.transform.position, GridManager.animTime);
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_luggage_attack_impact, transform.position);

        StartCoroutine(PushForwardAttackFinish());
    }
    private IEnumerator PushForwardAttackFinish() //perform coroutines
    {
        yield return StartCoroutine(GridManager.Instance.MoveAndAttack(1, 0));
        yield return StartCoroutine(showDamage()); 
        yield return StartCoroutine(BattleManager.Instance. useMove(BattleManager.Instance. attackMoveCost));
    }

    public IEnumerator UpsideDownAndDrop()
    {
        SetTarget();

        GridManager.Instance.Rotate(2, false);

        yield return StartCoroutine(upsideDownAndDropAttackAnimationPlayer.PlayFeedbacksCoroutine(this.transform.position, 1.0f, false));

        
    }
    public void UpsideDownAndDropImpact()
    {
        StartCoroutine(UpsideDownAndDropFinish());
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_luggage_attack_impact, transform.position);
    }

    private IEnumerator UpsideDownAndDropFinish()
    {
        yield return GridManager.Instance.MoveAndAttack(0, -1);

        yield return StartCoroutine(showDamage());
        yield return StartCoroutine(BattleManager.Instance.useMove(BattleManager.Instance.attackMoveCost));
    }

    public IEnumerator ThrowOutAndHitBack()
    {

        SetTarget();

        GridManager.Instance.Rotate(1, false);
        yield return StartCoroutine(throwOutAndHitBackAttackAnimationPlayer.PlayFeedbacksCoroutine(this.transform.position, 1.0f, false));
    }

    public void ThrowOutAndHitBackImpact()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_luggage_attack_impact, transform.position);

        StartCoroutine(ThrowOutAndHitBackFinish());
    }

    private IEnumerator ThrowOutAndHitBackFinish()
    {
        yield return GridManager.Instance.MoveAndAttack(-1, 0);

        yield return StartCoroutine(showDamage());
        yield return StartCoroutine(BattleManager.Instance.useMove(BattleManager.Instance.attackMoveCost));
    }

    IEnumerator showDamage()
    {
        //yield return new WaitForSeconds(GridManager.animTime * 1.1f);
        //yield return StartCoroutine( target.ShowDamage());

        transform.DOMove(transform.parent.position, GridManager.animTime);

        yield return new WaitForSeconds(GridManager.animTime);
    }
    

    public IEnumerator DoDamage(int dam)
    {
        yield return StartCoroutine( target.ApplyDamage(dam));
    }



    internal void StartCharacterWalking()
    {
        if (walkingAnimationPlayer != null)
        {
            walkingAnimationPlayer.PlayFeedbacks();
        }
    }

    internal void StopCharacterWalking()
    {
        if (walkingAnimationPlayer != null)
        {
            walkingAnimationPlayer.StopFeedbacks();
        }
        if (returnToIdleAnimationPlayer != null)
        {
            returnToIdleAnimationPlayer.PlayFeedbacks();
        }

    }

    public void playFootStepSound()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_luggage_walk, this.transform.position);
    }

    internal void playHurtAnimation()
    {
        hurtAnimationPlayer.PlayFeedbacks();
    }

    internal IEnumerator BagRotateAttackReceived()
    {
        yield return StartCoroutine(bagRotatedAnimationPlayer.PlayFeedbacksCoroutine(gameObject.transform.position, 1f, false));
    }
}
