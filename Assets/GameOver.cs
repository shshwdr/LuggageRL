using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : Singleton<GameOver>
{
    public bool isGameOver = false;

    public Text selectItemText;
    public Text bubbleText;
    public Transform selectedItems;
    public Button restartButton;
    public GameObject panel;
    public List<GridItem> selected;
    bool finishedAnim = false;
    bool isWin = false;
    public void select(SelectableItem sitem)
    {
        if (!finishedAnim)
        {
            return;
        }
        var item = sitem.GetComponent<GridItem>();
        if (selected.Contains(item))
        {
            selected.Remove(item);
        }
        else
        {
            if(selected.Count == GameManager.Instance. canKeepItemsCount)
            {
                selected.RemoveAt(0);
            }
            selected.Add(item);
        }

        foreach(Image rend in selectedItems.GetComponentsInChildren<Image>())
        {
            rend.gameObject.SetActive(false);
        }
        for(int i = 0; i < selected.Count; i++)
        {
            selectedItems.GetComponentsInChildren<Image>(true)[i].gameObject.SetActive(true);
            selectedItems.GetComponentsInChildren<Image>(true)[i].sprite = selected[i].core.info.sprite;
        }
    }

    public void restart()
    {
        GameManager.Instance.preselectedItems.Clear();


        foreach (var item in selected)
        {
            GameManager.Instance.preselectedItems.Add(item.type);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void startGameOver(bool win)
    {
        if (isGameOver)
        {
            return;
        }
        isGameOver = true;
        isWin = win;
        BattleManager.Instance.hideButtonCanvas();
        GameManager.Instance.canKeepItemsCount++;
        GameManager.Instance.canKeepItemsCount = Mathf.Min(GameManager.Instance.canKeepItemsCount, 3);


        if (win)
        {
            bubbleText.text = TutorialManager.Instance.getText("WinGame");
        }
        else
        {

            bubbleText.text = TutorialManager.Instance.getText("Gameover");
        }
            selectItemText.text = $"Select {GameManager.Instance.canKeepItemsCount} item in the bag for your next run";

        StartCoroutine(gameover());
    }



    IEnumerator gameover()
    {
        yield return StartCoroutine( BattleManager.Instance.turnSlider.ShowSlider(isWin?"You Win!":"Game Over!"));

        GridManager.Instance.clearAttackPreview();
        foreach (var item in GridManager.Instance.GridItemDict.Values)
        {
            item.destory();
        }
        GridManager.Instance.RemoveAll();
        yield return new WaitForSeconds(GridManager.animTime * 2);

        yield return StartCoroutine(Luggage.Instance.RotateBackToOrigin());

        //add all grid items

        yield return StartCoroutine(GridManager.Instance.DrawAllItemsFromPool());

        foreach (Image rend in selectedItems.GetComponentsInChildren<Image>(true))
        {
            rend.gameObject.SetActive(false);
        }
        finishedAnim = true;
        panel.SetActive(true);
        restartButton.gameObject.SetActive(true);
        restartButton.onClick.AddListener(restart);
    }
}
