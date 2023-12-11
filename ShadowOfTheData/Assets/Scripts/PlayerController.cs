//Script de funções do Jogador
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public EventManager eventManagerRef; //referência à classe EventManager para gerenciamento de eventos do jogo
    private CharacterController characterController;
    private AudioSource playerSound;
    private Animator anim;
    private const float speed = 5;
    private const float rotationSpeed = 65;
    private const float volumeControl = 0.2f;
    private const float playerFast = 1.8f;
    private const float playerSlow = 0.85f;
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerSound = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        //configuração de volume
        playerSound.volume = PlayerPrefs.GetFloat("volume") * volumeControl;
    }

    void Update()
    {
        if(eventManagerRef.canPlay) //o jogador só poderá se mover se o atributo canPlay for verdadeiro
        {
            Movement();
        } else {
            Stopped();
        }
    }

    void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = transform.forward * speed * vertical;
        Vector3 turn = transform.up * rotationSpeed * horizontal;

        characterController.Move(direction * Time.deltaTime);
        transform.Rotate(turn * Time.deltaTime);

        Animating(vertical, horizontal);
        Sounding(vertical, horizontal);
    }

    void Animating(float vertical, float horizontal)
    {
        bool foward = vertical > 0f;
        bool back = vertical < 0f;
        bool left = horizontal < 0f;
        bool right = horizontal > 0f;

        //configuração dos booleanos para execução ou não das animações
        anim.SetBool("Run", foward);
        anim.SetBool("BackRun", back);
        anim.SetBool("Left", left);
        anim.SetBool("Right", right);
    }

    void Sounding(float vertical, float horizontal)
    {
        //controle de som do personagem correndo(playerFast) ou rotacionando para os lados(playerSlow)
        if(vertical > 0f || vertical < 0f)
        {
            playerSound.pitch = playerFast;
        } else {
            playerSound.pitch = playerSlow;
        }

        //controle de som do personagem se movendo ou parado
        if(vertical != 0f || horizontal != 0f)
        {
            playerSound.enabled = true;
        } else {
            playerSound.enabled = false;
        }
    }

    void Stopped()
    {
        Animating(0, 0);
        Sounding(0, 0);
    }

    //função que se comunica com o script UIManager através do script EventManager
    //o evento é chamado inicialmente na função OnFadeInComplete() no script UIManager
    public void OnLevelAccessed()
    {
        eventManagerRef.canPlay = true;
    }
}