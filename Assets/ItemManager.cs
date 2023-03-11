using Sinbad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemInfo
{
    public string Item;
    public string DisplayName;

    public int Param1; 
    public int Param2;
    public string Description;
    public string Type;
    public bool IsBreakable => Type == "Breakable";
    public int DestoryAfterTrigger;
    public int Rarity;
    public int MaxCountInDeck;
    public string Strategy;
    public int Defense;

    public Sprite sprite => Resources.Load<Sprite>("itemSprite/" + Item);

}

public class ItemManager : Singleton<ItemManager>
{
    public bool isInControl;
    public Transform[] itemPositions;
    public GameObject[] itemsToActivate;
    public Dictionary<string, ItemInfo> itemDict = new Dictionary<string, ItemInfo>();
    public SkipAndHealButton skipButton;
    // Start is called before the first frame update
    void Start()
    {

        var itemInfos = CsvUtil.LoadObjects<ItemInfo>("item");
        foreach(var item in itemInfos)
        {
            itemDict[item.Item] = item;
        }
    }

    public ItemInfo getItemInfo(string itemName)
    {
        if (itemDict.ContainsKey(itemName))
        {
            return itemDict[itemName];

        }
        Debug.LogError("no item " + itemName);
        return null;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public GameObject createItem(ItemType type, Transform parent, Vector3 pos , int x = 0, int y = 0)
    {

        GameObject obj = Instantiate(Resources.Load<GameObject>("items/gridItem"), pos, Quaternion.identity, parent);
        obj.GetComponent<GridItem>().init(new Vector2Int(x,y), type);
        return obj;
    }
    GameObject item1;
    GameObject item2;
    public void AddItems()
    {
        var enumLength = System.Enum.GetValues(typeof(ItemType)).Length;
        var pickedItem1 = Random.Range(0, enumLength);
        var pickedItem2 = Random.Range(0, enumLength);

        Debug.Log(pickedItem1);
        Debug.Log(pickedItem2);

        item1 = createItem((ItemType)pickedItem1, itemPositions[0], itemPositions[0].position);
        item1.GetComponent<Draggable>().enabled = false;

        item2 = createItem((ItemType)pickedItem2, itemPositions[1], itemPositions[1].position);
        item2.GetComponent<Draggable>().enabled = false;
    }
    public void takeControl()
    {
        skipButton.actionWhenPressed = outControl;
        isInControl = true;
        foreach (var item in itemsToActivate)
        {
            item.SetActive(true);
        }
    }
    public void outControl()
    {

        isInControl = false;
        foreach (var item in itemsToActivate)
        {
            item.SetActive(false);
        }

        item1.gameObject.SetActive(false);
        item2.gameObject.SetActive(false);
        StageManager.Instance.takeControl();
    }

    public void select(SelectableItem item)
    {
        DialoguePopupManager.Instance.showDialogue(TutorialManager.Instance.getText("Popup_AddItem"), item.GetComponent<GridItem>().Core.info.sprite, () =>
          {
              GridManager.Instance.addItemToDeck(item.GetComponent<GridItem>().type);
              StartCoroutine( GridManager.Instance.DrawAllItemsFromPool());

              outControl();
          });

        


    }
}
