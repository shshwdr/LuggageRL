using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : Singleton<BattleManager>
{
    public BaseScene baseScene;
    public Text LuggageAttackText;
    public Button LuggageAttackButton;
    public GameObject[] itemsToActivate;
    bool canAttack = true;
    public Text MoveText;
    int selectedAttackIndex;
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
    public void hideButtonCanvas()
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
        //if (GridManager.Instance.CanDraw(out failedReason, count))
        //{
        //    yield return StartCoroutine(GridManager.Instance.DrawItem(count));
        //    if (!noCost)
        //    {
        //        yield return useMove(drawMoveCost);
        //    }
        //}
        //else
        //{

        //    FloatingTextManager.Instance.addText(failedReason, Vector3.zero, Color.red);
        //}

        yield return StartCoroutine(GridManager.Instance.DrawAllItemsFromPool());
        showButtonCanvas();
    }

    public IEnumerator FinishCurrentBattle()
    {
        if (!isBattleFinished)
        {
            isBattleFinished = true;

            yield return new WaitForSeconds(GridManager.animTime);
            FloatingTextManager.Instance.addText("Win Battle!", Vector3.zero, Color.red,1);
            yield return new WaitForSeconds(GridManager.animTime*3);
            //RemoveText();
            //clearTurnData();
            //StartCoroutine(searchNextBattle());
            //hmm make this into a class to control all?
            outControl();
            StageManager.Instance.takeControl();
        }


    }
    //IEnumerator searchNextBattle()
    //{
    //    hideButtonCanvas();
    //    yield return new WaitForSeconds(1);
    //    StartBattle();

    //}

    public void takeControl()
    {
        foreach (var item in itemsToActivate)
        {
            item.SetActive(true);
        }
        ButtonCanvas.gameObject.SetActive(true);
        showButtonCanvas();
        StartBattle();
        //StartCoroutine(test());
    }
    IEnumerator test()
    {
        yield return new WaitForSeconds(0.1f);
        showButtonCanvas();
        StartBattle();
    }
    public void outControl()
    {

        foreach (var item in itemsToActivate)
        {
            item.SetActive(false);
        }
        hideButtonCanvas();
        ButtonCanvas.gameObject.SetActive(false);
        baseScene.hasFinished = true;
    }
    void StartBattle()
    {


        ////clear old enemies, this is bad, hacky solution
        //foreach(var enemy in Transform.FindObjectsOfType<Enemy>(true))
        //{
        //    //if (!enemy.gameObject.activeInHierarchy)
        //    {
        //        Destroy(enemy.gameObject);
        //    }
        //}


        showButtonCanvas();
        isBattleFinished = false;
        canAttack = true;
        UpdateText();
        //AddEnemies();
        DrawItem(true);
        SelectAttack();
        EnemyManager.Instance.SelectEenmiesAttack();

    }
    public void AddEnemies(int numEnemiesToAdd)
    {
        if(numEnemiesToAdd > enemyPositions.Length)
        {
            Debug.LogError("Adding too many enemies to fit the slots");
        }
        for(int x = 0; x < numEnemiesToAdd; x++)
        {
            var enemySlot = enemyPositions[x];
            var pickedEnemy = enemies[Random.Range(0, enemies.Length)];
            var go = Instantiate(pickedEnemy, enemySlot.position, Quaternion.identity, enemySlot);
            go.transform.parent = enemySlot;
            go.transform.localPosition = Vector3.zero;//enemySlot.position;
            break;
        }
    }


    void SelectAttack()
    {
        selectedAttackIndex = Random.Range(0, 3);
        moveLeft = moveMax;
        UpdateText();
        GridManager.Instance.updateAttackEdge();
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
    public IEnumerator MoveTile()
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

        canAttack = false;
        StartCoroutine(PlayerAttackMove(selectedAttackIndex));
    }
    List<int> attackIdToRotationId = new List<int>() { 0, 1, 1, 3 };
    public int getCurrentAttackRotationId()
    {
        return attackIdToRotationId[selectedAttackIndex];
    }
    public IEnumerator PlayerAttackMove(int moveId)
    {
        hideButtonCanvas();
        
        //moveId = 0; //force to push (debug)

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


    public void PredictNextAttack()
    {
        switch (selectedAttackIndex)
        {
            case 0:
                GridManager.Instance.predict(1, 0);
                break;
            case 1:
                GridManager.Instance.predict(0, 1);
                break;
            case 2:
                GridManager.Instance.predict(0, 1);
                break;
            case 3:
                GridManager.Instance.predict(0, -1);
                break;
        }
    }

    public IEnumerator EndOfTurn()
    {
        hideButtonCanvas();

        yield return StartCoroutine(GridManager.Instance.EndTurnCardBehaviorEnumerator());
        
        yield return StartCoroutine(EnemyManager.Instance.EnemiesAttack());
        clearTurnData();

        SelectAttack();
        EnemyManager.Instance.SelectEenmiesAttack();
        yield return StartCoroutine(DrawItemEnumerator(true));
        canAttack = true;
        UpdateText();
        showButtonCanvas();

    }

    void clearTurnData()
    {

        canAttack = true;
        foreach (var item in GridManager.Instance.GridItemDict.Values)
        {
            item.GetComponent<GridItem>().finishedAttack();
        }
        UpdateText();
    }


    void UpdateText()
    {
        if (isBattleFinished)
        {
            return;
        }
        LuggageAttackButton.interactable = canAttack;
        if (canAttack)
        {

            LuggageAttackText.text = $" {attackString[selectedAttackIndex]} ({attackMoveCost})";
        }
        else
        {

            LuggageAttackText.text = $" {attackString[selectedAttackIndex]} (Attacked)";
        }
        MoveText.text = $"{moveLeft}";
    }

    void RemoveText()
    {
        FloatingTextManager.Instance.addText("Search for next Enemy",Vector3.zero,Color.white);
        //LuggageAttackText.text = "Search for next Enemy";u
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
