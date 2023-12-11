//Script de funções dos Terminais de Puzzle
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TerminalPuzzle : MonoBehaviour
{
    [SerializeField] private string nextPuzzle;
    private string selectedPuzzle;
    private Animator anim;
    private bool allowed = false; //booleano de permissão para acessar o terminal

    //Configuração de eventos do terminal
    public delegate void TerminalAcessedManager();
    public event TerminalAcessedManager TerminalAccessed;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(Input.GetKeyDown("e") && allowed)
        {
            selectedPuzzle = nextPuzzle;
            TerminalAccessed?.Invoke(); //evento iniciado e sua comunicação é feita no script EventManager
        }
    }

    //função de verificação de colisão
    //caso o jogador entre na aréa de colisão do terminal, é permitido acessá-lo 
    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            anim.SetBool("ShowAccess", true);
            allowed = true;
        }
    }

    //função de verificação de colisão
    //caso o jogador saia da aréa de colisão do terminal, não é mais permitido acessá-lo
    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            anim.SetBool("ShowAccess", false);
            allowed = false;
        }
    }

    //função que se comunica com o script UIManager através do script EventManager
    //o evento é chamado inicialmente na função OnFadeOutComplete() no script UIManager
    public void OnFadeFinished()
    {
        if(!string.IsNullOrEmpty(selectedPuzzle))
        {
            SceneManager.LoadScene(selectedPuzzle);
        }
    }
}