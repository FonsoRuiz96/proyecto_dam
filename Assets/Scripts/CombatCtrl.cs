using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    private void Update()
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

                    habSelected = true;
                    ButtonSelect = 0;
                }
            }
            else if (habSelected && consumibles && !conSelected)
            {
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
                } else if (Input.GetKeyUp(KeyCode.Escape))
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
                    UsarHabilidad(false);
                    DatabaseLoader.ActualizarCantidadConsumible((ButtonSelect + 1), false);
                }
                else if (Input.GetKeyUp(KeyCode.Escape))
                {
                    conSelected = false;
                    players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>().Select(false);
                    PlayerSelect = CurrentPlayer;
                }
            }
            else if (EnemysN >= 0)
            {
                enemies.transform.GetChild(EnemySelect).GetComponent<CharacterScript>().Select(false);
                if (Input.GetKeyDown(KeyCode.DownArrow)) EnemySelect--;
                if (Input.GetKeyDown(KeyCode.UpArrow)) EnemySelect++;
                EnemySelect = Mathf.Clamp(EnemySelect, 0, EnemysN);
                enemies.transform.GetChild(EnemySelect).GetComponent<CharacterScript>().Select(true);

                if (Input.GetKeyDown(KeyCode.Return)) UsarHabilidad(true);
                else if (Input.GetKeyUp(KeyCode.Escape))
                {
                    habSelected = false;
                    enemies.transform.GetChild(EnemySelect).GetComponent<CharacterScript>().Select(false);
                    EnemySelect = 0;
                }
            }
        }
    }

    //public void

    public void UsarHabilidad(bool damage)
    {
        if (damage)
        {
            if (turn && PlayersN >= 0)
            {
                CharacterScript cs = players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>();
                cs.CharacterAttack();

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
            
            List<ConsumibleData> consumibles = DatabaseLoader.ListaConsumibles(false);

            currentCs.CharacterHeal(consumibles[ButtonSelect].cantidad_recuperada);

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
    }

    IEnumerator AttackE()
    {
        enemies.transform.GetChild(EnemySelect).GetComponent<CharacterScript>().Select(false);
        if (EnemysN >= 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            for (int i = 0; i < enemies.transform.childCount; i++)
            {
                PlayerSelect = Random.Range(0, (PlayersN + 1));
                enemies.transform.GetChild(i).GetComponent<CharacterScript>().CharacterAttack();
                yield return new WaitForSecondsRealtime(1f);
            }
        }

        PlayerSelect = 0;
        turn = true;
        habSelected = false;
        players.transform.GetChild(PlayerSelect).GetComponent<CharacterScript>().Select(true);
        menuHabilidadesUI.MostrarHabilidades(PlayerSelect + 1);
    }
}