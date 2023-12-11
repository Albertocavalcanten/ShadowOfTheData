//Script de funções dos Puzzles
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PuzzleController : MonoBehaviour
{
    public TypewriterEffect typewriterRef; //referência à classe TypeWriterEffect para uso do efeito de digitação
    [SerializeField] private TMP_Text rightAnswerText;
    [SerializeField] private TMP_Text wrongAnswerText;
    [SerializeField] private TMP_Text problem;
    [SerializeField] private TMP_Text question;
    [SerializeField] private Button buttonA;
    [SerializeField] private Button buttonB;
    [SerializeField] private Button buttonC;
    [SerializeField] private Button buttonD;
    [SerializeField] private AudioSource textSound;
    [SerializeField] private AudioSource endOfPuzzleSound;
    [SerializeField] private string nextScene;
    [SerializeField] private int tries;

    void Start()
    {
        //configurações de volume
        textSound.volume = PlayerPrefs.GetFloat("volume");
        endOfPuzzleSound.volume = PlayerPrefs.GetFloat("volume");

        StartCoroutine(StartPuzzle());
    }

    IEnumerator StartPuzzle()
    {
        //exibe os textos do problema e da pergunta
        yield return StartCoroutine(typewriterRef.ShowText(problem, textSound));
        yield return StartCoroutine(typewriterRef.ShowText(question, textSound));
        StartCoroutine(ShowAnswers()); 
    }

    IEnumerator ShowAnswers()
    {
        //exibe os botões das opções de resposta
        yield return new WaitForSeconds(0.1f);
        buttonA.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        buttonB.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        buttonC.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        buttonD.gameObject.SetActive(true);
    }

    public void RightAnswer()
    {
        if (wrongAnswerText.gameObject.activeSelf == true) //esconde a mensagem de resposta errada se ela estiver sendo exibida
        {
            wrongAnswerText.gameObject.SetActive(false);
        }
        StartCoroutine(Right());
    }

    public void WrongAnswer(string message)
    {
        if (rightAnswerText.gameObject.activeSelf == true) //esconde a mensagem de resposta certa se ela estiver sendo exibida
        {
            rightAnswerText.gameObject.SetActive(false);
        }
        wrongAnswerText.text = "Resposta errada.\n" + message; //mensagem com uma dica para auxiliar o jogador
        StartCoroutine(Wrong());
    }

    IEnumerator Right()
    {
        yield return StartCoroutine(typewriterRef.ShowText(rightAnswerText, textSound));
        endOfPuzzleSound.Play();
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(nextScene);
    }

    IEnumerator Wrong()
    {
        yield return StartCoroutine(typewriterRef.ShowText(wrongAnswerText, textSound));
        tries -= 1; //a cada resposta errada, o número de tentativas diminui
        yield return new WaitForSeconds(0.5f);
        if(tries == 0){
            PlayerPrefs.SetFloat("timer", 0); //caso o número de tentativas chegue a zero, o jogador perde o jogo e o tempo é zerado 
            wrongAnswerText.text = "Suas tentativas se esgotaram.\nZerando tempo restante...";
            yield return StartCoroutine(typewriterRef.ShowText(wrongAnswerText, textSound));
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(PlayerPrefs.GetString("currentLevel")); //carrega o ultimo level alcançado pelo jogador
        }
    }
}