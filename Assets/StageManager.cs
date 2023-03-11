using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BiomeType { none,forest, desert,river}
public enum StageEventType { normalBattle, bossBattle, eliteBattle, itemSelect, upgradeLuggageNPC, upgradeRemoveNPC}
public class StageManager : Singleton<StageManager>
{
    [SerializeField] GameObject battleScenePrefab;
    [SerializeField] GameObject itemSelectScenePrefab;
    [SerializeField] GameObject removeItemScenePrefab;
    [SerializeField] GameObject luggageUpgradeScenePrefab;

    [SerializeField] float sceneDistanceStart;
    [SerializeField] float sceneDistanceEnd;
    public bool isMoving = true;
    int eventIndex = 0;
    public BiomeType biomeType = BiomeType.forest;
    public List<StageEventType> eventList = new List<StageEventType>() { StageEventType.itemSelect, StageEventType.normalBattle, StageEventType.itemSelect, StageEventType.normalBattle, StageEventType.itemSelect };
    // Start is called before the first frame update
    void Start()
    {
        takeControl();
    }

    public void takeControl()
    {
        var res  = addNextEvent();
        if (res)
        {

            foreach (var item in GameObject.FindObjectsOfType<BaseMovable>())
            {
                item.startMove();
            }
        }
    }

    public void outControl()
    {
        foreach (var item in GameObject.FindObjectsOfType<BaseMovable>())
        {
            item.stopMove();
        }
    }

    bool addNextEvent()
    {
        foreach (var scene in Transform.FindObjectsOfType<BaseScene>())
        {
            if (!scene.hasStarted)
            {
                return false;
            }
        }
        if (eventIndex >= eventList.Count)
        {
            FloatingTextManager.Instance.addText("Finished Stage!", Vector3.zero, Color.yellow, 5);
            return true;
        }
        float randomX = Random.Range(sceneDistanceStart, sceneDistanceEnd);
        GameObject go = null;
        bool res = false;
        switch (eventList[eventIndex])
        {
            case StageEventType.bossBattle:
                go = Instantiate(battleScenePrefab);
                go.GetComponent<BattleScene>().battleType = BattleType.boss;
                go.transform.position += new Vector3(randomX, 0, 0);
                res = true;
                break;
            case StageEventType.eliteBattle:
                go = Instantiate(battleScenePrefab);
                go.GetComponent<BattleScene>().battleType = BattleType.elite;
                go.transform.position += new Vector3(randomX, 0, 0);
                res = true;

                break;

            case StageEventType.normalBattle:
                go = Instantiate(battleScenePrefab);
                go.transform.position += new Vector3(randomX, 0, 0);
                res = true;

                break;
            case StageEventType.itemSelect:

                go = Instantiate(itemSelectScenePrefab);
                break;
            case StageEventType.upgradeRemoveNPC:

                go = Instantiate(removeItemScenePrefab);
                break;
            case StageEventType.upgradeLuggageNPC:

                go = Instantiate(luggageUpgradeScenePrefab);
                break;

        }
        eventIndex++;
        return res;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
