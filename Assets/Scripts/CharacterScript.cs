using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterScript : MonoBehaviour
{
    GameObject targets;
    CombatCtrl combatCtrl;
    public GameObject barlife;
    public GameObject select;
    public SpriteRenderer sr;

    public PersonajeData pj;

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

    public void CharacterAttack(int puntos, int idAtacante, HabilidadData hab)
    {
        StartCoroutine(AnimAttack());
        if (type) target = combatCtrl.EnemySelect;
        else target = combatCtrl.PlayerSelect;
        if (combatCtrl.EnemysN >= 0 && combatCtrl.PlayersN >= 0)
            targets.transform.GetChild(target).GetComponent<CharacterScript>().Damage(puntos);

        // asdas
        PersonajeData pjAtacante = DatabaseLoader.BuscarPersonaje(idAtacante);

        pjAtacante.energia_actual -= hab.coste;
        if (pjAtacante.energia_actual < 0) pjAtacante.energia_actual = 0;

        int idTarget = targets.transform.GetChild(target).GetComponent<CharacterScript>().pj.id;
        PersonajeData targetPj = DatabaseLoader.BuscarPersonaje(idTarget);

        if (hab.fisica == 1)
        {
            int danoRecibido = puntos - targetPj.RF_actual;

            if (danoRecibido >= 0)
            {
                targetPj.vida_actual -= danoRecibido;
                if (targetPj.vida_actual < 0) targetPj.vida_actual = 0;
            }
        }
        else
        {
            int danoRecibido = puntos - targetPj.RM_actual;

            if (danoRecibido >= 0)
            {
                targetPj.vida_actual -= danoRecibido;
                if (targetPj.vida_actual < 0) targetPj.vida_actual = 0;
            }
        }

        if (hab.enferma == 1)
        {
            int probabilidad = Random.Range(0, 100);

            if (probabilidad <= 25) targetPj.enfermo = 1;
        }

        if (hab.incapacita == 1)
        {
            int probabilidad = Random.Range(0, 100);

            if (probabilidad <= 25) targetPj.incapacitado = 1;
        }

        switch (hab.target_stat.ToUpper())
        {
            case "FF":
                if (hab.ofensiva == 1)
                {
                    targetPj.FF_actual -= (int)(targetPj.FF_base * 0.05);
                    if (targetPj.FF_actual < (targetPj.FF_base / 2)) targetPj.FF_actual = (targetPj.FF_base / 2);
                }
                else
                {
                    targetPj.FF_actual += (int)(targetPj.FF_base * 0.05);
                    if (targetPj.FF_actual > (int)(targetPj.FF_base * 1.5))
                        targetPj.FF_actual = (int)(targetPj.FF_base * 1.5);
                }

                break;

            case "RF":
                if (hab.ofensiva == 1)
                {
                    targetPj.RF_actual -= (int)(targetPj.RF_base * 0.05);
                    if (targetPj.RF_actual < (targetPj.RF_base / 2)) targetPj.RF_actual = (targetPj.RF_base / 2);
                }
                else
                {
                    targetPj.RF_actual += (int)(targetPj.RF_base * 0.05);
                    if (targetPj.RF_actual > (int)(targetPj.RF_base * 1.5))
                        targetPj.RF_actual = (int)(targetPj.RF_base * 1.5);
                }

                break;

            case "PM":
                if (hab.ofensiva == 1)
                {
                    targetPj.PM_actual -= (int)(targetPj.PM_base * 0.05);
                    if (targetPj.PM_actual < (targetPj.PM_base / 2)) targetPj.PM_actual = (targetPj.PM_base / 2);
                }
                else
                {
                    targetPj.PM_actual += (int)(targetPj.PM_base * 0.05);
                    if (targetPj.PM_actual > (int)(targetPj.PM_base * 1.5))
                        targetPj.PM_actual = (int)(targetPj.PM_base * 1.5);
                }

                break;

            case "RM":
                if (hab.ofensiva == 1)
                {
                    targetPj.RM_actual -= (int)(targetPj.RM_base * 0.05);
                    if (targetPj.RM_actual < (targetPj.RM_base / 2)) targetPj.RM_actual = (targetPj.RM_base / 2);
                }
                else
                {
                    targetPj.RM_actual += (int)(targetPj.RM_base * 0.05);
                    if (targetPj.RM_actual > (int)(targetPj.RM_base * 1.5))
                        targetPj.RM_actual = (int)(targetPj.RM_base * 1.5);
                }

                break;

            case "ALL":
                if (hab.ofensiva == 1)
                {
                    targetPj.FF_actual -= (int)(targetPj.FF_base * 0.05);
                    if (targetPj.FF_actual < (targetPj.FF_base / 2)) targetPj.FF_actual = (targetPj.FF_base / 2);

                    targetPj.RF_actual -= (int)(targetPj.RF_base * 0.05);
                    if (targetPj.RF_actual < (targetPj.RF_base / 2)) targetPj.RF_actual = (targetPj.RF_base / 2);

                    targetPj.PM_actual -= (int)(targetPj.PM_base * 0.05);
                    if (targetPj.PM_actual < (targetPj.PM_base / 2)) targetPj.PM_actual = (targetPj.PM_base / 2);

                    targetPj.RM_actual -= (int)(targetPj.RM_base * 0.05);
                    if (targetPj.RM_actual < (targetPj.RM_base / 2)) targetPj.RM_actual = (targetPj.RM_base / 2);
                }
                else
                {
                    targetPj.FF_actual += (int)(targetPj.FF_base * 0.05);
                    if (targetPj.FF_actual > (int)(targetPj.FF_base * 1.5))
                        targetPj.FF_actual = (int)(targetPj.FF_base * 1.5);

                    targetPj.RF_actual += (int)(targetPj.RF_base * 0.05);
                    if (targetPj.RF_actual > (int)(targetPj.RF_base * 1.5))
                        targetPj.RF_actual = (int)(targetPj.RF_base * 1.5);

                    targetPj.PM_actual += (int)(targetPj.PM_base * 0.05);
                    if (targetPj.PM_actual > (int)(targetPj.PM_base * 1.5))
                        targetPj.PM_actual = (int)(targetPj.PM_base * 1.5);

                    targetPj.RM_actual += (int)(targetPj.RM_base * 0.05);
                    if (targetPj.RM_actual > (int)(targetPj.RM_base * 1.5))
                        targetPj.RM_actual = (int)(targetPj.RM_base * 1.5);
                }

                break;
        }

        if (pjAtacante.id == 1 || pjAtacante.id == 2 || pjAtacante.id == 3 || pjAtacante.id == 4)
        {
            if (hab.id == pjAtacante.hab_basica_id)
            {
                if (pjAtacante.id == 1) pjAtacante.aggro += 2;
                else pjAtacante.aggro += 1;
            }
            else if (hab.id == pjAtacante.hab_secundaria_id)
            {
                if (pjAtacante.id == 1) pjAtacante.aggro += 3;
                else pjAtacante.aggro += 2;
            }
            else if (hab.id == pjAtacante.hab_especial_id)
            {
                if (pjAtacante.id == 1) pjAtacante.aggro += 4;
                else pjAtacante.aggro += 3;
            }
        }
        else
        {
            if (hab.id == pjAtacante.hab_basica_id)
            {
                if (pjAtacante.id == 11) targetPj.aggro -= 2;
                else pjAtacante.aggro -= 1;
            }
            else if (hab.id == pjAtacante.hab_secundaria_id)
            {
                if (pjAtacante.id == 11) targetPj.aggro -= 3;
                else targetPj.aggro -= 2;
            }
            else if (hab.id == pjAtacante.hab_especial_id)
            {
                if (pjAtacante.id == 11) targetPj.aggro -= 4;
                else targetPj.aggro -= 3;
            }
        }

        DatabaseLoader.ActualizarPersonaje(pjAtacante);
        DatabaseLoader.ActualizarPersonaje(targetPj);
        //assdas
    }

    public void CharacterHeal(int cantidad, int? idSanador, [CanBeNull] HabilidadData hab)
    {
        StartCoroutine(AnimAttack());
        target = combatCtrl.PlayerSelect;
        if (combatCtrl.EnemysN >= 0 && combatCtrl.PlayersN >= 0)
        {
            GameObject destinatario = GameObject.Find("Players").transform.GetChild(target).gameObject;

            destinatario.GetComponent<CharacterScript>().Heal(cantidad);
        }

        if (hab != null && idSanador != null)
        {
            PersonajeData pjSanador = DatabaseLoader.BuscarPersonaje(idSanador.Value);

            pjSanador.energia_actual -= hab.coste;
            if (pjSanador.energia_actual < 0) pjSanador.energia_actual = 0;

            int idTarget = targets.transform.GetChild(target).GetComponent<CharacterScript>().pj.id;
            PersonajeData targetPj = DatabaseLoader.BuscarPersonaje(idTarget);

            targetPj.vida_actual += cantidad;
            if (targetPj.vida_actual > targetPj.vida_max) targetPj.vida_actual = targetPj.vida_max;
            
            if (hab.enferma == 1) targetPj.enfermo = 0;

            switch (hab.target_stat.ToUpper())
            {
                case "FF":
                    if (hab.ofensiva == 1)
                    {
                        targetPj.FF_actual -= (int)(targetPj.FF_base * 0.05);
                        if (targetPj.FF_actual < (targetPj.FF_base / 2)) targetPj.FF_actual = (targetPj.FF_base / 2);
                    }
                    else
                    {
                        targetPj.FF_actual += (int)(targetPj.FF_base * 0.05);
                        if (targetPj.FF_actual > (int)(targetPj.FF_base * 1.5))
                            targetPj.FF_actual = (int)(targetPj.FF_base * 1.5);
                    }

                    break;

                case "RF":
                    if (hab.ofensiva == 1)
                    {
                        targetPj.RF_actual -= (int)(targetPj.RF_base * 0.05);
                        if (targetPj.RF_actual < (targetPj.RF_base / 2)) targetPj.RF_actual = (targetPj.RF_base / 2);
                    }
                    else
                    {
                        targetPj.RF_actual += (int)(targetPj.RF_base * 0.05);
                        if (targetPj.RF_actual > (int)(targetPj.RF_base * 1.5))
                            targetPj.RF_actual = (int)(targetPj.RF_base * 1.5);
                    }

                    break;

                case "PM":
                    if (hab.ofensiva == 1)
                    {
                        targetPj.PM_actual -= (int)(targetPj.PM_base * 0.05);
                        if (targetPj.PM_actual < (targetPj.PM_base / 2)) targetPj.PM_actual = (targetPj.PM_base / 2);
                    }
                    else
                    {
                        targetPj.PM_actual += (int)(targetPj.PM_base * 0.05);
                        if (targetPj.PM_actual > (int)(targetPj.PM_base * 1.5))
                            targetPj.PM_actual = (int)(targetPj.PM_base * 1.5);
                    }

                    break;

                case "RM":
                    if (hab.ofensiva == 1)
                    {
                        targetPj.RM_actual -= (int)(targetPj.RM_base * 0.05);
                        if (targetPj.RM_actual < (targetPj.RM_base / 2)) targetPj.RM_actual = (targetPj.RM_base / 2);
                    }
                    else
                    {
                        targetPj.RM_actual += (int)(targetPj.RM_base * 0.05);
                        if (targetPj.RM_actual > (int)(targetPj.RM_base * 1.5))
                            targetPj.RM_actual = (int)(targetPj.RM_base * 1.5);
                    }

                    break;

                case "ALL":
                    if (hab.ofensiva == 1)
                    {
                        targetPj.FF_actual -= (int)(targetPj.FF_base * 0.05);
                        if (targetPj.FF_actual < (targetPj.FF_base / 2)) targetPj.FF_actual = (targetPj.FF_base / 2);

                        targetPj.RF_actual -= (int)(targetPj.RF_base * 0.05);
                        if (targetPj.RF_actual < (targetPj.RF_base / 2)) targetPj.RF_actual = (targetPj.RF_base / 2);

                        targetPj.PM_actual -= (int)(targetPj.PM_base * 0.05);
                        if (targetPj.PM_actual < (targetPj.PM_base / 2)) targetPj.PM_actual = (targetPj.PM_base / 2);

                        targetPj.RM_actual -= (int)(targetPj.RM_base * 0.05);
                        if (targetPj.RM_actual < (targetPj.RM_base / 2)) targetPj.RM_actual = (targetPj.RM_base / 2);
                    }
                    else
                    {
                        targetPj.FF_actual += (int)(targetPj.FF_base * 0.05);
                        if (targetPj.FF_actual > (int)(targetPj.FF_base * 1.5))
                            targetPj.FF_actual = (int)(targetPj.FF_base * 1.5);

                        targetPj.RF_actual += (int)(targetPj.RF_base * 0.05);
                        if (targetPj.RF_actual > (int)(targetPj.RF_base * 1.5))
                            targetPj.RF_actual = (int)(targetPj.RF_base * 1.5);

                        targetPj.PM_actual += (int)(targetPj.PM_base * 0.05);
                        if (targetPj.PM_actual > (int)(targetPj.PM_base * 1.5))
                            targetPj.PM_actual = (int)(targetPj.PM_base * 1.5);

                        targetPj.RM_actual += (int)(targetPj.RM_base * 0.05);
                        if (targetPj.RM_actual > (int)(targetPj.RM_base * 1.5))
                            targetPj.RM_actual = (int)(targetPj.RM_base * 1.5);
                    }

                    break;
            }

            if (pjSanador.id == 1 || pjSanador.id == 2 || pjSanador.id == 3 || pjSanador.id == 4)
            {
                if (hab.id == pjSanador.hab_basica_id)
                {
                    if (pjSanador.id == 1) pjSanador.aggro += 2;
                    else pjSanador.aggro += 1;
                }
                else if (hab.id == pjSanador.hab_secundaria_id)
                {
                    if (pjSanador.id == 1) pjSanador.aggro += 3;
                    else pjSanador.aggro += 2;
                }
                else if (hab.id == pjSanador.hab_especial_id)
                {
                    if (pjSanador.id == 1) pjSanador.aggro += 4;
                    else pjSanador.aggro += 3;
                }
            }
            else
            {
                if (hab.id == pjSanador.hab_basica_id)
                {
                    if (pjSanador.id == 11) targetPj.aggro -= 2;
                    else pjSanador.aggro -= 1;
                }
                else if (hab.id == pjSanador.hab_secundaria_id)
                {
                    if (pjSanador.id == 11) targetPj.aggro -= 3;
                    else targetPj.aggro -= 2;
                }
                else if (hab.id == pjSanador.hab_especial_id)
                {
                    if (pjSanador.id == 11) targetPj.aggro -= 4;
                    else targetPj.aggro -= 3;
                }
            }

            DatabaseLoader.ActualizarPersonaje(pjSanador);
            DatabaseLoader.ActualizarPersonaje(targetPj);
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
            if (type) target = combatCtrl.PlayersN--;
            else target = combatCtrl.EnemysN--;
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
            barlife.transform.localScale =
                new Vector3(1f, barlife.transform.localScale.y, barlife.transform.localScale.z);
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


/*using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterScript : MonoBehaviour
{
    GameObject targets;
    CombatCtrl combatCtrl;
    public GameObject barlife;
    public GameObject select;
    public SpriteRenderer sr;

    float ScaleI;
    int target;

    public int id;

    public bool type;

    private void Start()
    {
        ScaleI = barlife.transform.localScale.x;
        id = DatabaseLoader.BuscarPersonaje(transform.GetSiblingIndex() + 1).id;
        combatCtrl = GameObject.Find("CombatCtrl").GetComponent<CombatCtrl>();
        if (type)
        {
            targets = GameObject.Find("Enemies");
        }
        else targets = GameObject.Find("Players");
    }

    public void CharacterAttack(int puntos, int idAtacante, HabilidadData hab)
    {
        StartCoroutine(AnimAttack());
        if (type) target = combatCtrl.EnemySelect;
        else target = combatCtrl.PlayerSelect;
        var objetivo = targets.transform.GetChild(target).GetComponent<CharacterScript>();

        PersonajeData targetPj = DatabaseLoader.BuscarPersonaje(target + 1);
        objetivo.AnimDamage(puntos, targetPj.vida_max);

        PersonajeData pjAtacante = DatabaseLoader.BuscarPersonaje(idAtacante);

        pjAtacante.energia_actual -= hab.coste;
        if (pjAtacante.energia_actual < 0) pjAtacante.energia_actual = 0;

        if (hab.fisica == 1)
        {
            int danoRecibido = puntos - targetPj.RF_actual;

            targetPj.vida_actual -= danoRecibido;
        }
        else
        {
            int danoRecibido = puntos - targetPj.RM_actual;

            targetPj.vida_actual -= danoRecibido;
        }

        targetPj.energia_actual -= hab.coste;
        if (targetPj.energia_actual < 0) targetPj.energia_actual = 0;

        if (hab.enferma == 1)
        {
            int probabilidad = Random.Range(0, 100);

            if (probabilidad <= 25) targetPj.enfermo = 1;
        }

        if (hab.incapacita == 1)
        {
            int probabilidad = Random.Range(0, 100);

            if (probabilidad <= 25) targetPj.enfermo = 1;
        }

        switch (hab.target_stat.ToUpper())
        {
            case "FF":
                if (hab.ofensiva == 1)
                {
                    targetPj.FF_actual -= (int)(targetPj.FF_base * 0.05);
                    if (targetPj.FF_actual < (targetPj.FF_base / 2)) targetPj.FF_actual = (targetPj.FF_base / 2);
                }
                else
                {
                    targetPj.FF_actual += (int)(targetPj.FF_base * 0.05);
                    if (targetPj.FF_actual > (int)(targetPj.FF_base * 1.5))
                        targetPj.FF_actual = (int)(targetPj.FF_base * 1.5);
                }

                break;

            case "RF":
                if (hab.ofensiva == 1)
                {
                    targetPj.RF_actual -= (int)(targetPj.RF_base * 0.05);
                    if (targetPj.RF_actual < (targetPj.RF_base / 2)) targetPj.RF_actual = (targetPj.RF_base / 2);
                }
                else
                {
                    targetPj.RF_actual += (int)(targetPj.RF_base * 0.05);
                    if (targetPj.RF_actual > (int)(targetPj.RF_base * 1.5))
                        targetPj.RF_actual = (int)(targetPj.RF_base * 1.5);
                }

                break;

            case "PM":
                if (hab.ofensiva == 1)
                {
                    targetPj.PM_actual -= (int)(targetPj.PM_base * 0.05);
                    if (targetPj.PM_actual < (targetPj.PM_base / 2)) targetPj.PM_actual = (targetPj.PM_base / 2);
                }
                else
                {
                    targetPj.PM_actual += (int)(targetPj.PM_base * 0.05);
                    if (targetPj.PM_actual > (int)(targetPj.PM_base * 1.5))
                        targetPj.PM_actual = (int)(targetPj.PM_base * 1.5);
                }

                break;

            case "RM":
                if (hab.ofensiva == 1)
                {
                    targetPj.RM_actual -= (int)(targetPj.RM_base * 0.05);
                    if (targetPj.RM_actual < (targetPj.RM_base / 2)) targetPj.RM_actual = (targetPj.RM_base / 2);
                }
                else
                {
                    targetPj.RM_actual += (int)(targetPj.RM_base * 0.05);
                    if (targetPj.RM_actual > (int)(targetPj.RM_base * 1.5))
                        targetPj.RM_actual = (int)(targetPj.RM_base * 1.5);
                }

                break;

            case "AGL":
                if (hab.ofensiva == 1)
                {
                    targetPj.agilidad -= (int)(targetPj.agilidad * 0.05);
                    if (targetPj.agilidad < (targetPj.agilidad / 2)) targetPj.agilidad = (targetPj.agilidad / 2);
                }
                else
                {
                    targetPj.agilidad += (int)(targetPj.agilidad * 0.05);
                    if (targetPj.agilidad > (int)(targetPj.agilidad * 1.5))
                        targetPj.agilidad = (int)(targetPj.agilidad * 1.5);
                }

                break;

            case "ALL":
                if (hab.ofensiva == 1)
                {
                    targetPj.FF_actual -= (int)(targetPj.FF_base * 0.05);
                    if (targetPj.FF_actual < (targetPj.FF_base / 2)) targetPj.FF_actual = (targetPj.FF_base / 2);

                    targetPj.RF_actual -= (int)(targetPj.RF_base * 0.05);
                    if (targetPj.RF_actual < (targetPj.RF_base / 2)) targetPj.RF_actual = (targetPj.RF_base / 2);

                    targetPj.PM_actual -= (int)(targetPj.PM_base * 0.05);
                    if (targetPj.PM_actual < (targetPj.PM_base / 2)) targetPj.PM_actual = (targetPj.PM_base / 2);

                    targetPj.RM_actual -= (int)(targetPj.RM_base * 0.05);
                    if (targetPj.RM_actual < (targetPj.RM_base / 2)) targetPj.RM_actual = (targetPj.RM_base / 2);

                    targetPj.agilidad -= (int)(targetPj.agilidad * 0.05);
                    if (targetPj.agilidad < (targetPj.agilidad / 2)) targetPj.agilidad = (targetPj.agilidad / 2);
                }
                else
                {
                    targetPj.FF_actual += (int)(targetPj.FF_base * 0.05);
                    if (targetPj.FF_actual > (int)(targetPj.FF_base * 1.5))
                        targetPj.FF_actual = (int)(targetPj.FF_base * 1.5);

                    targetPj.RF_actual += (int)(targetPj.RF_base * 0.05);
                    if (targetPj.RF_actual > (int)(targetPj.RF_base * 1.5))
                        targetPj.RF_actual = (int)(targetPj.RF_base * 1.5);

                    targetPj.PM_actual += (int)(targetPj.PM_base * 0.05);
                    if (targetPj.PM_actual > (int)(targetPj.PM_base * 1.5))
                        targetPj.PM_actual = (int)(targetPj.PM_base * 1.5);

                    targetPj.RM_actual += (int)(targetPj.RM_base * 0.05);
                    if (targetPj.RM_actual > (int)(targetPj.RM_base * 1.5))
                        targetPj.RM_actual = (int)(targetPj.RM_base * 1.5);

                    targetPj.agilidad += (int)(targetPj.agilidad * 0.05);
                    if (targetPj.agilidad > (int)(targetPj.agilidad * 1.5))
                        targetPj.agilidad = (int)(targetPj.agilidad * 1.5);
                }

                break;
        }

        if (pjAtacante.id == 1 || pjAtacante.id == 2 || pjAtacante.id == 3 || pjAtacante.id == 4)
        {
            if (hab.id == pjAtacante.hab_basica_id)
            {
                if (pjAtacante.id == 1) pjAtacante.aggro += 2;
                else pjAtacante.aggro += 1;
            }
            else if (hab.id == pjAtacante.hab_secundaria_id)
            {
                if (pjAtacante.id == 1) pjAtacante.aggro += 3;
                else pjAtacante.aggro += 2;
            }
            else if (hab.id == pjAtacante.hab_especial_id)
            {
                if (pjAtacante.id == 1) pjAtacante.aggro += 4;
                else pjAtacante.aggro += 3;
            }
        }
        else
        {
            if (hab.id == pjAtacante.hab_basica_id)
            {
                if (pjAtacante.id == 11) targetPj.aggro -= 2;
                else pjAtacante.aggro -= 1;
            }
            else if (hab.id == pjAtacante.hab_secundaria_id)
            {
                if (pjAtacante.id == 11) targetPj.aggro -= 3;
                else targetPj.aggro -= 2;
            }
            else if (hab.id == pjAtacante.hab_especial_id)
            {
                if (pjAtacante.id == 11) targetPj.aggro -= 4;
                else targetPj.aggro -= 3;
            }
        }


        StartCoroutine(AnimDamage(puntos, targetPj.energia_max));


        if (targetPj.energia_actual <= 0)
        {
            if (type) target = combatCtrl.PlayersN--;
            else target = combatCtrl.EnemysN--;
            Destroy(gameObject);
        }

        DatabaseLoader.ActualizarPersonaje(targetPj);
        DatabaseLoader.ActualizarPersonaje(pjAtacante);
    }

    public void CharacterHeal(int puntos, int? idSanador, [CanBeNull] HabilidadData hab, [CanBeNull] ConsumibleData con)
    {
        StartCoroutine(AnimAttack());
        target = combatCtrl.PlayerSelect;

        PersonajeData targetPj = DatabaseLoader.BuscarPersonaje(target + 1);
        PersonajeData pjSanador = idSanador.HasValue
            ? DatabaseLoader.BuscarPersonaje(idSanador.Value)
            : null;

        if (hab != null)
        {
            pjSanador.energia_actual -= hab.coste;
            if (pjSanador.energia_actual < 0) pjSanador.energia_actual = 0;

            if (hab.vida == 1)
            {
                targetPj.vida_actual += puntos;
                if (targetPj.vida_actual > targetPj.vida_max) targetPj.vida_actual = targetPj.vida_max;
            }
            else
            {
                targetPj.energia_actual += puntos;
                if (targetPj.energia_actual > targetPj.energia_max) targetPj.energia_actual = targetPj.energia_max;
            }


            if (targetPj.enfermo == 1) targetPj.enfermo = 0;

            switch (hab.target_stat.ToUpper())
            {
                case "FF":
                    if (hab.ofensiva == 1)
                    {
                        targetPj.FF_actual -= (int)(targetPj.FF_base * 0.05);
                        if (targetPj.FF_actual < (targetPj.FF_base / 2)) targetPj.FF_actual = (targetPj.FF_base / 2);
                    }
                    else
                    {
                        targetPj.FF_actual += (int)(targetPj.FF_base * 0.05);
                        if (targetPj.FF_actual > (int)(targetPj.FF_base * 1.5))
                            targetPj.FF_actual = (int)(targetPj.FF_base * 1.5);
                    }

                    break;

                case "RF":
                    if (hab.ofensiva == 1)
                    {
                        targetPj.RF_actual -= (int)(targetPj.RF_base * 0.05);
                        if (targetPj.RF_actual < (targetPj.RF_base / 2)) targetPj.RF_actual = (targetPj.RF_base / 2);
                    }
                    else
                    {
                        targetPj.RF_actual += (int)(targetPj.RF_base * 0.05);
                        if (targetPj.RF_actual > (int)(targetPj.RF_base * 1.5))
                            targetPj.RF_actual = (int)(targetPj.RF_base * 1.5);
                    }

                    break;

                case "PM":
                    if (hab.ofensiva == 1)
                    {
                        targetPj.PM_actual -= (int)(targetPj.PM_base * 0.05);
                        if (targetPj.PM_actual < (targetPj.PM_base / 2)) targetPj.PM_actual = (targetPj.PM_base / 2);
                    }
                    else
                    {
                        targetPj.PM_actual += (int)(targetPj.PM_base * 0.05);
                        if (targetPj.PM_actual > (int)(targetPj.PM_base * 1.5))
                            targetPj.PM_actual = (int)(targetPj.PM_base * 1.5);
                    }

                    break;

                case "RM":
                    if (hab.ofensiva == 1)
                    {
                        targetPj.RM_actual -= (int)(targetPj.RM_base * 0.05);
                        if (targetPj.RM_actual < (targetPj.RM_base / 2)) targetPj.RM_actual = (targetPj.RM_base / 2);
                    }
                    else
                    {
                        targetPj.RM_actual += (int)(targetPj.RM_base * 0.05);
                        if (targetPj.RM_actual > (int)(targetPj.RM_base * 1.5))
                            targetPj.RM_actual = (int)(targetPj.RM_base * 1.5);
                    }

                    break;

                case "AGL":
                    if (hab.ofensiva == 1)
                    {
                        targetPj.agilidad -= (int)(targetPj.agilidad * 0.05);
                        if (targetPj.agilidad < (targetPj.agilidad / 2)) targetPj.agilidad = (targetPj.agilidad / 2);
                    }
                    else
                    {
                        targetPj.agilidad += (int)(targetPj.agilidad * 0.05);
                        if (targetPj.agilidad > (int)(targetPj.agilidad * 1.5))
                            targetPj.agilidad = (int)(targetPj.agilidad * 1.5);
                    }

                    break;

                case "ALL":
                    if (hab.ofensiva == 1)
                    {
                        targetPj.FF_actual -= (int)(targetPj.FF_base * 0.05);
                        if (targetPj.FF_actual < (targetPj.FF_base / 2)) targetPj.FF_actual = (targetPj.FF_base / 2);

                        targetPj.RF_actual -= (int)(targetPj.RF_base * 0.05);
                        if (targetPj.RF_actual < (targetPj.RF_base / 2)) targetPj.RF_actual = (targetPj.RF_base / 2);

                        targetPj.PM_actual -= (int)(targetPj.PM_base * 0.05);
                        if (targetPj.PM_actual < (targetPj.PM_base / 2)) targetPj.PM_actual = (targetPj.PM_base / 2);

                        targetPj.RM_actual -= (int)(targetPj.RM_base * 0.05);
                        if (targetPj.RM_actual < (targetPj.RM_base / 2)) targetPj.RM_actual = (targetPj.RM_base / 2);

                        targetPj.agilidad -= (int)(targetPj.agilidad * 0.05);
                        if (targetPj.agilidad < (targetPj.agilidad / 2)) targetPj.agilidad = (targetPj.agilidad / 2);
                    }
                    else
                    {
                        targetPj.FF_actual += (int)(targetPj.FF_base * 0.05);
                        if (targetPj.FF_actual > (int)(targetPj.FF_base * 1.5))
                            targetPj.FF_actual = (int)(targetPj.FF_base * 1.5);

                        targetPj.RF_actual += (int)(targetPj.RF_base * 0.05);
                        if (targetPj.RF_actual > (int)(targetPj.RF_base * 1.5))
                            targetPj.RF_actual = (int)(targetPj.RF_base * 1.5);

                        targetPj.PM_actual += (int)(targetPj.PM_base * 0.05);
                        if (targetPj.PM_actual > (int)(targetPj.PM_base * 1.5))
                            targetPj.PM_actual = (int)(targetPj.PM_base * 1.5);

                        targetPj.RM_actual += (int)(targetPj.RM_base * 0.05);
                        if (targetPj.RM_actual > (int)(targetPj.RM_base * 1.5))
                            targetPj.RM_actual = (int)(targetPj.RM_base * 1.5);

                        targetPj.agilidad += (int)(targetPj.agilidad * 0.05);
                        if (targetPj.agilidad > (int)(targetPj.agilidad * 1.5))
                            targetPj.agilidad = (int)(targetPj.agilidad * 1.5);
                    }

                    break;
            }

            DatabaseLoader.ActualizarPersonaje(targetPj);
            DatabaseLoader.ActualizarPersonaje(pjSanador);
        }
        else if (con != null)
        {
            if (con.vida == 1)
            {
                targetPj.vida_actual += con.cantidad_recuperada;
                if (targetPj.vida_actual > targetPj.vida_max) targetPj.vida_actual = targetPj.vida_max;
            }
            else
            {
                targetPj.energia_actual += con.cantidad_recuperada;
                if (targetPj.energia_actual > targetPj.energia_max) targetPj.energia_actual = targetPj.energia_max;
            }

            if (con.enfermedad == 1)
            {
                targetPj.enfermo = 0;
            }

            DatabaseLoader.ActualizarPersonaje(targetPj);
            DatabaseLoader.ActualizarCantidadConsumible(con.id, false);
        }

        StartCoroutine(AnimHeal(puntos, targetPj.energia_max, targetPj.vida_actual));
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

    IEnumerator AnimDamage(float damage, int maxLife)
    {
        barlife.transform.localScale = new Vector3(barlife.transform.localScale.x - (damage / maxLife) * ScaleI,
            barlife.transform.localScale.y, barlife.transform.localScale.z);
        for (int i = 0; i < 6; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    IEnumerator AnimHeal(float cantidad, int maxLife, int life)
    {
        if (cantidad > (maxLife - life))
        {
            barlife.transform.localScale =
                new Vector3(1f, barlife.transform.localScale.y, barlife.transform.localScale.z);
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
}*/