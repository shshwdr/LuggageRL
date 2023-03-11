using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectScene : BaseScene
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Text>(true).text = TutorialManager.Instance.getText("PickupSelection");
        itemPositions = itemPositionsParent.GetComponentsInChildren<Transform>();
        ItemManager.Instance.itemPositions = itemPositions;
        ItemManager.Instance.itemsToActivate = itemsToActivate;
        ItemManager.Instance.AddItems();

        ItemManager.Instance.skipButton = GetComponentInChildren<SkipAndHealButton>(); 
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
                ItemManager.Instance.takeControl();
                StageManager.Instance.outControl();
            }
        }
    }
}
