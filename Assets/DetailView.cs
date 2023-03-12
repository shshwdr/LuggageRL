using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailView : Singleton<DetailView>
{
    public Text enemyName;
    public Text enemyDescription;
    public Text actionDescription;
    public GameObject enemyView;


    public GameObject CardView;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Desc;
    public TextMeshProUGUI Stretegy;
    public TextMeshProUGUI Attack;
    public TextMeshProUGUI Defense;
    public Image image;
    public Transform buffParent;
    public GameObject buffPrefab;
    

    void Start()
    {
    }
    Dictionary<BuffType, string> buffMap = new Dictionary<BuffType, string>() { { BuffType.piggyBank, "PiggyBank" },{ BuffType.poison, "Poison" },{ BuffType.balancer, "Balancer" } };  
    public void UpdateCard(GridItemCore item)
    {

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
            actionDescription.text = enemy.Core.currentAction.Desc;
        }
    }
}
