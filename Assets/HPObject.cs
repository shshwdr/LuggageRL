using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HPObject : MonoBehaviour
{
    public int maxHP = 10;
    protected int hp = 0;
    public bool isDead = false;
    public HPBar hpbar;
    public int shield;

    private int currentDamageCounter = 0;
    private FloatingText currentFloatingText;
    public virtual IEnumerator ApplyDamage(int damage, bool isFinalAttack = true)
    {
        currentDamageCounter += damage;
        if (shield > 0)
        {
            var reduce = Mathf.Min(damage, shield);
            damage -= reduce;
            yield return StartCoroutine(ShieldBeAttacked(reduce));

        }
        
        if (currentFloatingText != null)
        {
            currentFloatingText.UpdateText(currentDamageCounter.ToString(), isFinalAttack);
        } 
        else
        {
            currentFloatingText = FloatingTextManager.Instance.addText(currentDamageCounter.ToString(), transform.position + new Vector3(0, 3, 0), Color.red, isFinalAttack);
        }

        if (isFinalAttack)
        {
            currentDamageCounter = 0;
            currentFloatingText = null;
        }

        if (damage < 0)
        {
            Debug.LogWarning("how damage get lower than 0");
            damage = Mathf.Max(0, damage);
        }
        hp -= damage;
        hp = math.max(hp, 0);

        if (damage > 0)
        {
            //try
            //{

                reactToDamage();
            //}
            //catch ()
            //{

            //}
        }
        
        yield return new WaitForSeconds(GridManager.animTime);
        hpbar.updateHPBar(hp, maxHP);
        if (hp <= 0)
        {
            yield return StartCoroutine( Die());
        }
    }

    public virtual void reactToDamage()
    {
        
    }

    public virtual IEnumerator ShieldBeAttacked(int amount)
    {
        yield return null;
    }

    public IEnumerator HealEnumerator(int damage)
    {

        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_effect_heal, transform.position);
        hp += damage;
        hp = Mathf.Min(hp, maxHP);
        hpbar.updateHPBar(hp, maxHP);

        FloatingTextManager.Instance.addText(damage.ToString(), transform.position, Color.green);
        yield return new WaitForSeconds(GridManager.animTime);
    }
    public IEnumerator Die()
    {
        if (!isDead)
        {
            isDead = true;
            //fx_luggage_death_blow 
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.sfx_random_boing, transform.position);
            if (currentFloatingText != null)
            {
                currentFloatingText.FlingAndDestroyText();
                currentFloatingText = null;
            }
            yield return StartCoroutine( DieInteral());
        }
    }

    protected virtual IEnumerator DieInteral()
    {
        yield return null;
    }
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        hp = maxHP;
        if (hpbar == null)
        {
            hpbar = GetComponentInChildren<HPBar>();
        }
        
        hpbar.updateHPBar(hp, maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
