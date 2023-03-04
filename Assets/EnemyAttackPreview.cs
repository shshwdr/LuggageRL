using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttackPreview : MonoBehaviour
{
    public Sprite empty;
    public Sprite attack;

    public List<Image> previewCell;
    public void UpdatePreview(int ind, bool startFromBottom)
    {
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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
