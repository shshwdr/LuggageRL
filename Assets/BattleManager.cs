using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] private int rotateMoveCost = 0;
    [SerializeField] private int attackMoveCost = 0;
    [SerializeField] private int swapActionCost = 0;
    [SerializeField] private int drawMoveCost = 0;
    public Transform ButtonCanvas;

    void hideButtonCanvas()
    {
        foreach (var button in ButtonCanvas.GetComponentsInChildren<Button>())
        {
            button.gameObject.SetActive(false);
        }
    }
    void showButtonCanvas()
    {
        if (isBattleFinished)
        {
            return;
        }
        foreach (var button in ButtonCanvas.GetComponentsInChildren<Button>(true))
        {
            button.gameObject.SetActive(true);
        }
    }
    public void SkipMove()
    {
        moveLeft = 0;

        StartCoroutine(EndOfTurn());
    }
    public void DrawItem(bool noCost = false)
    {
        if (!noCost && moveLeft < drawMoveCost)
        {
            return;
        }
        StartCoroutine(DrawItemEnumerator(noCost));
    }
    public IEnumerator DrawItemEnumerator(bool noCost =false)
    {
        hideButtonCanvas();
        string failedReason;
        int count = noCost ? startDrawCount : drawCount;
        if (GridManager.Instance.CanDraw(out failedReason, count))
        {
            yield return StartCoroutine(GridManager.Instance.DrawItem(count));
            if (!noCost)
            {
                yield return useMove(drawMoveCost);
            }
        }
        else
        {

            FloatingTextManager.Instance.addText(failedReason, Vector3.zero, Color.red);
        }
        showButtonCanvas();
    }

    public IEnumerator FinishCurrentBattle()
    {
        if (!isBattleFinished)
        {
            isBattleFinished = true;
            FloatingTextManager.Instance.addText("Win Battle!", Vector3.zero, Color.red);
            yield return new WaitForSeconds(GridManager.animTime);
            //reward
            RemoveText();
            StartCoroutine(searchNextBattle());
        }


    }
    IEnumerator searchNextBattle()
    {
        hideButtonCanvas();
        yield return new WaitForSeconds(1);
        StartBattle();

    }

    void StartBattle()
    {
        //clear old enemies, this is bad, hacky solution
        foreach(var enemy in Transform.FindObjectsOfType<Enemy>(true))
        {
            //if (!enemy.gameObject.activeInHierarchy)
            {
                Destroy(enemy.gameObject);
            }
        }


        showButtonCanvas();
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
        hideButtonCanvas();
        yield return useMove(swapActionCost);
        showButtonCanvas();
    }

    public void Rotate(int i)
    {

        StartCoroutine(RotateIEnumerator(i));
    }
    public IEnumerator RotateIEnumerator(int i)
    {

        hideButtonCanvas();
        yield return StartCoroutine(Luggage.Instance.Rotate(i));
        yield return useMove(rotateMoveCost);

        showButtonCanvas();
    }
    public void MoveForward(int i)
    {
        hideButtonCanvas();

        StartCoroutine(MoveForwardIEnumerator());
    }
    public IEnumerator MoveForwardIEnumerator()
    {

        hideButtonCanvas();
        yield return StartCoroutine(Luggage.Instance.MoveForward());
        yield return useMove(rotateMoveCost);

        showButtonCanvas();
    }

    public void PlayerAttackManuallySelectId(int i)
    {

        if (moveLeft < attackMoveCost)
        {
            return;
        }
        StartCoroutine(PlayerAttackMove(i));
    }
    public void PlayerAttackManually()
    {
        if (moveLeft < attackMoveCost)
        {
            return;
        }
        StartCoroutine(PlayerAttackMove(selected));
    }

    public IEnumerator PlayerAttackMove(int moveId)
    {
        hideButtonCanvas();
        switch (moveId)
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
            case 3:
                yield return StartCoroutine(Luggage.Instance.LiftAndDownAttack());
                break;
        }
        yield return useMove(attackMoveCost);

        showButtonCanvas();
    }

    public IEnumerator EndOfTurn()
    {
        hideButtonCanvas();
        yield return StartCoroutine(EnemyManager.Instance.EnemiesAttack());
        SelectAttack();
        EnemyManager.Instance.SelectEenmiesAttack();
        showButtonCanvas();

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
