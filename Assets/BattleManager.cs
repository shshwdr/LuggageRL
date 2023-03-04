using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : Singleton<BattleManager>
{
    public Text LuggageAttackText;
    int selected;
    int moveMax = 2;
    int moveLeft;
    bool isBattleFinished = false;
    string[] attackString = new string[] {"Push","Upside Down","Throw And Back" };
    public GameObject[] enemies;
    public Transform[] enemyPositions;
    public Player player;

    public void SkipMove()
    {
        moveLeft = 0;

         StartCoroutine(PlayerAttackMove());
    }
    public void FinishCurrentBattle()
    {
        if (!isBattleFinished)
        {
            isBattleFinished = true;

            FloatingTextManager.Instance.addText("Win Battle!", Vector3.zero);
            //reward
            RemoveText();
            StartCoroutine(searchNextBattle());
        }


    }
    IEnumerator searchNextBattle()
    {
        yield return new WaitForSeconds(1);
        StartBattle();

    }

    void StartBattle()
    {
        isBattleFinished = false;
        AddEnemies();
        SelectAttack();
        EnemyManager.Instance.SelectEenmiesAttack();

    }
    void AddEnemies()
    {
        var pickedEnemy = enemies[Random.Range(0, enemies.Length)];
        var go = Instantiate(pickedEnemy);
        go.transform.position =  enemyPositions[0].position;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine( searchNextBattle());
    }

    void SelectAttack()
    {
        selected = Random.Range(0, 3);
        moveLeft = moveMax;
        UpdateText();
    }

    public IEnumerator Move()
    {
        moveLeft -= 1;
        UpdateText();
        if(moveLeft == 0)
        {
            yield return StartCoroutine(PlayerAttackMove());
        }
    }

    public IEnumerator PlayerAttackMove()
    {
        switch (selected)
        {
            case 0:
                yield return StartCoroutine(Luggage.Instance.PushForwardAttack());
                break;
            case 1:
                yield return StartCoroutine(Luggage.Instance.UpsideDownAndDrop());
                break;
            case 2:
                yield return StartCoroutine(Luggage.Instance.ThrowOutAndHitBack());
                break;
        }

        yield return StartCoroutine(EnemyManager.Instance.EnemiesAttack());
        SelectAttack();
        EnemyManager.Instance. SelectEenmiesAttack();
    }

    void UpdateText()
    {
        if (isBattleFinished)
        {
            return;
        }
        LuggageAttackText.text = $"Next Attack: {attackString[selected]} (in {moveLeft} moves)";
    }

    void RemoveText()
    {
        LuggageAttackText.text = "Search for next Enemy";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
