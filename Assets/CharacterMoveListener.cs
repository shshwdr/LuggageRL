using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveListener : BaseMovable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    override public void startMove()
    {
        Luggage.Instance.StartCharacterWalking();
    }
    override public void stopMove()
    {
        Luggage.Instance.StopCharacterWalking();
    }
}
