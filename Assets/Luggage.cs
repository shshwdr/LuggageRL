using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Luggage : Singleton<Luggage>
{

    [SerializeField] MMF_Player pushForwardAttackAnimationPlayer;
    [SerializeField] MMF_Player throwOutAndHitBackAttackAnimationPlayer;
    [SerializeField] MMF_Player upsideDownAndDropAttackAnimationPlayer;
    List<MMF_Player> animationPlayers = new List<MMF_Player>();
    public Transform idleTransform;

    // Start is called before the first frame update
    void Start()
    {
        animationPlayers.Add(pushForwardAttackAnimationPlayer);
        animationPlayers.Add(throwOutAndHitBackAttackAnimationPlayer);
        animationPlayers.Add(upsideDownAndDropAttackAnimationPlayer);
    }

    // Update is called once per frame  
    void Update()
    {

    }
    Enemy target;

    private static float JUMP_ATTACK_HEIGHT = 2.5F; //how high the luggage will go to ground pound
    public static float BEHIND_ATTACK_DISTANCE = 2.5F; //how far away right luggage will land for attack.

    enum ENEMY_ATTACK_LOCATION { TOP, LEFT, RIGHT };

    void SetTarget() //EnemyManager will handle target
    {
        target = EnemyManager.Instance.GetCurrentTargetedEnemy();
        //target.ClearDamage()
        foreach (MMF_Player animationPlayerList in animationPlayers)
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

        yield return StartCoroutine(upsideDownAndDropAttackAnimationPlayer.PlayFeedbacksCoroutine(this.transform.position, 1.0f, false));

        /*transform.DOMove(target.transform.position + Vector3.up*5, GridManager.animTime*2);
        transform.DORotate(new Vector3(0, 0, 90 * GridManager.Instance.rotatedTime), GridManager.animTime * 2);
        yield return new WaitForSeconds(GridManager.animTime * 2);

        transform.DOMove(target.transform.position, GridManager.animTime);
        //GridManager.Instance.Move(0, -1); ;
*/
        yield return GridManager.Instance.MoveAndAttack(0, -1);
        //yield return new WaitForSeconds(GridManager.animTime);

        yield return StartCoroutine(showDamage());
    }
    public IEnumerator ThrowOutAndHitBack()
    {

        SetTarget();


        GridManager.Instance.Rotate(1, false);
        yield return StartCoroutine(throwOutAndHitBackAttackAnimationPlayer.PlayFeedbacksCoroutine(this.transform.position, 1.0f, false));

        /*transform.DORotate(new Vector3(0, 0, 90 * GridManager.Instance.rotatedTime), GridManager.animTime * 2);
        transform.DOMove(target.transform.position + Vector3.up * 5, GridManager.animTime );
        yield return new WaitForSeconds(GridManager.animTime);
        transform.DOMove(target.transform.position + Vector3.right * 2, GridManager.animTime);
        yield return new WaitForSeconds(GridManager.animTime );

        transform.DOMove(target.transform.position, GridManager.animTime);
        //GridManager.Instance.Move(-1, 0);*/
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
