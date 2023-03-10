using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailView : Singleton<DetailView>
{
    public Text enemyDescription;
    public Text actionDescription;
    public GameObject enemyView;
    void Start()
    {
    }
    public void UpdateValue(Enemy enemy)
    {
        if(enemy == null)
        {
            enemyView.SetActive(false);
        }
        else
        {
            enemyView.SetActive(true);
            enemyDescription.text = enemy.Desc;
            actionDescription.text = enemy.Core.currentAction.Desc;
        }
    }
}
