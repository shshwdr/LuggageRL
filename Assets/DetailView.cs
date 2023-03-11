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
    void Start()
    {
    }

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
