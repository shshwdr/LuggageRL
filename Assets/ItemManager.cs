using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public Transform[] itemPositions;
    public GameObject[] itemsToActivate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject createItem(ItemType type,Transform parent, Vector3 pos)
    {

        GameObject obj = Instantiate(Resources.Load<GameObject>("items/" + type.ToString()),pos,Quaternion.identity, parent);
        obj.GetComponent<GridItem>().init(new Vector2Int(0,0), type);
        return obj;
    }
    GameObject item1;
    GameObject item2;
    public void AddItems()
    {
        var enumLength = System.Enum.GetValues(typeof(ItemType)).Length;
        var pickedItem1 = Random.Range(0, enumLength);
        var pickedItem2 = Random.Range(0, enumLength);
        item1 = createItem((ItemType)pickedItem1, itemPositions[0], itemPositions[0].position);
        item1.GetComponent<Draggable>().enabled = false;
        
        item2 = createItem((ItemType)pickedItem2, itemPositions[1], itemPositions[1].position);
        item2.GetComponent<Draggable>().enabled = false;
    }
    public void takeControl()
    {
        foreach(var item in itemsToActivate)
        {
            item.SetActive(true);
        }
        item1.AddComponent<SelectableItem>();
        item2.AddComponent<SelectableItem>();
        //StartCoroutine(test());
    }
    //IEnumerator test()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    showButtonCanvas();
    //    StartBattle();
    //}
    public void outControl()
    {

        foreach (var item in itemsToActivate)
        {
            item.SetActive(false);
        }

        item1. gameObject.SetActive (false);
        item2.gameObject.SetActive(false);
    }

    public void select(SelectableItem item)
    {
        GridManager.Instance.addItemToDeck(item.GetComponent<GridItem>().type);
        outControl();
        StageManager.Instance.takeControl();


    }
}
