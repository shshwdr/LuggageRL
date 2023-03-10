using Sinbad;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    List<Enemy> enemies = new List<Enemy>();
    public Enemy currentTargetedEnemy;

    public List<Enemy> GetEnemies() => enemies;
    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }
    public IEnumerator RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            yield return StartCoroutine(BattleManager.Instance.FinishCurrentBattle());

        }
        else
        {
            if (enemy == currentTargetedEnemy)
            {
                foreach (var ene in enemies)
                {
                    setCurrentTargetedEnemy(ene);
                }
            }
        }
    }
    public void SelectEenmiesAction()
    {
        foreach (var enemy in enemies)
        {
            enemy.SelectAction();
        }
    }
    public IEnumerator EnemiesAttack()
    {
        foreach(var enemy in enemies)
        {
            enemy.EndOfTurn();
            yield return StartCoroutine( enemy.Core.TakeAction());

        }
    }
    public Enemy GetCurrentTargetedEnemy()
    {
        if (currentTargetedEnemy == null)
        {
            if (enemies.Count > 0)
            {
                currentTargetedEnemy = GetFrontEnemy();
            } else
            {
                Debug.Log("No enemy available to select");
            }
        }
        return currentTargetedEnemy;
    }

    public Enemy GetFrontEnemy()
    {
        return enemies[0];
    }
    public List<EnemyBehavior> GetEnemyEnemyBehaviorsToAdd()
    {
        return new List<EnemyBehavior> {new StealAttackEnemy(),new AttackStealEnemy() };
    }
    public Dictionary<string, EnemyInfo> enemyDict = new Dictionary<string, EnemyInfo>();
    // Start is called before the first frame update
    void Start()
    {

        var itemInfos = CsvUtil.LoadObjects<EnemyInfo>("enemy");
        foreach (var item in itemInfos)
        {
            enemyDict[item.Name] = item;
        }
    }

    public EnemyInfo getEnemyInfo(string itemName)
    {
        if (enemyDict.ContainsKey(itemName))
        {
            return enemyDict[itemName];

        }
        Debug.LogError("no enemy " + itemName);
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            for(int i = 0;i<enemies.Count;i++)
            {
                var enemy = enemies[0];
                StartCoroutine( RemoveEnemy(enemy));
            }
        }
    }
    public void setCurrentTargetedEnemy()
    {
        setCurrentTargetedEnemy(enemies[0]);
    }
    public void setCurrentTargetedEnemy(Enemy enemy)
    {
        foreach(var e in enemies)
        {
            e.setIsTargeted(false);
        }
        enemy.setIsTargeted(true);
        currentTargetedEnemy = enemy; 
    }
}
