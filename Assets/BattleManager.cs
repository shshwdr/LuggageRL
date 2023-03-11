using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : Singleton<BattleManager>
{
    public bool isInControl;
    public BaseScene baseScene;
    public Text LuggageAttackText;
    public Button LuggageAttackButton;
    public GameObject[] itemsToActivate;
    bool CanAttack => attackCountUsed<attackCount;
    int attackCount => 1 +LuggageManager.Instance.UpgradedTime[UpgradeType.attackCount];
    int attackCountUsed = 0;
    public Text MoveText;
    int selectedAttackIndex;
    [SerializeField] private int moveMax = 4;
    int moveLeft;
    bool isBattleFinished = false;
    string[] attackString = new string[] {"Push","Upside Down","Throw And Back" };
    public GameObject enemyPrefab;
    public Transform[] enemyPositions;
    public Player player;
    [SerializeField] private int rotateMoveCost = 0;
    [SerializeField] private int attackMoveCost = 0;
    [SerializeField] private int swapActionCost = 0;
    [SerializeField] private int drawMoveCost = 0;
    public Transform ButtonCanvas;

    int battleMet = 0;

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
            GridManager.Instance.clearAttackPreview();
            return;
        }

        GridManager.Instance.showAllAttackPreview();
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

        yield return StartCoroutine(GridManager.Instance.DrawItemsFromPool());
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

            //clean all grid items

            foreach(var item in GridManager.Instance.GridItemDict.Values)
            {
                item.destory();
            }
            GridManager.Instance.RemoveAll();
            yield return new WaitForSeconds(GridManager.animTime * 2);

            yield return StartCoroutine( Luggage.Instance.RotateBackToOrigin());

            //add all grid items

            yield return StartCoroutine(GridManager.Instance.DrawAllItemsFromPool());

            outControl();
        }
    }



    public void takeControl()
    {
        isInControl = true;
        foreach (var item in itemsToActivate)
        {
            item.SetActive(true);
        }
        ButtonCanvas.gameObject.SetActive(true);
        showButtonCanvas();
        StartBattle();
        //StartCoroutine(test());
    }
    public void outControl()
    {
        isInControl = false;
        foreach (var item in itemsToActivate)
        {
            item.SetActive(false);
        }
        hideButtonCanvas();
        ButtonCanvas.gameObject.SetActive(false);
        baseScene.hasFinished = true;
        StageManager.Instance.takeControl();
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
        attackCountUsed = 0;
        UpdateText();
        DrawItem(true);
        SelectAttack();
        EnemyManager.Instance.SelectEenmiesAction();

        battleMet++;

    }
    public void AddEnemies(BattleType battleType)
    {
        var maxEnemy = 3;
        if (battleMet < 7)
        {
            maxEnemy = 2;
        }
        if(battleMet < 3)
        {
            maxEnemy = 1;
        }
        var enemyList = EnemyManager.Instance.GetEnemyInfosToAdd(battleMet, battleType, maxEnemy);

        if(enemyList.Count > enemyPositions.Length)
        {
            Debug.LogError("Adding too many enemies to fit the slots");
        }
        for(int x = 0; x < enemyList.Count; x++)
        {
            var enemySlot = enemyPositions[x];
            var go = Instantiate(enemyPrefab, enemySlot.position, Quaternion.identity, enemySlot);
            go.GetComponent<Enemy>().Init(enemyList[x]);
            go.transform.parent = enemySlot;
            //go.transform.position = 
            //go.transform.localPosition = Vector3.zero;//enemySlot.position;
        }
        EnemyManager.Instance.setCurrentTargetedEnemy(EnemyManager.Instance.GetFrontEnemy());
    }


    void SelectAttack()
    {
        selectedAttackIndex = Random.Range(0, 3);
        moveLeft = moveMax + LuggageManager.Instance.UpgradedTime[UpgradeType.actionCount];
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

        attackCountUsed ++;
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
        if (isBattleFinished)
        {
            return;
        }
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
        EnemyManager.Instance.SelectEenmiesAction();
        yield return StartCoroutine(DrawItemEnumerator(true));
        attackCountUsed = 0;
        UpdateText();

        
        showButtonCanvas();



    }

    void clearTurnData()
    {

        attackCountUsed = 0;
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
        LuggageAttackButton.interactable = CanAttack;
        if (CanAttack)
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
