using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePopupManager : Singleton<DialoguePopupManager>
{
    public Text text;
    public Image image;
    public Button button1;
    public Button button2;
    public GameObject panel;
    
    public void showDialogue(string t, Sprite s, Action action1 = null, Action action2 = null)
    {
        panel.SetActive(true);
        text.text = t;
        if (s)
        {
            image.gameObject.SetActive(true);
            image.sprite = s;
        }
        else
        {
            image.gameObject.SetActive(false);
        }

        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();

        if (button1 != null)
        {
            button1.onClick.AddListener(() =>
            {
                action1.Invoke();
                panel.SetActive(false);
            }

                );

            button2.gameObject.SetActive(true);

            button2.onClick.AddListener(() =>
            {
                panel.SetActive(false);

            }
            );
        }
        else
        {

            button2.gameObject.SetActive(false);
        }
    }
}
