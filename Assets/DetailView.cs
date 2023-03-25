using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailView : Singleton<DetailView>
{
    public Text enemyName;
    public Text enemyDescription;
    public Text actionDescription;
    public GameObject enemyView;


    public GameObject willAttackOB;
    public GameObject willDefendOB;
    public GameObject willDestoryOB;

    public GameObject CardView;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Desc;
    public TextMeshProUGUI Stretegy;
    public TextMeshProUGUI Attack;
    public TextMeshProUGUI Defense;
    public Image image;
    public Transform buffParent;
    public GameObject buffPrefab;

    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI tutorialTextTitle;


    private void Start()
    {
        CardView.SetActive(false);
        enemyView.SetActive(false);
        tutorialText.transform.parent.gameObject.SetActive(false);
    }
    public void showTutorial(string title, string t)
    {
        if(t == "")
        {
            return;
        }

        tutorialText.transform.parent.DOShakeScale(1,0.5f);
        tutorialTextTitle.text = title;
        tutorialText.text = t.Replace('/','\n');
        tutorialText.transform.parent.gameObject.SetActive(true);
    }
    public void clearTutorial()
    {
        tutorialText.text = "";
        tutorialText.transform.parent.gameObject.SetActive(false);
    }
    Dictionary<BuffType, string> buffMap = new Dictionary<BuffType, string>() { { BuffType.piggyBank, "PiggyBank" },{ BuffType.poison, "Poison" },{ BuffType.balancer, "Balancer" } };  
    public void UpdateCard(BaseItem baseItem)
    {
        var item = baseItem?baseItem.GetComponent<GridItem>().core:null;
        enemyView.SetActive(false);
        if (item == null)
        {
            CardView.SetActive(false);
        }
        else
        {

            CardView.SetActive(true);
            image.sprite = item.info.sprite;
            Desc.text = item.Desc;
            Stretegy.text = item.info.Strategy;
            Name.text = item.info.DisplayName;
            Attack.text = item.Attack.ToString();
            Defense.text = item.info.Defense.ToString();

            willDefendOB.SetActive(baseItem.willDefend);
            willAttackOB.SetActive(baseItem.willAttack);
            willDestoryOB.SetActive(baseItem.willBeDestroyed);
            

            foreach (Transform child in buffParent)
            {
                if(child != buffPrefab && child.gameObject.active)
                {
                    Destroy(child.gameObject);
                }
            }
            for (int i = 0;i< item.buffs.Keys.Count; i++)
            {
                var key = item.buffs.Keys[i];
                var value = item.buffs.Values[i];
                var go = Instantiate(buffPrefab, buffParent);
                go.SetActive(true);
                go.GetComponentInChildren<Image>().sprite = ItemManager.Instance.getItemInfo( buffMap[key]).sprite;
                go.GetComponentInChildren<Text>().text = value.ToString();
            }
        }
        }
    public void UpdateValue(Enemy enemy)
    {
        CardView.SetActive(false);
        if (enemy == null)
        {
            enemyView.SetActive(false);
        }
        else
        {
            enemyView.SetActive(true);
            enemyName.text = enemy.DisplayName;
            enemyDescription.text = enemy.Desc;
            actionDescription.text = "Next turn: "+ enemy.Core.currentAction.Desc;
        }
    }

}
