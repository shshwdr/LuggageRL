using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttackPreview : MonoBehaviour
{

    public GameObject attackPanel;

    public Sprite empty;
    public Sprite attack;


    public GameObject otherPanel;
    public Image otherSprite;
    public Text otherText;

    public List<Image> previewCell;
    public void UpdateAttackPreview(int ind, bool startFromBottom)
    {
        otherPanel.SetActive(false);
        attackPanel.SetActive(true);
        foreach (var cell in previewCell)
        {
            cell.gameObject.SetActive(false);
        }
        for (int i = 0; i < ind; i++)
        {

            if (startFromBottom)
            {
                previewCell[previewCell.Count - 1 - i].gameObject.SetActive(true);
                previewCell[previewCell.Count - 1 - i].sprite = empty;
            }
        }
        if (startFromBottom)
        {
            previewCell[previewCell.Count - 1 - ind].gameObject.SetActive(true);
            previewCell[previewCell.Count - 1 - ind].sprite = attack;
        }
    }
    
    public void UpdateOtherPreview(string actionName, string count)
    {
        var sprite = Resources.Load<Sprite>("enemyActionSprite/" + actionName);
        if(sprite == null)
        {
            Debug.LogError("no action sprite " + actionName);
        }
        otherSprite.sprite = sprite;
        if(count == "")
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
