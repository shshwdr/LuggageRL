using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseItem : MonoBehaviour
{
    public Text AttackText;
    public Text HealText;
    public GameObject DestroyOverlay;

    public void WillAttack(int damage)
    {
        AttackText.text = damage.ToString();
        AttackText.gameObject.SetActive(true);
    }
    public void WillHeal(int damage)
    {
        HealText.text = damage.ToString();
        HealText.gameObject.SetActive(true);
    }
    public void WillDestroy()
    {
        DestroyOverlay.SetActive(true);
    }
    public void ClearOverlayes()
    {
        HealText.gameObject.SetActive(false);
        DestroyOverlay.SetActive(false);
        AttackText.gameObject.SetActive(false);
    }
}
