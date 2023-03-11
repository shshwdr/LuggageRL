using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralButtonSound : MonoBehaviour
{
    public void hover()
    {

        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_ui_hover, transform.position);
    }
    public void click()
    {

        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_ui_select, transform.position);
    }
}
