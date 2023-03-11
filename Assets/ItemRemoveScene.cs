using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemRemoveScene : BaseScene
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Text>(true).text = TutorialManager.Instance.getText("RemoveItemSelection");
        //itemPositions = itemPositionsParent.GetComponentsInChildren<Transform>();
        //ItemRemoveManager.Instance.itemPositions = itemPositions;
        ItemRemoveManager.Instance.itemsToActivate = itemsToActivate;
        ItemRemoveManager.Instance.skipButton = GetComponentInChildren<SkipAndHealButton>();
       //ItemRemoveManager.Instance.AddItems();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!hasStarted)
        {
            if (transform.position.x <= 0)
            {
                hasStarted = true;
                //start battle
                ItemRemoveManager.Instance.takeControl();
                StageManager.Instance.outControl();
            }
        }
    }
}
