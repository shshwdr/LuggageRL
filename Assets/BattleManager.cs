using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : Singleton<BattleManager>
{
    public Text LuggageAttackText;
    int selected;
    int moveMax = 2;
    int moveLeft;
    string[] attackString = new string[] {"Push","Upside Down","Throw And Back" };
    // Start is called before the first frame update
    void Start()
    {
        SelectAttack();
    }

    void SelectAttack()
    {
        selected = Random.Range(0, 3);
        moveLeft = moveMax;
        UpdateText();
    }

    public IEnumerator Move()
    {
        moveLeft -= 1;
        UpdateText();
        if(moveLeft == 0)
        {
            switch (selected)
            {
                case 0:
                    yield return StartCoroutine(Luggage.Instance.PushForwardAttack());
                    break;
                case 1:
                    yield return StartCoroutine(Luggage.Instance.UpsideDownAndDrop());
                    break;
                case 2:
                    yield return StartCoroutine(Luggage.Instance.ThrowOutAndHitBack());
                    break;
            }
            SelectAttack();
        }
    }

    void UpdateText()
    {
        LuggageAttackText.text = $"Next Attack: {attackString[selected]} (in {moveLeft} moves)";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
