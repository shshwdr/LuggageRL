using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseItem : MonoBehaviour
{
    public Text AttackText;
    public GameObject DestroyOverlay;

    public void WillAttack(int damage)
    {
        AttackText.text = damage.ToString();
        AttackText.gameObject.SetActive(true);
    }
    public void WillDestroy()
    {
        DestroyOverlay.SetActive(true);
    }
    public void ClearOverlayes()
    {

        DestroyOverlay.SetActive(false);
        AttackText.gameObject.SetActive(false);
    }
}
