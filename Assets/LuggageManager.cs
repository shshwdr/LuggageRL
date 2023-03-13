using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LuggageManager : Singleton<LuggageManager>
{
    public Dictionary<UpgradeType, string> typeDescription = new Dictionary<UpgradeType, string>();
    public Dictionary<UpgradeType, int> UpgradedTime = new Dictionary<UpgradeType, int>();


    public bool isInControl;
    public bool WouldRemoveItem;
    public GameObject[] itemsToActivate;
    public SkipAndHealButton skipButton;
    // Start is called before the first frame update
    void Start()
    {
        typeDescription[UpgradeType.actionCount] = "Action Count +1";
        typeDescription[UpgradeType.drawCount] = "Draw Count +1";
        typeDescription[UpgradeType.hp] = "hp +10";
        typeDescription[UpgradeType.attackCount] = "Attack Count +1";
        typeDescription[UpgradeType.basicAttack] = "Final Damage +1";
        foreach (var type in typeDescription)
        {
            UpgradedTime[type.Key] = 0;
        }
        //DontDestroyOnLoad(this.gameObject);
    }
    public UpgradeType select1;
    public UpgradeType select2;
    public void selectedTypes()
    {
        List < UpgradeType > types = typeDescription.Keys.ToList();
        select1 = UpgradeType.basicAttack;//types[Random.Range(0,types.Count)];
        types.Remove(select1);
        select2 = types[Random.Range(0, types.Count)];
    }
    public void select(int ind)
    {
        UpgradeType sele;
        if (ind == 0)
        {
            sele = select1;

        }
        else
        {

            sele = select2;
        }
        //DialoguePopupManager.Instance.showDialogue("Do you want to upgrad this: " + typeDescription[sele], null, () => {


            UpgradedTime[sele]++;
            if (sele == UpgradeType.hp)
            {
                BattleManager.Instance.player.addMaxHP(10);
            }
            if(sele == UpgradeType.basicAttack)
            {
                BattleManager.Instance.updateBuff();
            }
            outControl();
        //});


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
}
