using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRemoveManager : Singleton<ItemRemoveManager>
{
    public bool isInControl;
    public bool WouldRemoveItem;
    public GameObject[] itemsToActivate;
    public SkipAndHealButton skipButton;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
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
        StageManager.Instance.takeControl();

    }

    public void select(SelectableItem item)
    {
        DialoguePopupManager.Instance.showDialogue(TutorialManager.Instance.getText("Popup_RemoveItem"), item.GetComponent<GridItem>().Core.info.sprite, () =>
        {
            GridManager.Instance.RemoveDeck(item.GetComponent<GridItem>());

            StartCoroutine(GridManager.Instance.MoveAfter(0, -1));

            outControl();
        });
    }
}
