using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum UpgradeType { actionCount, drawCount, hp, attackCount, basicAttack}
public class LuggageUpgradeScene : BaseScene
{
    public Button button1;
    public Button button2;
    // Start is called before the first frame update
    void Start()
    {




        GetComponentInChildren<Text>(true).text = TutorialManager.Instance.getText("LuggageUpgradeSelect");
        //itemPositions = itemPositionsParent.GetComponentsInChildren<Transform>();
        //ItemRemoveManager.Instance.itemPositions = itemPositions;
        LuggageManager.Instance.itemsToActivate = itemsToActivate;
        LuggageManager.Instance.skipButton = GetComponentInChildren<SkipAndHealButton>();
        LuggageManager.Instance.selectedTypes();
        button1.GetComponentInChildren<Text>().text = LuggageManager.Instance.typeDescription[LuggageManager.Instance.select1];
        button1.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("upgradeIcons/" + LuggageManager.Instance.select1.ToString());
        button1.onClick.AddListener(() => LuggageManager.Instance.select(0));
        button2.GetComponentInChildren<Text>().text = LuggageManager.Instance.typeDescription[LuggageManager.Instance.select2];
        button2.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("upgradeIcons/" + LuggageManager.Instance.select2.ToString());
        button2.onClick.AddListener(() => LuggageManager.Instance.select(1));
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
                LuggageManager.Instance.takeControl();
                StageManager.Instance.outControl();
            }
        }
    }
}
