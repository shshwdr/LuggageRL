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
            if (GameOver.Instance.isGameOver)
            {
                yield break;
            }

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
        foreach(var e in enemies)
        {
            if (!e.isDead)
            {
                return e;
            }
        }
        return null;
    }
    EnemyInfo GetBossInBiome(BiomeType biome)
    {
        EnemyInfo selected = null;
        if(GameManager.Instance.showedBoss == "")
        {
            selected = enemyDict["BossPinata"];
        }
        else
        {
            var potentials = new List<EnemyInfo>();
            foreach (var info in enemyDict.Values)
            {
                if (info.Type == "boss" && info.canPutInBiome(biome))
                {
                    if (GameManager.Instance.showedBoss == info.Name)
                    {
                        continue;
                    }
                    potentials.Add(info);
                }
            }
            selected = potentials[UnityEngine.Random.Range(0, potentials.Count)];
            enemyDict.Remove(selected.Name);
        }
        
        if (GameManager.Instance.showedBoss == "")
        {
            GameManager.Instance.showedBoss = selected.Name;
        }
        else if (GameManager.Instance.showedBoss == "both")
        {

        }
        else
        {
            GameManager.Instance.showedBoss = "both";
        }
        return selected;
    }
    EnemyInfo GetEliteInBiome(BiomeType biome)
    {
        var potentials = new List<EnemyInfo>();
        foreach (var info in enemyDict.Values)
        {
            if (info.Type == "elite" && info.canPutInBiome(biome))
            {
                if(GameManager.Instance.showedElite == info.Name)
                {
                    continue;
                }
                potentials.Add(info);
            }
        }
        var selected = potentials[UnityEngine.Random.Range(0, potentials.Count)];
        enemyDict.Remove(selected.Name);
        if (GameManager.Instance.showedElite == "")
        {
            GameManager.Instance.showedElite = selected.Name;
        }
        else if (GameManager.Instance.showedElite =="both")
        {

        }
        else
        {
            GameManager.Instance.showedElite = "both";
        }
        return selected;
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
        //return new List<EnemyInfo>() { enemyDict["AttackRotateEnemy"]};
        //return new List<EnemyInfo>() { enemyDict["BossExplode"], enemyDict["SimpleAttackEnemy"], enemyDict["SimpleAttackEnemy"] };
        //return new List<EnemyInfo>() { enemyDict["SimpleAttackEnemy"], enemyDict["AttackFlyEnemy"] };
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
        if (difficultCount > 0)
        {
            Debug.Log("remains difficulty " + difficultCount);
            remainsDiffultCount = difficultCount;
        }


        return res;
    }

    public int remainsDiffultCount = 0;
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
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    for (int i = 0; i < enemies.Count; i++)
        //    {
        //        var enemy = enemies[0];
        //        StartCoroutine( enemy.ApplyDamage(10000));
        //        //StartCoroutine(RemoveEnemy(enemy));
        //    }
        //}


        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    StartCoroutine(BattleManager.Instance.player.HealEnumerator(10000));
        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    StartCoroutine(BattleManager.Instance.player.ApplyDamage(10000));
        //}
    }
    public void setCurrentTargetedEnemy(Enemy enemy)
    {
        foreach (var e in enemies)
        {
            e.setIsTargeted(false);
        }
        if (enemy)
        {
            enemy.setIsTargeted(true);
            currentTargetedEnemy = enemy;
        }
    }
}
