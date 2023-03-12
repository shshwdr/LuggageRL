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
    
    public void showDialogue(string t, Sprite s = null, Action action1 = null, Action action2 = null)
    {
        if(t == "") { return; }
        if (action1 != null)
        {

            action1.Invoke();
            return;
        }
        Time.timeScale = 0;
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

        if (action1 != null)
        {
            button1.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_ui_select, transform.position);
                action1.Invoke();
                panel.SetActive(false);

                Time.timeScale = 1;
            }

                );

            button2.gameObject.SetActive(true);

            button2.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_ui_select, transform.position);
                panel.SetActive(false);
                Time.timeScale = 1;

            }
            );
        }
        else
        {

            button1.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_ui_select, transform.position);
                panel.SetActive(false);

                Time.timeScale = 1;
            }

                );
            button2.gameObject.SetActive(false);
        }
    }
}
