using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckMenu : MonoBehaviour
{
    public GameObject Panel;

    public GameObject cell;
    Transform cellParent;

    public void showDeck()
    {
        Panel.SetActive(true);
        foreach (Transform child in cellParent)
        {
            if (child.gameObject.activeSelf)
            {
                
                Destroy(child.gameObject);
            }
        }

        foreach (var itemType in GridManager.Instance.DeckPool)
        {
            var newCell = Instantiate(cell, cellParent);
            newCell.GetComponent<ItemUICell>().item.sprite = Resources.Load<Sprite>("itemSprite/" + itemType);
            newCell.gameObject.SetActive(true);
            if (ItemManager.Instance.getItemInfo(itemType.ToString()).IsBreakable)
            {
                newCell.GetComponent<ItemUICell>().glassBK.gameObject.SetActive(true);
                newCell.GetComponent<ItemUICell>().bk.gameObject.SetActive(false);
            }
        }
    }

    public void hide()
    {
        
        Panel.SetActive(false);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        cellParent = cell.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
