using System;
using System.Collections;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    GameObject targets;
    CombatCtrl combatCtrl;
    public GameObject barlife;
    public GameObject select;
    public SpriteRenderer sr;

    float ScaleI;
    int maxLife;
    public int life;
    public int atk;
    int target;

    public bool type;

    private void Start()
    {
        ScaleI = barlife.transform.localScale.x;
        maxLife = life;
        combatCtrl = GameObject.Find("CombatCtrl").GetComponent<CombatCtrl>();
        if (type)
        {
            targets = GameObject.Find("Enemies");
        }
        else targets = GameObject.Find("Players");
    }

    public void CharacterAttack()
    {
        StartCoroutine(AnimAttack());
        if (type) target = combatCtrl.EnemySelect;
        else target = combatCtrl.PlayerSelect;
        if (combatCtrl.EnemysN >= 0 && combatCtrl.PlayersN >= 0)
                targets.transform.GetChild(target).GetComponent<CharacterScript>().Damage(atk);
    }
    
    public void CharacterHeal(int cantidad)
    {
        StartCoroutine(AnimAttack());
        target = combatCtrl.PlayerSelect;
        if (combatCtrl.EnemysN >= 0 && combatCtrl.PlayersN >= 0)
        {
            GameObject destinatario = GameObject.Find("Players").transform.GetChild(target).gameObject;

            destinatario.GetComponent<CharacterScript>().Heal(cantidad);
        }
    }
    
    private void Heal(int cantidad)
    {
        if (cantidad < (maxLife - life))
        {
            life += cantidad;
        }
        StartCoroutine(AnimHeal(cantidad));
    }

    private void Damage(int atk)
    {
        life -= atk;
        StartCoroutine(AnimDamage(atk));
        if (life <= 0)
        {
            if (type) target = combatCtrl.PlayersN--; else target = combatCtrl.EnemysN--;
            Destroy(gameObject);
        }
    }

    public void Select(bool select)
    {
        this.select.SetActive(select);
    }

    IEnumerator AnimAttack()
    {
        float mov = 0.3f;
        if (!type) mov *= -1;
        transform.position = new Vector3(transform.position.x + mov, transform.position.y, transform.position.z);
        yield return new WaitForSecondsRealtime(0.2f);
        transform.position = new Vector3(transform.position.x - mov, transform.position.y, transform.position.z);
    }

    IEnumerator AnimDamage(float damage)
    {
        barlife.transform.localScale = new Vector3(barlife.transform.localScale.x - (damage / maxLife) * ScaleI, 
            barlife.transform.localScale.y, barlife.transform.localScale.z);
        for (int i = 0; i < 6; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    
    IEnumerator AnimHeal(float cantidad)
    {
        if (cantidad > (maxLife - life))
        {
            barlife.transform.localScale = new Vector3(1f, barlife.transform.localScale.y, barlife.transform.localScale.z);
            life = maxLife;
        }
        else
        {
            barlife.transform.localScale = new Vector3(barlife.transform.localScale.x + (cantidad / maxLife) * ScaleI, 
                barlife.transform.localScale.y, barlife.transform.localScale.z);
        }
        
        float mov = 0.6f;
        for (int i = 0; i < 3; i++)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + mov, transform.position.z);
            yield return new WaitForSecondsRealtime(0.3f);
            transform.position = new Vector3(transform.position.x, transform.position.y - mov, transform.position.z);
            yield return new WaitForSecondsRealtime(0.3f);
        }
    }
}
