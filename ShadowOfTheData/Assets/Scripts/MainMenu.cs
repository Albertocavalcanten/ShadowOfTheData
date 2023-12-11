//Script de funções do Menu Principal
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueOption;
    [SerializeField] private AudioSource music;
    private AudioSource clickSound;
    private Animator anim;
    private string nextScene;
    private Slider volumeSlider;
    private bool playing = true;
    private const float volumeControl = 0.3f;
    private const float totalTime = 160;

    void Start()
    {
        anim = GetComponent<Animator>();
        clickSound = GetComponent<AudioSource>();
        volumeSlider = GetComponentInChildren<Slider>(true);

        //configurações iniciais de volume
        clickSound.volume = PlayerPrefs.GetFloat("volume") * volumeControl;
        music.volume = PlayerPrefs.GetFloat("volume");
        volumeSlider.value = PlayerPrefs.GetFloat("volume");

        music.Play();
        CheckContinueButton();
    }    

    //funções das opções de jogo
    void NewGame()
    {
        clickSound.Play();
        nextScene = "Intro";
        PlayerPrefs.SetFloat("timer", totalTime);
        anim.Play("All_Menu_Fade_Out"); //no fim da animação, executa a função OnFadeOutFinished()
    }

    void Continue()
    {
        clickSound.Play();
        nextScene = PlayerPrefs.GetString("currentLevel");
        PlayerPrefs.GetFloat("timer");
        anim.Play("All_Menu_Fade_Out"); //no fim da animação, executa a função OnFadeOutFinished()
    }

    //função executada sempre que a animação All_Menu_Fade_Out termina
    void OnFadeOutFinished()
    {
        if(playing)
        {
            SceneManager.LoadScene(nextScene);
        } else {
            Application.Quit();
        }
    }

    //função de exibição da opção de continuar a campanha anterior
    void CheckContinueButton()
    {
        if(PlayerPrefs.GetString("currentLevel") != "semLevel")
        {
            continueOption.gameObject.SetActive(true);
        } else {
            continueOption.gameObject.SetActive(false);
        }
    }

    void ShowOptions(string animation)
    {
        clickSound.Play();
        anim.Play(animation);
    }

    void Volume(float value)
    {
        PlayerPrefs.SetFloat("volume", value);
        clickSound.volume = value * volumeControl;
        music.volume = value;
        volumeSlider.value = value;
    }

    void Quit()
    {
        clickSound.Play();
        playing = false;
        anim.Play("All_Menu_Fade_Out"); //no fim da animação, executa a função OnFadeOutFinished()
    }
}