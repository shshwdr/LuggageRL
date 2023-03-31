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
    public Text LuggageAttackBuffText;
    public GameObject[] luggageAttackTutorial;
    public Text roundText;
    public GameObject[] itemsToActivate;

    public bool hasAttacked = false;
    
    public Button SkipTurnButton;

    public GameObject luggageAttackDisabledOb;
    public GameObject luggageAttackEnabledOb;

    public int finalDamageIncrease=>LuggageManager.Instance.UpgradedTime[UpgradeType.basicAttack];

    public Sprite[] tutorialAttackSprites;

    public Text moveHint;
    bool CanAttack => attackCountUsed < attackCount && moveLeft>=2;
    int attackCount => 1 + LuggageManager.Instance.UpgradedTime[UpgradeType.attackCount];
    int attackCountUsed = 0;
    public Text MoveText;
    int selectedAttackIndex = -1;
    private int maxAttackCount = 1;
    [SerializeField] private int moveMax = 4;
    int moveLeft;
    public bool isBattleFinished = false;
    string[] attackString = new string[] { "Pilling Hammer",
        "Hit Back",
        "Hit Up",
"Backflip",
//"Overhead Backbreaker"
};
    
    
    public GameObject enemyPrefab;
    public Transform[] enemyPositions;
    public Player player;
    [SerializeField] private int rotateMoveCost = 0;
    [SerializeField] public int attackMoveCost = 0;
    [SerializeField] private int swapActionCost = 0;
    [SerializeField] private int drawMoveCost = 0;
    public Transform ButtonCanvas;

    public int battleCount = 0;
    public int turnCount = 0;
    public bool canPlayerControl = true;
    public bool CanPlayerControl => canPlayerControl && !GameOver.Instance.isGameOver;
    public TurnSlider turnSlider;
    
    private int[] learnNewAttackBattle = new[] { 1, 3, 8 };
    public void LearnNewAttackType(int battleId)
    {
        if (maxAttackCount >= learnNewAttackBattle.Length)
        {
            return;
        }

        if (battleId >= learnNewAttackBattle[maxAttackCount-1])
        {
            DialoguePopupManager.Instance.showDialogue("You learned a new attack\n"+attackString[maxAttackCount]+"\nAttacks would randomly pick after each attack.",tutorialAttackSprites[maxAttackCount]);
        
            maxAttackCount++;
        }
    }
    
    public void hideButtonCanvas()
    {
        canPlayerControl = false;
        foreach (var button in ButtonCanvas.GetComponentsInChildren<Button>())
        {
            button.gameObject.SetActive(false);
        }

        MoveText.transform.parent.gameObject.SetActive(false);
    }
    void showButtonCanvas()
    {
        canPlayerControl = true;
        if (isBattleFinished)
        {
            GridManager.Instance.clearAttackPreview();
            return;
        }

        GridManager.Instance.showAllAttackPreview();
        //StartCoroutine(test());
        foreach (var button in ButtonCanvas.GetComponentsInChildren<Button>(true))
        {
            button.gameObject.SetActive(true);
        }
        MoveText.transform.parent.gameObject.SetActive(true);

        if (BabySittingTutorial.Instance.shouldDisableAction(PlayerActionType.EndTurn))
        {
            SkipTurnButton.gameObject.SetActive(false);
        }

    }
    IEnumerator test()
    {
        yield return new WaitForSeconds(0.3f);
        GridManager.Instance.showAllAttackPreview();
    }
    public void SkipMove()
    {
        moveLeft = 0;

        StartCoroutine(EndOfTurn());
    }
    public IEnumerator DrawItem(bool noCost = false)
    {
        if (!noCost && moveLeft < drawMoveCost)
        {
            yield break;
        }
        yield return StartCoroutine(DrawItemEnumerator(noCost));
    }
    public IEnumerator DrawItemEnumerator(bool noCost = false)
    {
        hideButtonCanvas();

        yield return StartCoroutine(GridManager.Instance.DrawItemsFromPool());
        showButtonCanvas();

        if (battleCount == 1 && turnCount == 0)
        {
            
            BabySittingTutorial.Instance.showOverlay(BabySittingTutorial.Instance.attackOverlay);
            //DetailView.Instance.showTutorial("Attack!", TutorialManager.Instance.getUnreadText("Attack!"));
        }
    }

    public IEnumerator FinishCurrentBattle()
    {
        if (!isBattleFinished)
        {
            isBattleFinished = true;


            DetailView.Instance.clearAll();
            if (battleCount >= 2)
            {
                BabySittingTutorial.Instance.finishTutorial();
            }
            
            hidePlayerAttack();

            yield return new WaitForSeconds(GridManager.animTime);

            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.music_win_round, transform.position);

            yield return StartCoroutine(GetComponentInChildren<TurnSlider>().ShowSlider("WIN!"));

            //DetailView.Instance.clearTutorial();
            moveHint.text = "";
            //FloatingTextManager.Instance.addText("Win Battle!", Vector3.zero, Color.red,1);
            //yield return new WaitForSeconds(GridManager.animTime*3);
            //RemoveText();
            //clearTurnData();
            //StartCoroutine(searchNextBattle());
            //hmm make this into a class to control all?

            //clean all grid items

            GridManager.Instance.clearAttackPreview();
            foreach (var item in GridManager.Instance.GridItemDict.Values)
            {
                item.destory();
            }
            GridManager.Instance.RemoveAll();
            yield return new WaitForSeconds(GridManager.animTime * 2);

            yield return StartCoroutine(Luggage.Instance.RotateBackToOrigin());

            //add all grid items

            yield return StartCoroutine(GridManager.Instance.DrawAllItemsFromPool());

            outControl();

            LearnNewAttackType(battleCount);
        }
    }

    void Start()
    {
        turnSlider = GetComponentInChildren<TurnSlider>();
        LuggageAttackButton.gameObject.SetActive(false);
    }

    public void takeControl()
    {

        updateBuff();
        if (battleCount == 1)
        {
            //DetailView.Instance.showTutorial("Different Attack", TutorialManager.Instance.getUnreadText("Different Attack"));
            //DetailView.Instance.showTutorial("Defend", TutorialManager.Instance.getUnreadText("Defend"));
        }
        //AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_enemy_death, transform.position);
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
        // if (battleCount == 2)
        // {
        //     DetailView.Instance.showTutorial("DifferentAttack", TutorialManager.Instance.getUnreadText("DifferentAttack"));
        //     
        // }
        hasAttacked = false;
        turnCount = 0;
        if (battleCount == 0)
        {
            moveHint.text = TutorialManager.Instance.getText("MoveItemHint");
        }
        
        battleCount++;

        StartCoroutine(startBattleEnumerator());
    }

    IEnumerator startBattleEnumerator()
    {
        
        foreach (var item in GridManager.Instance.GridItemDict.Values)
        {
            item.destory();
        }
        GridManager.Instance.RemoveAll();

        yield return new WaitForSeconds(GridManager.animTime * 2);
        
        StartCoroutine(turnSlider.ShowSlider("Player Turn"));
        
        
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
        moveLeft = moveMax + LuggageManager.Instance.UpgradedTime[UpgradeType.actionCount];
        yield return  StartCoroutine(DrawItem(true));
        SelectAttack();
        UpdateText();
        EnemyManager.Instance.SelectEenmiesAction();
        
        


    }

    // public void hoverHoverAttackButton()
    // {
    //
    // }
    public void AddEnemies(BattleType battleType)
    {
        var maxEnemy = 3;
        if (battleCount < 7)
        {
            maxEnemy = 2;
        }
        if (battleCount < 2)
        {
            maxEnemy = 1;
        }
        var enemyList = EnemyManager.Instance.GetEnemyInfosToAdd(battleCount, battleType, maxEnemy);

        if (enemyList.Count > enemyPositions.Length)
        {
            Debug.LogError("Adding too many enemies to fit the slots");
        }
        for (int x = 0; x < enemyList.Count; x++)
        {
            var enemySlot = enemyPositions[x];
            var go = Instantiate(enemyPrefab, enemySlot.position, Quaternion.identity, enemySlot);
            go.GetComponent<Enemy>().Init(enemyList[x], x);
            go.transform.parent = enemySlot;
            //go.transform.position = 
            //go.transform.localPosition = Vector3.zero;//enemySlot.position;
        }
        EnemyManager.Instance.setCurrentTargetedEnemy(EnemyManager.Instance.GetFrontEnemy());

        StartCoroutine(GridManager.Instance.DrawAllItemsFromPool());
    }


    public void SelectAttack()
    {
        if (selectedAttackIndex == -1)
        {
            selectedAttackIndex = 0;
        }
        else
        {
            selectedAttackIndex = BabySittingTutorial.Instance.selectAttack();
            if (selectedAttackIndex == -1)
            {
                selectedAttackIndex = Random.Range(0, maxAttackCount);
            }
        }
        //moveLeft = moveMax + LuggageManager.Instance.UpgradedTime[UpgradeType.actionCount];
        UpdateText();
        GridManager.Instance.updateAttackEdge();
        PredictNextAttack();
    }
    public IEnumerator useMove(int amount)
    {
        moveLeft -= amount;
        UpdateText();
        BabySittingTutorial.Instance. hideLines();
        if (moveLeft == 0)
        {
            yield return StartCoroutine(EndOfTurn());

            //yield return StartCoroutine(PlayerAttackMove());
        }
        showButtonCanvas();

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

    public void PlayerAttackManually()
    {
        if (moveLeft < attackMoveCost)
        {
            return;
        }

        attackCountUsed++;
        StartCoroutine(PlayerAttackMove(selectedAttackIndex));
    }
    List<int> attackIdToRotationId = new List<int>() { 0, 2, 1, 1 };
    public int getCurrentAttackRotationId()
    {
        return attackIdToRotationId[selectedAttackIndex];
    }

    public void FinishAttack()
    {
        
        BabySittingTutorial.Instance.showOverlay(BabySittingTutorial.Instance.endTurnOverlay);
        //DetailView.Instance.showTutorial("End Turn", TutorialManager.Instance.getUnreadText("End Turn"));
        if (battleCount == 2)
        {
            BabySittingTutorial.Instance.showOverlay(BabySittingTutorial.Instance.defenseOverlay);
            //DetailView.Instance.showTutorial("Defend", TutorialManager.Instance.getUnreadText("Defend"));
            BabySittingTutorial.Instance. showLines();
        }

        
    }
    public IEnumerator PlayerAttackMove(int moveId)
    {
        hasAttacked = true;
        
        
        
        hideButtonCanvas();
        GridManager.Instance.clearAttackPreview();
        //moveId = 0; //force to push (debug)

        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_luggage_attack_whoosh, new Vector3(0, 0, 0));


        switch (moveId)
        {
            case 0:
                yield return StartCoroutine(Luggage.Instance.PushForwardAttack());
                break;
            case 1:
                yield return StartCoroutine(Luggage.Instance.PushBackwardAttack());
                //yield return StartCoroutine(Luggage.Instance.UpsideDownAndDrop());
                break;
            case 2:
                yield return StartCoroutine(Luggage.Instance.PushUpwardAttack());
                //yield return StartCoroutine(Luggage.Instance.ThrowOutAndHitBack());
                break;
                //case 3:
                //    yield return StartCoroutine(Luggage.Instance.LiftAndDownAttack());
                //    break;
        }
        
        // if (battleCount == 1 && turnCount >= 2)
        // {
        //     DetailView.Instance.showTutorial("Fly Away", TutorialManager.Instance.getUnreadText("Fly Away"));
        //     
        // }
        
        //yield return useMove(attackMoveCost);

        //showButtonCanvas();
    }


    public void PredictNextAttack()
    {
        if (isBattleFinished || GameOver.Instance.isGameOver)
        {
            return;
        }
        switch (selectedAttackIndex)
        {
            case 0:
                GridManager.Instance.predict(1, 0);
                break;
            case 1:
                GridManager.Instance.predict(-1, 0);
                break;
            case 2:
                GridManager.Instance.predict(0, 1);
                break;
            
            case 3:
                GridManager.Instance.predict(0, 1);
                break;
            case 4:
                GridManager.Instance.predict(0, 1);
                break;
            case 5:
                GridManager.Instance.predict(0, -1);
                break;
        }

        showPlayerAttack();
    }

    void hidePlayerAttack()
    {
        player.attackOb.SetActive(false);
        playerAttack = 0;
    }

    public int playerAttack = 0;
    void showPlayerAttack()
    {
        if (isInControl && !isBattleFinished)
        {
            player.attackOb.SetActive(true);
            player.attackText.text = playerAttack.ToString();
            playerAttack = 0;
        }

    }
    public IEnumerator EndOfTurn()
    {
        hideButtonCanvas();
        hidePlayerAttack();

        yield return StartCoroutine(GridManager.Instance.EndTurnCardBehaviorEnumerator());

        if (!isBattleFinished)
        {
            yield return StartCoroutine(turnSlider.ShowSlider("Enemy Turn"));
        }
        yield return StartCoroutine(EnemyManager.Instance.EnemiesAttack());
        if (GameOver.Instance.isGameOver)
        {
            yield break;
        }
        clearTurnData();
        //SelectAttack();
        EnemyManager.Instance.SelectEenmiesAction();
        if (battleCount == 2 && turnCount == 0)
        {
            BabySittingTutorial.Instance.showOverlay(BabySittingTutorial.Instance.breakableOverlay);
            //DetailView.Instance.showTutorial("Destroy Breakable", TutorialManager.Instance.getUnreadText("Destroy Breakable"));
            //DetailView.Instance.showTutorial("Different Attack", TutorialManager.Instance.getUnreadText("Different Attack"));
            
        }
        // else if (battleCount == 2 && turnCount == 1)
        // {
        //     DetailView.Instance.showTutorial("Destroy Breakable", TutorialManager.Instance.getUnreadText("Destroy Breakable"));
        //     
        // }
        turnCount++;
        yield return StartCoroutine(DrawItemEnumerator(true));
        moveLeft = moveMax + LuggageManager.Instance.UpgradedTime[UpgradeType.actionCount];
        attackCountUsed = 0;

        if (turnCount == 1)
        {
            BabySittingTutorial.Instance.showOverlay(BabySittingTutorial.Instance.moveOverlay);
        }
        else
        {
            
            BabySittingTutorial.Instance.showOverlay(BabySittingTutorial.Instance.arrowOverlay);
        }
        //DetailView.Instance.showTutorial("Drag", TutorialManager.Instance.getUnreadText("Drag"));

        
        //BabySittingTutorial.Instance.showLines();
        
        UpdateText();


        showButtonCanvas();
        DetailView.Instance.clearAll();
        
        

        if (!isBattleFinished)
        {

            yield return StartCoroutine(turnSlider.ShowSlider("Player Turn"));
        }
        else
        {

        }

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

        foreach (var item in luggageAttackTutorial)
        {
            item.SetActive(false);
        }
        roundText.text = $"{attackCount}x per round";
        luggageAttackTutorial[selectedAttackIndex].SetActive(true);
        if (CanAttack)
        {

            luggageAttackEnabledOb.SetActive(true);
            luggageAttackDisabledOb.SetActive(false);
            LuggageAttackText.text = $"{attackString[selectedAttackIndex]} ({attackCount - attackCountUsed})";
        }
        else
        {

            LuggageAttackText.text = $"";
            luggageAttackEnabledOb.SetActive(false);
            luggageAttackDisabledOb.SetActive(true);
        }
        MoveText.text = $"{moveLeft}";
    }
    public void updateBuff()
    {
        LuggageAttackBuffText.text = LuggageManager.Instance.UpgradedTime[UpgradeType.basicAttack] == 0?"":$"(+{LuggageManager.Instance.UpgradedTime[UpgradeType.basicAttack]})";
    }
    void RemoveText()
    {
        FloatingTextManager.Instance.addText("Search for next Enemy", Vector3.zero, Color.white);
        //LuggageAttackText.text = "Search for next Enemy";u
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
