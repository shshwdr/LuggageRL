using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Text hpText;
    public Image hpFrontImage;

    public void updateHPBar(int hp, int maxHP)
    {
        hpText.text = hp + " / " + maxHP;
        hpFrontImage.fillAmount = (float)hp / (float)maxHP;
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
