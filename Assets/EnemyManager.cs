using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    List<Enemy> enemies = new List<Enemy>();
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
}
