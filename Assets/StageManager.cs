using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageEventType { normalBattle, bossBattle, eliteBattle, itemSelect, upgradeLuggageNPC, upgradeItemNPC}
public class StageManager : Singleton<StageManager>
{
    [SerializeField] GameObject battleScenePrefab;
    [SerializeField] GameObject itemSelectScenePrefab;

    [SerializeField] float sceneDistanceStart;
    [SerializeField] float sceneDistanceEnd;
    public bool isMoving = true;
    int eventIndex = 0;
    public List<StageEventType> eventList = new List<StageEventType>() { StageEventType.itemSelect, StageEventType.normalBattle, StageEventType.itemSelect, StageEventType.normalBattle, StageEventType.itemSelect };
    // Start is called before the first frame update
    void Start()
    {
        takeControl();
    }

    public void takeControl()
    {
        addNextEvent();
        foreach(var item in GameObject.FindObjectsOfType<BaseMovable>())
        {
            item.startMove();
        }
    }

    public void outControl()
    {
        foreach (var item in GameObject.FindObjectsOfType<BaseMovable>())
        {
            item.stopMove();
        }
    }

    void meetEvent()
    {

    }
    void addNextEvent()
    {
        if (eventIndex >= eventList.Count)
        {
            FloatingTextManager.Instance.addText("Finished Stage!", Vector3.zero, Color.yellow, 5);
        }
        float randomX = Random.Range(sceneDistanceStart, sceneDistanceEnd);
        GameObject go = null;
        switch (eventList[eventIndex]) {
            case StageEventType.normalBattle:
                go = Instantiate(battleScenePrefab);

                break;
            case StageEventType.itemSelect:

                go = Instantiate(itemSelectScenePrefab);
                break;
        }
        eventIndex++;
        go.transform.position += new Vector3(randomX, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}