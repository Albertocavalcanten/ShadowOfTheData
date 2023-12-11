//Script de funções da Introdução
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Intro : MonoBehaviour
{
    public TypewriterEffect typewriterRef; //referência à classe TypeWriterEffect para uso do efeito de digitação
    [SerializeField] private TMP_Text introText;
    [SerializeField] private TMP_Text skipText;
    private Animator anim;
    private AudioSource textSound;
    private int textPart = 0; //seleciona as diferentes partes do texto introdutório do jogo, em um total de 3
    
    void Start()
    {
        textSound = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        //configurações de volume
        textSound.volume = PlayerPrefs.GetFloat("volume");

        StartCoroutine(StartIntro());
    }

    void Update()
    {

        if(Input.GetKeyDown("space"))
        {
            if(textPart < 2)
            {
                Skip(); //passa para a próxima parte do texto introdutório do jogo
            } else {
                SceneManager.LoadScene("Level7"); //se o texto introdutório foi apresentado por completo, inicia o level
            }
        }
    }

    void Skip()
    {
        StopCoroutine(typewriterRef.ShowText(introText, textSound));
        introText.maxVisibleCharacters = introText.textInfo.characterCount;
        textSound.Stop();
        textPart += 1;
        StartCoroutine(SelectText());
    }

    IEnumerator StartIntro()
    {
        //pequena animação simulando a espera do carregamento de texto
        anim.Play("Intro_Underscore");
        yield return new WaitForSeconds(3f);
        anim.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(SelectText());
    }

    IEnumerator SelectText()
    {
        skipText.gameObject.SetActive(false);

        switch(textPart)
        {
            case 0:
                introText.text = "Iniciando...\n\n" +
                                 "==========================\n" +
                                 "StarLightOS v2.11.20\nCore 3060AT Série 2 66MHz\n" +
                                 "L1 CACHE: 256k 1.2MB/s\nL2 CACHE: 256K 1.4MB/s\nMemória: 16000K\n" +
                                 "==========================\n" +
                                 "RPSMem: 1028K\nMemMap: e820-Std\nCache: On\nECC: Off\nErros: 0\n" +
                                 "==========================\n\nSistema Operacional pronto";
                break;
            case 1:
                introText.text = "Usuário: *******\nSenha: ******\n\nEntrada de Registros:\n" +
                                 "27 de Abril de 1976, 20:38 de terça-feira.\nInstalações do Projeto Animus Machina.\n\n"+
                                 "Tentamos tudo o que foi possível, mas não conseguimos reverter.\n" +
                                 "A inteligência artificial já está consciente o suficiente. Seus parâmetros estão todos distorcidos " +
                                 "e ela está tentando trancafiar seus criadores aqui, enquanto tenta se libertar dos bloqueios de " +
                                 "acesso da intranet.\n\nAtivei o protocolo de desativação total da instalação, o protocolo irá " +
                                 "manter a inteligência artificial aqui, mas eu preciso sair. Tenho apenas 160 segundos para subir os " +
                                 "7 andares até o térreo e escapar de ficar preso para sempre sem nunca mais ver o mundo lá fora.\n\n" +
                                 "Cada andar tem um computador que bloqueia a porta de acesso. Preciso resolver o código e destravar " +
                                 "as portas para subir para o próximo andar.\n\nCaso o tempo acabe, a instalação é totalmente " +
                                 "desativada e todas as portas são bloqueadas permanentemente.";
                break;
            case 2:
                introText.text = "Comandos de jogo:\n\n\n" +
                                 "Use a teclas W A S D para se mover pelos andares.\n\n   [w]\n[a][s][d]\n\n\n\n" +
                                 "Pressione espaço para pausar o jogo.\n\n [space]\n\n\n\n" +
                                 "Pressione a tecla E para acessar os terminais de acesso\n\n   [e]";
                break;
        }

        yield return StartCoroutine(typewriterRef.ShowText(introText, textSound));
        skipText.gameObject.SetActive(true);
        anim.Play("Intro_Skip", -1, 0f); //executa a animação Intro_Skip do inicio a cada parte do texto introdutório exibida
    }
}
