using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{

}

public class EnemyBehavior {

    public virtual string Name { get; }
}


public class DummyEnemy : EnemyBehavior
{

    public override string Name => "dummy";


}

