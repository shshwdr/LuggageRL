using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class BattleManager : Singleton<BattleManager>
{
    public Text LuggageAttackText;
    int selected;
    [SerializeField] private int moveMax = 4; 
    int moveLeft;
    bool isBattleFinished = false;
    string[] attackString = new string[] {"Push","Upside Down","Throw And Back" };
    public GameObject[] enemies;
    public Transform[] enemyPositions;
    public Player player;
    [SerializeField] private int drawCount = 2;
    [SerializeField] private int startDrawCount = 4;
    [SerializeField] private int attackMoveCost = 1;

    public void SkipMove()
    {
        moveLeft = 0;

        StartCoroutine(EndOfTurn());
    }
    public void DrawItem(bool noCost = false)
    {
        StartCoroutine(DrawItemEnumerator(noCost));
    }
    public IEnumerator DrawItemEnumerator(bool noCost =false)
    {
        string failedReason;
        int count = noCost ? startDrawCount : drawCount;
        if (GridManager.Instance.CanDraw(out failedReason, count))
        {
            yield return StartCoroutine(GridManager.Instance.DrawItem(count));
            if (!noCost)
            {
                yield return useMove(1);
            }
        }
        else
        {

            FloatingTextManager.Instance.addText(failedReason, Vector3.zero, Color.red);
        }
    }

    public void FinishCurrentBattle()
    {
        if (!isBattleFinished)
        {
            isBattleFinished = true;

            FloatingTextManager.Instance.addText("Win Battle!", Vector3.zero, Color.red);
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
        DrawItem(true);
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
    IEnumerator useMove(int amount)
    {
        moveLeft -= amount;
        UpdateText();
        if (moveLeft == 0)
        {
            yield return StartCoroutine(EndOfTurn());
            //yield return StartCoroutine(PlayerAttackMove());
        }

    }
    public IEnumerator Move()
    {
        yield return useMove(1);
    }

    public void PlayerAttackManually()
    {
        if (moveLeft < 2)
        {
            return;
        }
        StartCoroutine(PlayerAttackMove());
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
        yield return useMove(2);
    }

    public IEnumerator EndOfTurn()
    {
        yield return StartCoroutine(EnemyManager.Instance.EnemiesAttack());
        SelectAttack();
        EnemyManager.Instance.SelectEenmiesAttack();
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

    //----------------EDITOR ACTION BUTTONS---------------

    [Button("Load")]
    public void performLoad()
    {
        //Moves forward, and then draws in one turn
        StartCoroutine(GridManager.Instance.MoveEnumerator(1, 0, false));
        DrawItem(false);
        //StartCoroutine(useMove(attackMoveCost));

    }
    /*[Button("Push")]
    public void performPush() {
        StartCoroutine(Luggage.Instance.PushForwardAttack());
        StartCoroutine(useMove(attackMovePrice));

    }*/
    [Button("Push and Rotate")]
    public void performPushandRotate()
    {
        StartCoroutine(Luggage.Instance.PushAndRotateAttack());
        StartCoroutine(useMove(attackMoveCost));

    }
    [Button("Ground Pound")]
    public void performGroundPound()
    {
        StartCoroutine(Luggage.Instance.UpsideDownAndDrop());
        StartCoroutine(useMove(attackMoveCost));

    }
    [Button("Boomerang")]
    public void performBoomerang()
    {
        StartCoroutine(Luggage.Instance.ThrowOutAndHitBack());
        StartCoroutine(useMove(attackMoveCost));
    }

}
