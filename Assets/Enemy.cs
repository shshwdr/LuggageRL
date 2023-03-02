using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EnemyManager.Instance.AddEnemy(this);
    }
    int damage = 0;
    public void GetDamage(int dam)
    {
        damage += dam;
    }
    public void ClearDamage()
    {
        damage = 0;
    }

    public void ShowDamage()
    {

        FloatingTextManager.Instance.addText(damage.ToString(), transform.position+new Vector3(0,1,0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
