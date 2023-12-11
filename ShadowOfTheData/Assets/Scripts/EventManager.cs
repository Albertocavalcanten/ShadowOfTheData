//Script de funções do Gerenciador de Eventos
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    //Elementos envolvidos nos eventos
    Backgroundmusic backgroundMusic;
    PlayerController player;
    TerminalPuzzle[] terminalPuzzle;
    UIManager uiManager;

    public bool canPlay = false; //booleano de permissão para o jogador se movimentar e o tempo contar

    void Start()
    {
        backgroundMusic = FindObjectOfType<Backgroundmusic>();
        player = FindObjectOfType<PlayerController>();
        terminalPuzzle = FindObjectsOfType<TerminalPuzzle>();
        uiManager = FindObjectOfType<UIManager>();

        //Para os seguintes comandos abaixo:
        //quando o evento TerminalAccessed é chamado, a função OnTerminalAccessed é executada em seus scripts destinos
        //o mesmo vale para os eventos FadeFinished, LevelAccessed e GameFinished
        foreach(TerminalPuzzle terminal in terminalPuzzle)
        {
            terminal.TerminalAccessed += uiManager.OnTerminalAccessed;
            terminal.TerminalAccessed += backgroundMusic.OnTerminalAccessed;
            uiManager.FadeFinished += terminal.OnFadeFinished;
        }

        uiManager.LevelAccessed += backgroundMusic.OnLevelAccessed;
        uiManager.LevelAccessed += player.OnLevelAccessed;
        uiManager.GameFinished += backgroundMusic.OnGameFinished;
    }
}
