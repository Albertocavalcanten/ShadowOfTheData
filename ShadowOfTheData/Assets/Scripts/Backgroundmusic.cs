//Script de funções de Música de Fundo
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backgroundmusic : MonoBehaviour
{
    private AudioSource music;
    private const float volumeControl = 0.5f;

    public static Backgroundmusic instance;

    private void Awake()
    {
        //Apenas uma instancia de Backgroundmusic será permitida ao longo do jogo
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
     music = GetComponent<AudioSource>();
     music.volume = PlayerPrefs.GetFloat("volume"); //configuração de volume
     music.Play();
    }

    //função que se comunica com o script TerminalPuzzle através do script EventManager
    //o evento é chamado inicialmente na função Update() no script TerminalPuzzle
    public void OnTerminalAccessed()
    {
        music.volume = PlayerPrefs.GetFloat("volume") * volumeControl;
    }

    //função que se comunica com o script UIManager através do script EventManager
    //o evento é chamado inicialmente na função OnFadeInComplete() no script UIManager
    public void OnLevelAccessed()
    {
        music.volume = PlayerPrefs.GetFloat("volume");
    }

    //função que se comunica com o script UIManager através do script EventManager
    //o evento é chamado inicialmente na função OnFadeOutComplete() no script UIManager
    public void OnGameFinished()
    {
        Destroy(gameObject);
    }
}