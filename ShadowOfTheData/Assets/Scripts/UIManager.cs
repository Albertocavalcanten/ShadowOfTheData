//Script de funções do Gerenciador de UI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public EventManager eventManagerRef; //referência à classe EventManager para gerenciamento de eventos do jogo
    [SerializeField] private GameObject pause;
    [SerializeField] private TMP_Text messageTitle;
    [SerializeField] private TMP_Text messageContent;
    private TMP_Text timerText;
    private float currentTime;
    private Animator anim;
    private int typeFadeOut; //tipo de Fade Out realizado
    private const int typePuzzle = 0; //Fade Out para o Puzzle
    private const int typeMainMenu = 1; //Fade Out para o Menu Principal

    //configuração de eventos do gerenciador de UI
    public delegate void FadeHandler();
    public event FadeHandler FadeFinished;
    public delegate void LevelHandler();
    public event LevelHandler LevelAccessed;
    public delegate void GameStatus();
    public event GameStatus GameFinished;

    void Start()
    {
        currentTime = PlayerPrefs.GetFloat("timer");
        PlayerPrefs.SetString("currentLevel", SceneManager.GetActiveScene().name);
        timerText = GetComponentInChildren<TMP_Text>();
        anim = GetComponentInChildren<Animator>();

        if(SceneManager.GetActiveScene().name == "Level0") //verificação para caso de vitória
        {
            Win();
        }
    }

    void Update()
    {
        if(eventManagerRef.canPlay) //o tempo só poderá contar se o atributo canPlay for verdadeiro
        {
            TimeCount();
        }

        if(Input.GetKeyDown("space"))
        {
            Pause();
        }
    }

    void TimeCount()
    {
        currentTime -= Time.deltaTime;
        timerText.text = "Tempo restante: " + (int)currentTime;
        if (currentTime <= 0)
        {
            GameOver();
        }
    }

    void Pause()
    {
        eventManagerRef.canPlay = !eventManagerRef.canPlay;
        timerText.gameObject.SetActive(eventManagerRef.canPlay);
        pause.gameObject.SetActive(!eventManagerRef.canPlay);
        PlayerPrefs.SetFloat("timer", currentTime);
    }

    //função executada sempre que o jogador entra em um level
    void OnFadeInComplete()
    {
        eventManagerRef.canPlay = true;
        LevelAccessed?.Invoke(); //evento iniciado e sua comunicação é feita no script EventManager
    }

    //executada sempre que a animação Level_Fade_Out termina
    void OnFadeOutComplete()
    {
        switch(typeFadeOut)
        {
            case 0:
                FadeFinished?.Invoke(); //evento iniciado e sua comunicação é feita no script EventManager
                break;
            case 1:
                SceneManager.LoadScene("MainMenu");
                GameFinished?.Invoke(); //evento iniciado e sua comunicação é feita no script EventManager
                break;
        }
    }

    void GameOver()
    {
        timerText.gameObject.SetActive(false);
        eventManagerRef.canPlay = false;
        messageTitle.text = "O tempo acabou";
        messageContent.text = "Todas as portas foram bloqueadas e você ficou preso no laboratório para sempre.";
        anim.Play("Message");
        PlayerPrefs.SetString("currentLevel", "semLevel");
    }

    void Win()
    {
        timerText.gameObject.SetActive(false);
        eventManagerRef.canPlay = false;
        messageTitle.text = "Você conseguiu";
        messageContent.text = "Você desbloqueou todas as portas, subindo pelos andares e agora está livre para avisar " +
                              "as autoridades sobre o perigo criado nas instalações.";
        anim.Play("Message");
        PlayerPrefs.SetString("currentLevel", "semLevel");
    }

    //função executada quando o botão de retornar ao Menu Principal é pressionado
    public void ToMainMenu()
    {
        anim.Play("Level_Fade_Out");
        typeFadeOut = typeMainMenu;
    }

    //função que se comunica com o script TerminalPuzzle através do script EventManager
    //o evento é chamado inicialmente na função Update() no script TerminalPuzzle
    public void OnTerminalAccessed()
    {
        anim.Play("Level_Fade_Out"); //no fim da animação, executa a função OnFadeOutComplete()
        typeFadeOut = typePuzzle;
        eventManagerRef.canPlay = false;
        PlayerPrefs.SetFloat("timer", currentTime);
    }
}