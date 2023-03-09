using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    List<Enemy> enemies = new List<Enemy>();
    public Enemy currentTargetedEnemy;
    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }
    public IEnumerator RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        if(enemies.Count == 0)
        {
            yield return StartCoroutine( BattleManager.Instance.FinishCurrentBattle());

        }
    }
    public void SelectEenmiesAttack()
    {
        foreach (var enemy in enemies)
        {
            enemy.SelectAttack();
        }
    }
    public IEnumerator EnemiesAttack()
    {
        foreach(var enemy in enemies)
        {
            yield return StartCoroutine( enemy.Attack());
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void setCurrentTargetedEnemy(Enemy enemy)
    {
        if (currentTargetedEnemy != null)
        {
            currentTargetedEnemy.setIsTargeted(false); //out with the old
        }
        currentTargetedEnemy = enemy; //in with the new
    }
}
