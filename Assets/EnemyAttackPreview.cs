using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttackPreview : MonoBehaviour
{

    public GameObject attackPanel;

    public Sprite empty;
    public Sprite attack;
    public Sprite topHalfEmpty;
    public Sprite bottomHalfEmpty;


    public GameObject otherPanel;
    public Image otherSprite;
    public Text otherText;
    public Image otherSprite2;

    public Color shieldColor;
    public Color healColor;


    public List<Image> previewCell;
    public void UpdateAttackPreview(int ind, bool startFromBottom, int attackAmount,int attackRangeV)
    {
        otherPanel.SetActive(false);
        attackPanel.SetActive(true);
        foreach (var cell in previewCell)
        {
            cell.gameObject.SetActive(false);
        }
        if (startFromBottom)
        {
            for (int i = 0; i < ind; i++)
            {

                previewCell[previewCell.Count - 1 - i].gameObject.SetActive(true);
                previewCell[previewCell.Count - 1 - i].sprite = empty;
                previewCell[previewCell.Count - 1 - i].GetComponentInChildren<Text>().text = "";

            }
            for(int i = 0;i< attackRangeV; i++)
            {

                previewCell[previewCell.Count - 1 - ind - (i)].gameObject.SetActive(true);
                previewCell[previewCell.Count - 1 - ind - (i )].sprite = attack;
                previewCell[previewCell.Count - 1 - ind - (i)].GetComponentInChildren<Text>().text = attackAmount.ToString();
            }

            //if()
            previewCell[previewCell.Count - 1 - ind - (attackRangeV - 1)-1].gameObject.SetActive(true);
            previewCell[previewCell.Count - 1 - ind - (attackRangeV - 1) - 1].sprite = bottomHalfEmpty;
            previewCell[previewCell.Count - 1 - ind - (attackRangeV - 1) - 1].GetComponentInChildren<Text>().text = "";
        }
        else
        {
            previewCell[previewCell.Count - 2].gameObject.SetActive(true);
            previewCell[previewCell.Count - 2].sprite = attack;
            previewCell[previewCell.Count - 2].GetComponentInChildren<Text>().text = attackAmount.ToString();
            previewCell[previewCell.Count - 1].gameObject.SetActive(true);
            previewCell[previewCell.Count - 1].sprite = topHalfEmpty;
            previewCell[previewCell.Count - 1].GetComponentInChildren<Text>().text = "";
        }
    }
    public void UpdateOtherPreviewTwoImage(string actionName, string itemName)
    {

        otherSprite.sprite = Resources.Load<Sprite>("enemyActionSprite/" + actionName);
        otherSprite2.sprite = Resources.Load<Sprite>("itemSprite/" + itemName);
        otherText.gameObject.SetActive(false);
        otherSprite2.gameObject.SetActive(true);
        otherPanel.SetActive(true);
        attackPanel.SetActive(false);
    }
    public void UpdateOtherPreview(string actionName, string count)
    {
        if (actionName == "shield")
        {
            otherText.color = shieldColor;
        }
        else if (actionName == "heal")
        {

            otherText.color = healColor;
        }
        Sprite sprite;
        //if (isAction)
        //{
        sprite = Resources.Load<Sprite>("enemyActionSprite/" + actionName);
        //}
        ////else
        //{

        //    sprite = Resources.Load<Sprite>("itemSprite/" + actionName);
        //}
        if (sprite == null)
        {
            Debug.LogError("no action sprite " + actionName);
        }
        otherSprite.sprite = sprite;
        otherSprite2.gameObject.SetActive(false);
        if (count == "")
        {

            otherText.gameObject.SetActive(false);
        }
        else
        {
            otherText.gameObject.SetActive(true);
            otherText.text = count;
        }
        otherText.text = count;
        otherPanel.SetActive(true);
        attackPanel.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
