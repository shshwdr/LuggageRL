using DG.Tweening;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EnemyInfo
{
    public string Name;
    public string DisplayName;
    public string Description;
    public string Type;
    public int Difficulty;
    public string BiomeString;
    public int IsLarge;
    public int BasicAttack;
    public BiomeType BiomeType{get{
            Debug.Log("biome " +Name+" "+ BiomeString);
            return (BiomeType) System.Enum.Parse(typeof(BiomeType), BiomeString); }}
    public bool canPutInBiome(BiomeType type) => BiomeType == type|| BiomeType == BiomeType.none;
    public int HP;
}
public class Enemy : HPObject
{
    [SerializeField] MMF_Player flyerAttackAnimationPlayer;
    [SerializeField] MMF_Player simpleAttackAnimationPlayer;
    [SerializeField] MMF_Player hurtAnimationPlayer;
    [SerializeField] MMF_Player rotateAttackAnimationPlayer;
    //List<MMF_Player> animations = new List<MMF_Player>();

    public EnemyAttackPreview attackPreview;
    public SpriteRenderer enemyRender;
    public int attack => Core.currentAction is EnemyActionAttack attackAction ?(int)( (attackAction.attackDamage + info.BasicAttack) * (1+EnemyManager.Instance.remainsDiffultCount*0.05f)) : 0;
    public int defense => Core.currentAction is EnemyActionShield attackAction ? (int)((attackAction.shieldAmount + info.BasicAttack) * (1 + EnemyManager.Instance.remainsDiffultCount * 0.05f)) : 0;
    public int heal => Core.currentAction is EnemyActionHeal heal ? (int)((heal.healAmount + info.BasicAttack) * (1 + EnemyManager.Instance.remainsDiffultCount * 0.05f)) : 0;
    public Transform idlePosition;
    public Transform leftTargetTransform; //where player should impact
    public Transform topTargetTransform;
    public Transform rightTargetTransform;
    public GameObject targetedIndicator;

    public int attackInd => Core.currentAction is EnemyActionAttack attackAction ? attackAction.attackHeight : 0;
    public int attackRange => Core.currentAction is EnemyActionAttack attackAction ? attackAction.attackRange : 0;
    public int attackRangeV => Core.currentAction is EnemyActionAttack attackAction ? attackAction.attackRangeVerticle : 0;
    public bool attackFromBottom =>Core.currentAction is EnemyActionAttack attackAction?attackAction.attackFromBottom:false;
    public EnemyInfo info;

    public string DisplayName => info.DisplayName;
    public virtual string Desc => info.Description;

    public EnemyBehavior Core;

    public GameObject shieldObj;
    public Text shieldText;



    public override IEnumerator ApplyDamage(int damage, bool isFinalAttack = true)
    {

        if(Core is EliteCap)
        {
            if (damage > 5)
            {
                damage = 5;
                FloatingTextManager.Instance.addText("Cap Damage!", transform.position, Color.yellow);
                yield return new WaitForSeconds(GridManager.animTime / 2);
            }
        }


        yield return StartCoroutine( base.ApplyDamage(damage, isFinalAttack));


        if (Core is EliteThorn)
        {
            FloatingTextManager.Instance.addText("Thorn!", transform.position, Color.yellow);
            yield return StartCoroutine(BattleManager.Instance.player.ApplyDamage(1));
        }
    }
    public void EndOfTurn()
    {
        //clear shiild 
        shield = 0;
        updateShield();
    }
    public override IEnumerator ShieldBeAttacked(int amount)
    {
        shield -= amount;
        attackPreview.transform.DOShakeScale(GridManager.animTime);
        yield return new WaitForSeconds(GridManager.animTime);

        updateShield();
    }
    public IEnumerator AddShield(int amount)
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_enemy_action_shield, transform.position);
        attackPreview.transform.DOShakeScale(GridManager.animTime);
        yield return new WaitForSeconds(GridManager.animTime);

        shield += amount;

        updateShield();
    }
    void updateShield()
    {
        if(shield == 0)
        {
            shieldObj.SetActive(false);
        }
        else
        {

            shieldObj.SetActive(true);
            shieldText.text = shield.ToString();
        }
    }

    public IEnumerator AddItem(ItemType type, int amount)
    {
        attackPreview.transform.DOShakeScale(GridManager.animTime * 2);
        yield return new WaitForSeconds(GridManager.animTime * 2);

        for(int i = 0; i < amount; i++)
        {

            var obj = GridManager.Instance.AddItemRandomPosition(type);
            if (obj == null)
            {

                //FloatingTextManager.Instance.addText("Bag is Full!", transform.position, Color.yellow);
                yield break;
            }
            else
            {
                var originPosition = obj.transform.position;
                obj.transform.position = transform.position;

                obj.transform.DOMove(originPosition, GridManager.animTime);


            }
        }
        yield return new WaitForSeconds(GridManager.animTime);


    }
    public IEnumerator StealItem(ItemType type)
    {


        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_enemy_ability_steal, transform.position);
        attackPreview.transform.DOShakeScale(GridManager.animTime * 2);
        yield return new WaitForSeconds(GridManager.animTime * 2);



        var items = GridManager.Instance.GetItemOfType(type);

        foreach (var item in items)
        {
            GridManager.Instance.RemoveGrid(item);
            item.transform.DOMove(transform.position, GridManager.animTime);

        }
        if (items.Count > 0)
        {
            yield return new WaitForSeconds(GridManager.animTime * 2);
        }
        foreach (var item in items)
        {
            Destroy(item.gameObject);
        }
    }
    public float HPRatio()
    {
        return (float)hp / (float)maxHP;
    }
    public IEnumerator HealMinHP(int healAmount)
    {
        float minHPRatio = 1;
        Enemy healTarget = this;
        //find enemy with lowest hp
        foreach(var enemy in EnemyManager.Instance.GetEnemies())
        {
            var ene = enemy.GetComponent<Enemy>();
            if (ene.HPRatio() < minHPRatio)
            {
                minHPRatio = ene.HPRatio();
                healTarget = ene;
            }
        }

        yield return StartCoroutine(healTarget.HealEnumerator(healAmount));

        attackPreview.transform.DOShakeScale(GridManager.animTime*2);
        yield return new WaitForSeconds(GridManager.animTime*2);
    }

    public void Init(EnemyInfo _info)
    {
        Debug.Log("select " + info.Name);
        if(_info.Name == "dummy")
        {
            Debug.LogError("??");
        }
        Core = (EnemyBehavior)System.Activator.CreateInstance(System.Type.GetType(_info.Name.ToString()));
        Core.enemy = this;
        info = _info;
        maxHP = (int)(info.HP * (1 + EnemyManager.Instance.remainsDiffultCount * 0.05f));
        hp = maxHP;
        base.Awake();
        EnemyManager.Instance.AddEnemy(this);
        attackPreview = GetComponentInChildren<EnemyAttackPreview>();
        InitiateAnimations();
        hpbar.updateHPBar(hp, maxHP);
        var prefab = Resources.Load<Sprite>("enemies/" + info.Name);
        if(prefab == null)
        {
            Debug.LogWarning("no enemy image "+ info.Name);
        }
        else
        {

            enemyRender.sprite = prefab;
        }
    }

    private void InitiateAnimations()
    {
        //animations.Add(flyerAttackAnimationPlayer);
        //animations.Add(simpleAttackAnimationPlayer);
        MMF_Player[] animations = GetComponentsInChildren<MMF_Player>();

        foreach (MMF_Player animationPlayerList in animations)
        {
            foreach (MMF_Position feedback in animationPlayerList.GetFeedbacksOfType<MMF_Position>())
            {
                if (feedback.Mode == MMF_Position.Modes.ToDestination)
                {
                    switch (feedback.Label)
                    {
                        case "PositionRightLuggageTarget":
                            feedback.DestinationPositionTransform = Luggage.Instance.luggageRightTargetTransform;
                            break;
                        case "PositionToIdle":
                            feedback.DestinationPositionTransform = idlePosition;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }                  

    int damage = 0;
    [SerializeField] private bool isTargeted;

    //public void GetDamage(int dam)
    //{
    //    damage += dam;
    //}
    //public void ClearDamage()
    //{
    //    damage = 0;
    //}
    protected override IEnumerator DieInteral()
    {
        targetedIndicator.SetActive(false);
        yield return StartCoroutine( base.DieInteral());
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_enemy_death, transform.position);
        transform.DOShakeScale(GridManager.animTime);
        yield return new WaitForSeconds(GridManager.animTime);
        transform.DOLocalMoveY(-10, GridManager.animTime);
        //Destroy(gameObject,GridManager.animTime);
        yield return StartCoroutine(EnemyManager.Instance.RemoveEnemy(this));
        EnemyManager.Instance.setCurrentTargetedEnemy(EnemyManager.Instance.GetFrontEnemy());
        Luggage.Instance. target = EnemyManager.Instance.GetCurrentTargetedEnemy();
        GridManager.Instance.showAllAttackPreview();
    }
    //public IEnumerator ShowDamage()
    //{
    //    yield return new WaitForSeconds(0.3f);
    //    yield return  StartCoroutine( ApplyDamage(damage));
    //}


    public void SelectAction()
    {
        Core.SelectAction();
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
    public IEnumerator RotateBag()
    {

        yield return StartCoroutine(rotateAttackAnimationPlayer.PlayFeedbacksCoroutine(gameObject.transform.position, 1f, false));
    }
    public void RotateBagImpact()
    {
        StartCoroutine(RotateBagFinish());
    }

    private IEnumerator RotateBagFinish()
    {
        GridManager.Instance.Rotate(1, false);
        yield return Luggage.Instance.BagRotateAttackReceived();
    }
    bool finishedAttack = false;
    public IEnumerator Attack()
    {
        GridManager.Instance.cleanAndShowAttackPreviewOfEnemy(this);
        finishedAttack = false;
        StartCoroutine(simpleAttackAnimationPlayer.PlayFeedbacksCoroutine(gameObject.transform.position, 1f, false));
        yield return new WaitUntil(() => finishedAttack);
        finishedAttack = false;

        /* originalPosition = transform.position;
         transform.DOMove(Luggage.Instance.transform.position, GridManager.animTime);
         yield return new WaitForSeconds(GridManager.animTime);
 */
        //attack item
        //yield return StartCoroutine(GridManager.Instance.EnemyAttackEnumerator(this));

        //GridManager.Instance.clearAttackPreview();


        /*
        transform.DOMove(originalPosition, GridManager.animTime);
        yield return new WaitForSeconds(GridManager.animTime);*/


    }

    public void AttackImpact()
    {
        StartCoroutine(AttackFinish());
    }
    public IEnumerator AttackFinish()
    {

        if (info.IsLarge == 1)
        {

            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_enemy_heavy_attack, transform.position);
        }
        else
        {

            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_enemy_light_attack, transform.position);
        }
        //attack item
        yield return StartCoroutine(GridManager.Instance.EnemyAttackEnumerator(this));

        GridManager.Instance.clearAttackPreview();

        finishedAttack = true;
        /*
        transform.DOMove(originalPosition, GridManager.animTime);
        yield return new WaitForSeconds(GridManager.animTime);*/
    }

    public override void reactToDamage()
    {
        hurtAnimationPlayer.Initialization(); //don't know why, this one seems to need this. 
        hurtAnimationPlayer.PlayFeedbacks();
    }


    private void OnMouseEnter()
    {
        //show attack preview
        GridManager.Instance.cleanAndShowAttackPreviewOfEnemy(this);

        DetailView.Instance.UpdateValue(this);
    }

    private void OnMouseExit()
    {
        GridManager.Instance.showAllAttackPreview();
        DetailView.Instance.UpdateValue(null);
    }
    private void OnMouseDown()
    {
        if (BattleManager.Instance.CanPlayerControl)
        {
            EnemyManager.Instance.setCurrentTargetedEnemy(this);
        }
    }
}
