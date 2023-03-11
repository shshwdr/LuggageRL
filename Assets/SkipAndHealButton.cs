using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipAndHealButton : MonoBehaviour
{
    public Action actionWhenPressed;
    public int percentage = 50;
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Text>().text = string.Format("Skip And Heal", percentage,  (int)(BattleManager.Instance.player.maxHP * percentage/100));
        GetComponent<Button>().onClick.AddListener(()=> {
            DialoguePopupManager.Instance.showDialogue(
            TutorialManager.Instance.getText("SkipAndHeal"), null, () =>
            {
                StartCoroutine(BattleManager.Instance.player.HealEnumerator(BattleManager.Instance.player.maxHP * percentage / 100));
                actionWhenPressed.Invoke();
            });
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
