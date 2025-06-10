using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Xsl;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

public class CombatCtrl : MonoBehaviour
{
    public int EnemysN = 2, PlayersN = 1, ButtonsN = 3;
    public int EnemySelect, PlayerSelect, ButtonSelect;
    public GameObject enemies, players, buttons;
    private int CurrentPlayer;
    private bool turn = true;
    private bool habSelected = false;
    private bool consumibles = false;
    private bool conSelected = false;
    private bool sanacion = false;
    public MenuHabilidadesUI menuHabilidadesUI;

    public ScrollRect scrollView;

    private void Start()
    {
        PlayerSelect = 0;
        CharacterScript statsP = players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>();
        statsP.Select(true);
        menuHabilidadesUI.MostrarHabilidades(PlayerSelect + 1);

        CharacterScript statsE = enemies.transform.GetChild(EnemySelect).GetComponent<CharacterScript>();
        statsE.Select(false);

        var button = buttons.transform.GetChild(ButtonSelect).GetComponent<Button>();
        var colors = button.colors;
        button.targetGraphic.color = colors.pressedColor;

        for (int i = 0; i < players.transform.childCount; i++)
        {
            CharacterScript cs = players.transform.GetChild(i).GetComponent<CharacterScript>();
            cs.pj = DatabaseLoader.AsignarPersonajes(true, null)[i];
        }

        for (int i = 0; i < enemies.transform.childCount; i++)
        {
            CharacterScript cs = enemies.transform.GetChild(i).GetComponent<CharacterScript>();
            if (enemies.transform.childCount == 3)
            {
                cs.pj = DatabaseLoader.AsignarPersonajes(false, true)[i];
            }
            else
            {
                cs.pj = DatabaseLoader.AsignarPersonajes(false, false)[i];
            }
        }
    }

    private void Update()
    {
        if (PlayersN == 0)
        {
        }
        else if (EnemysN == 0)
        {
        }
        else
        {
            if (turn)
            {
                if (!habSelected)
                {
                    enemies.transform.GetChild(EnemySelect).GetComponent<CharacterScript>().Select(false);
                    var button = buttons.transform.GetChild(ButtonSelect).GetComponent<Button>();
                    var colors = button.colors;
                    button.targetGraphic.color = colors.normalColor;
                    if (Input.GetKeyDown(KeyCode.DownArrow)) ButtonSelect++;
                    if (Input.GetKeyDown(KeyCode.UpArrow)) ButtonSelect--;
                    ButtonsN = menuHabilidadesUI.contenedorHabilidades.transform.childCount - 1;
                    ButtonSelect = Mathf.Clamp(ButtonSelect, 0, ButtonsN);
                    button = buttons.transform.GetChild(ButtonSelect).GetComponent<Button>();
                    button.targetGraphic.color = colors.pressedColor;

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        if (ButtonSelect == 3)
                        {
                            menuHabilidadesUI.OcultarHabilidades();
                            menuHabilidadesUI.MostrarConsumibles();

                            consumibles = true;
                        }
                        else
                        {
                            int idHabilidad = buttons.transform.GetChild(ButtonSelect).GetComponent<BotonHabilidad>()
                                .idHabilidad;
                            if (idHabilidad == 5 || idHabilidad == 10 || idHabilidad == 11)
                            {
                                CurrentPlayer = PlayerSelect;
                                PlayerSelect = 0;
                                sanacion = true;
                            }
                        }

                        habSelected = true;
                    }
                }
                else if (habSelected && sanacion)
                {
                    players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>().Select(false);
                    if (Input.GetKeyDown(KeyCode.DownArrow)) PlayerSelect--;
                    if (Input.GetKeyDown(KeyCode.UpArrow)) PlayerSelect++;
                    PlayerSelect = Mathf.Clamp(PlayerSelect, 0, PlayersN);
                    players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>().Select(true);

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        habSelected = false;
                        consumibles = false;
                        conSelected = false;
                        sanacion = false;
                        players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>().Select(false);
                        int idHabilidad = buttons.transform.GetChild(ButtonSelect).GetComponent<BotonHabilidad>()
                            .idHabilidad;
                        UsarHabilidad(DatabaseLoader.BuscarHabilidad(idHabilidad));
                    }
                    else if (Input.GetKeyUp(KeyCode.Escape))
                    {
                        habSelected = false;
                        sanacion = false;
                        players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>().Select(false);
                        PlayerSelect = CurrentPlayer;
                    }
                }
                else if (habSelected && consumibles && !conSelected)
                {
                    ButtonSelect = 0;

                    players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>().Select(false);
                    var button = buttons.transform.GetChild(ButtonSelect).GetComponent<Button>();
                    var colors = button.colors;
                    button.targetGraphic.color = colors.normalColor;
                    if (Input.GetKeyDown(KeyCode.DownArrow)) ButtonSelect++;
                    if (Input.GetKeyDown(KeyCode.UpArrow)) ButtonSelect--;
                    ButtonsN = menuHabilidadesUI.contenedorConsumibles.transform.childCount - 1;
                    ButtonSelect = Mathf.Clamp(ButtonSelect, 0, ButtonsN);
                    button = buttons.transform.GetChild(ButtonSelect).GetComponent<Button>();
                    button.targetGraphic.color = colors.pressedColor;

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        conSelected = true;
                        CurrentPlayer = PlayerSelect;
                        PlayerSelect = 0;
                    }
                    else if (Input.GetKeyUp(KeyCode.Escape))
                    {
                        habSelected = false;
                        consumibles = false;

                        menuHabilidadesUI.OcultarConsumibles();
                        menuHabilidadesUI.MostrarHabilidades(PlayerSelect + 1);
                        players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>().Select(true);
                    }
                }
                else if (habSelected && consumibles && conSelected)
                {
                    players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>().Select(false);
                    if (Input.GetKeyDown(KeyCode.DownArrow)) PlayerSelect--;
                    if (Input.GetKeyDown(KeyCode.UpArrow)) PlayerSelect++;
                    PlayerSelect = Mathf.Clamp(PlayerSelect, 0, PlayersN);
                    players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>().Select(true);

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        habSelected = false;
                        consumibles = false;
                        conSelected = false;
                        players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>().Select(false);
                        int idConsumible = buttons.transform.GetChild(ButtonSelect).GetComponent<BotonConsumible>()
                            .idConsumible;
                        UsarConsumible(DatabaseLoader.BuscarConsumible(idConsumible));
                        DatabaseLoader.ActualizarCantidadConsumible((ButtonSelect + 1), false);
                    }
                    else if (Input.GetKeyUp(KeyCode.Escape))
                    {
                        conSelected = false;
                        players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>().Select(false);
                        PlayerSelect = CurrentPlayer;
                    }
                }
                else if (habSelected)
                {
                    int idHabilidad = buttons.transform.GetChild(ButtonSelect).GetComponent<BotonHabilidad>()
                        .idHabilidad;
                    HabilidadData hab = DatabaseLoader.BuscarHabilidad(idHabilidad);

                    if (hab.objetivo.ToUpper() == "AUTO" || hab.objetivo.ToUpper() == "AREA")
                    {
                        UsarHabilidad(hab);
                    }
                    else
                    {
                        enemies.transform.GetChild(EnemySelect).GetComponent<CharacterScript>().Select(false);
                        if (Input.GetKeyDown(KeyCode.DownArrow)) EnemySelect--;
                        if (Input.GetKeyDown(KeyCode.UpArrow)) EnemySelect++;
                        EnemySelect = Mathf.Clamp(EnemySelect, 0, EnemysN);
                        enemies.transform.GetChild(EnemySelect).GetComponent<CharacterScript>().Select(true);

                        if (Input.GetKeyDown(KeyCode.Return)) UsarHabilidad(hab);
                        else if (Input.GetKeyUp(KeyCode.Escape))
                        {
                            habSelected = false;
                            enemies.transform.GetChild(EnemySelect).GetComponent<CharacterScript>().Select(false);
                            EnemySelect = 0;
                        }
                    }
                }
            }
        }
    }

    //public void

    public void UsarHabilidad(HabilidadData hab)
    {
        int idPj = players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>().pj.id;
        PersonajeData pj = DatabaseLoader.BuscarPersonaje(idPj);


        int puntosBase = 0;
        int totalPuntos = 0;


        if (hab.fisica == 1)
        {
            for (int i = 0; i < hab.cantidad; i++)
            {
                puntosBase += Random.Range(0, pj.FF_actual + 1);
            }

            totalPuntos = puntosBase + pj.FF_actual;
        }
        else if (hab.fisica == 0)
        {
            for (int i = 0; i < hab.cantidad; i++)
            {
                puntosBase += Random.Range(0, pj.PM_actual + 1);
            }

            totalPuntos = puntosBase + pj.PM_actual;
        }

        if (hab.dano == 1)
        {
            if (turn && PlayersN >= 0)
            {
                CharacterScript cs = players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>();
                if (hab.objetivo.ToUpper() == "AREA")
                {
                    int currentE = EnemySelect;
                    for (int z = 0; z < enemies.transform.childCount; z++)
                    {
                        EnemySelect = z;
                        cs.CharacterAttack(totalPuntos, idPj, hab);
                    }

                    EnemySelect = currentE;
                }
                else
                {
                    cs.CharacterAttack(totalPuntos, idPj, hab);
                }

                cs.Select(false);
                menuHabilidadesUI.OcultarHabilidades();

                if (PlayerSelect == PlayersN)
                {
                    PlayerSelect = 0;
                    turn = false;
                    StartCoroutine(AttackE());
                    menuHabilidadesUI.OcultarHabilidades();
                }
                else
                {
                    PlayerSelect++;
                    cs = players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>();
                    cs.Select(true);
                    habSelected = false;
                    menuHabilidadesUI.MostrarHabilidades(PlayerSelect + 1);
                }
            }

            enemies.transform.GetChild(EnemySelect).GetComponent<CharacterScript>().Select(false);
            EnemySelect = 0;
        }
        else
        {
            CharacterScript currentCs = players.transform.GetChild(CurrentPlayer).GetComponent<CharacterScript>();
            CharacterScript selectedCs = players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>();

            if (hab.objetivo.ToUpper() == "AREA")
            {
                int currentP = PlayerSelect;

                for (int z = 0; z < enemies.transform.childCount; z++)
                {
                    PlayerSelect = z;
                    selectedCs = players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>();
                    currentCs.CharacterHeal(totalPuntos, idPj, hab);
                }

                currentCs.CharacterHeal(totalPuntos, idPj, hab);
                PlayerSelect = currentP;
            }
            else
            {
                if (hab.objetivo.ToUpper() == "AUTO")
                {
                    selectedCs = currentCs;
                    totalPuntos = 0;
                }

                currentCs.CharacterHeal(totalPuntos, currentCs.pj.id, hab);
            }

            currentCs.Select(false);
            selectedCs.Select(false);
            PlayerSelect = CurrentPlayer;
            menuHabilidadesUI.OcultarHabilidades();

            if (PlayerSelect == PlayersN)
            {
                PlayerSelect = 0;
                turn = false;
                StartCoroutine(AttackE());
                menuHabilidadesUI.OcultarHabilidades();
            }
            else
            {
                PlayerSelect++;
                selectedCs.Select(true);
                habSelected = false;
                menuHabilidadesUI.MostrarHabilidades(PlayerSelect + 1);
            }
        }
    }

    public void UsarConsumible(ConsumibleData con)
    {
        CharacterScript currentCs = players.transform.GetChild(CurrentPlayer).GetComponent<CharacterScript>();
        CharacterScript selectedCs = players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>();

        currentCs.CharacterHeal(con.cantidad_recuperada, null, null);

        currentCs.Select(false);
        selectedCs.Select(false);
        menuHabilidadesUI.OcultarConsumibles();

        PlayerSelect = CurrentPlayer;

        if (PlayerSelect == PlayersN)
        {
            PlayerSelect = 0;
            turn = false;
            StartCoroutine(AttackE());
        }
        else
        {
            PlayerSelect++;
            currentCs = players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>();
            currentCs.Select(true);
            habSelected = false;
            menuHabilidadesUI.MostrarHabilidades(PlayerSelect + 1);
        }
    }

    IEnumerator AttackE()
    {
        enemies.transform.GetChild(EnemySelect).GetComponent<CharacterScript>().Select(false);

        for (int i = 0; i < enemies.transform.childCount; i++)
        {
            int aggroTotal = 0;

            for (int j = 0; j < players.transform.childCount; j++)
            {
                GameObject player = players.transform.GetChild(j).gameObject;
                CharacterScript cs = player.GetComponent<CharacterScript>();
                aggroTotal += cs.pj.aggro;
            }

            double porcentaje = 100 / aggroTotal;

            List<int> aggroList = new List<int>();

            for (int j = 0; j < players.transform.childCount; j++)
            {
                GameObject player = players.transform.GetChild(j).gameObject;
                CharacterScript cs = player.GetComponent<CharacterScript>();

                aggroList.Add((int)Math.Round(porcentaje * cs.pj.aggro));
            }

            int num = Random.Range(0, 101);
            int totalRedondeos = 0;

            for (int k = 0; k < aggroList.Count; k++)
            {
                totalRedondeos += aggroList[k];
                if (k == 0)
                {
                    if (num > 0 && num <= totalRedondeos)
                    {
                        PlayerSelect = 0;
                    }
                }
                else
                {
                    if (num > (totalRedondeos - aggroList[k]) && num <= totalRedondeos)
                    {
                        PlayerSelect = k;
                    }
                }
            }

            yield return new WaitForSecondsRealtime(1f);

            CharacterScript csE = enemies.transform.GetChild(i).GetComponent<CharacterScript>();
            int idHabilidad;
            if (csE.pj.id == 9)
            {
                idHabilidad = Random.Range(15, 19);
            }
            else
            {
                idHabilidad = csE.pj.hab_basica_id;
            }

            HabilidadData hab = DatabaseLoader.BuscarHabilidad(idHabilidad);

            int puntosBase = 0;
            int totalPuntos = 0;

            if (hab.fisica == 1)
            {
                for (int z = 0; z < hab.cantidad; z++)
                {
                    puntosBase += Random.Range(0, csE.pj.FF_actual + 1);
                }

                totalPuntos = puntosBase + csE.pj.FF_actual;
            }
            else if (hab.fisica == 0)
            {
                for (int z = 0; z < hab.cantidad; z++)
                {
                    puntosBase += Random.Range(0, csE.pj.PM_actual + 1);
                }

                totalPuntos = puntosBase + csE.pj.PM_actual;
            }

            if (hab.objetivo.ToUpper() == "AREA")
            {
                int currentE = EnemySelect;
                for (int z = 0; z < enemies.transform.childCount; z++)
                {
                    EnemySelect = z;
                    csE.CharacterAttack(totalPuntos, csE.pj.id, hab);
                }

                EnemySelect = currentE;
            }
            else
            {
                csE.CharacterAttack(totalPuntos, csE.pj.id, hab);
            }

            csE.CharacterAttack(0, csE.pj.id, hab);
            yield return new WaitForSecondsRealtime(1f);
        }

        PlayerSelect = 0;
        turn = true;
        habSelected = false;
        players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>().Select(true);
        menuHabilidadesUI.MostrarHabilidades(PlayerSelect + 1);
    }
}