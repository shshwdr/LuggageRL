using Sinbad;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BattleType { normal, elite, boss}

public class EnemyActionInfo
{
    public string Name;
    public string Description;
}
public class EnemyManager : Singleton<EnemyManager>
{
    List<Enemy> enemies = new List<Enemy>();
    public Enemy currentTargetedEnemy;
    public Dictionary<string, EnemyInfo> enemyDict = new Dictionary<string, EnemyInfo>();
    public Dictionary<string, EnemyActionInfo> enemyActionDict = new Dictionary<string, EnemyActionInfo>();

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
        foreach (var enemy in enemies)
        {
            enemy.EndOfTurn();
            yield return StartCoroutine(enemy.Core.TakeAction());

        }
    }
    public Enemy GetCurrentTargetedEnemy()
    {
        if (currentTargetedEnemy == null)
        {
            if (enemies.Count > 0)
            {
                currentTargetedEnemy = GetFrontEnemy();
                setCurrentTargetedEnemy(currentTargetedEnemy);
            }
            else
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
    EnemyInfo GetBossInBiome(BiomeType biome)
    {
        foreach (var info in enemyDict.Values)
        {
            if (info.Type == "boss" && info.canPutInBiome(biome))
            {
                return info;
            }
        }
        return null;
    }
    EnemyInfo GetEliteInBiome(BiomeType biome)
    {
        foreach (var info in enemyDict.Values)
        {
            if (info.Type == "elite" && info.canPutInBiome(biome))
            {
                return info;
            }
        }
        return null;
    }
    List<EnemyInfo> GetEnemiesWithLowerDifficulty(int d, BiomeType biome)
    {
        List<EnemyInfo> res = new List<EnemyInfo>();
        foreach(var info in enemyDict.Values)
        {
            if (info.Difficulty <= d && info.Difficulty>0 && info.canPutInBiome(biome))
            {
                res.Add(info);
            }
        }
        return res;
    }

    public List<EnemyInfo> GetEnemyInfosToAdd(int difficultCount, BattleType battleType, int maxEnemy = 3)
    {

        //return new List<EnemyInfo>() { enemyDict["BossPinata"], enemyDict["BossPinata"], }
        List<EnemyInfo> res = new List<EnemyInfo>();
        if (battleType == BattleType.boss)
        {
            var boss = GetBossInBiome(StageManager.Instance.biomeType);
            res.Add(boss);
        }
        else if(battleType == BattleType.elite)
        {

            var boss = GetEliteInBiome(StageManager.Instance.biomeType);
            res.Add(boss);
        }
        else
        {

        }

        if(difficultCount == 0)
        {
            res.Add(enemyDict["DummyEnemy"]);
            return res;
        }
        var c = res.Count;
        for ( int i =c; i < maxEnemy; i++)
        {

            var potentials = GetEnemiesWithLowerDifficulty(difficultCount, StageManager.Instance.biomeType);
            if(potentials.Count == 0)
            {
                break;
            }
            var picked = potentials[UnityEngine.Random.Range(0, potentials.Count)];
            res.Add(picked);
            difficultCount -= picked.Difficulty;
            if(difficultCount == 0)
            {
                break;
            }
        }


        return res;
    }
    void Start()
    {

        var itemInfos = CsvUtil.LoadObjects<EnemyInfo>("enemy");
        foreach (var item in itemInfos)
        {
            enemyDict[item.Name] = item;
        }
        var itemActionInfos = CsvUtil.LoadObjects<EnemyActionInfo>("enemyAction");
        foreach (var item in itemActionInfos)
        {
            enemyActionDict[item.Name] = item;
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

    public EnemyActionInfo getEnemyActionInfo(string itemName)
    {
        if (enemyActionDict.ContainsKey(itemName))
        {
            return enemyActionDict[itemName];

        }
        Debug.LogError("no enemy " + itemName);
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[0];
                StartCoroutine(RemoveEnemy(enemy));
            }
        }
    }
    public void setCurrentTargetedEnemy(Enemy enemy)
    {
        foreach (var e in enemies)
        {
            e.setIsTargeted(false);
        }
        enemy.setIsTargeted(true);
        currentTargetedEnemy = enemy;
    }
}
