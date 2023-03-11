using DG.Tweening;
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
    public BiomeType BiomeType{get{
            Debug.Log("biome " +Name+" "+ BiomeString);
            return (BiomeType) System.Enum.Parse(typeof(BiomeType), BiomeString); }}
    public bool canPutInBiome(BiomeType type) => BiomeType == type|| BiomeType == BiomeType.none;
    public int HP;
}
public class Enemy : HPObject
{
    public EnemyAttackPreview attackPreview;
    public SpriteRenderer enemyRender;
    public int attack = 3;
    Vector3 originalPosition;
    public Transform leftTargetTransform; //where player should impact
    public Transform topTargetTransform;
    public Transform rightTargetTransform;
    public GameObject targetedIndicator;

    public int attackInd;
    public bool attackFromBottom = true;
    public EnemyInfo info;

    public string DisplayName => info.DisplayName;
    public virtual string Desc => info.Description;

    public EnemyBehavior Core;

    public GameObject shieldObj;
    public Text shieldText;


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

    public IEnumerator AddItem(ItemType type)
    {
        attackPreview.transform.DOShakeScale(GridManager.animTime * 2);
        yield return new WaitForSeconds(GridManager.animTime * 2);

        var obj = GridManager.Instance.AddItemRandomPosition(type);

        if(obj == null)
        {

            //FloatingTextManager.Instance.addText("Bag is Full!", transform.position, Color.yellow);
            yield return new WaitForSeconds(GridManager.animTime);
        }
        else
        {
            var originPosition = obj.transform.position;
            obj.transform.position = transform.position;

            obj.transform.DOMove(originPosition, GridManager.animTime);
            yield return new WaitForSeconds(GridManager.animTime);


        }

    }
    public IEnumerator StealItem(ItemType type)
    {


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
        Core = (EnemyBehavior)System.Activator.CreateInstance(System.Type.GetType(_info.Name.ToString()));
        Core.enemy = this;
        info = _info;
        maxHP = info.HP;
        hp = maxHP;
        base.Awake();
        EnemyManager.Instance.AddEnemy(this);
        attackPreview = GetComponentInChildren<EnemyAttackPreview>();
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

    int damage = 0;
    [SerializeField] private bool isTargeted;

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
        yield return  StartCoroutine( ApplyDamage(damage));
    }


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
    public IEnumerator Attack()
    {

        GridManager.Instance.showAttackPreviewOfEnemy(this);

        originalPosition = transform.position;
        transform.DOMove(Luggage.Instance.transform.position, GridManager.animTime);
        yield return new WaitForSeconds(GridManager.animTime);



        //attack item
        yield return StartCoroutine(GridManager.Instance.EnemyAttackEnumerator(this));


        GridManager.Instance.clearAttackPreview();
        transform.DOMove(originalPosition, GridManager.animTime);
        yield return new WaitForSeconds(GridManager.animTime);
    }

    private void OnMouseEnter()
    {
        //show attack preview
        GridManager.Instance.showAttackPreviewOfEnemy(this);

        DetailView.Instance.UpdateValue(this);
    }

    private void OnMouseExit()
    {
        GridManager.Instance.clearAttackPreview();
        DetailView.Instance.UpdateValue(null);
    }
    private void OnMouseDown()
    {
        EnemyManager.Instance.setCurrentTargetedEnemy(this);
    }
}
