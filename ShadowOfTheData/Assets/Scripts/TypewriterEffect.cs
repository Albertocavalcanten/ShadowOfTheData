//Script de função do Efeito de Digitação
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    private const float textVelocity = 0.04f;
    private int currentVisibleCharacterIndex;

    public IEnumerator ShowText(TMP_Text textBox, AudioSource sound)
    {
        textBox.maxVisibleCharacters = 0;
        currentVisibleCharacterIndex = 0;
        textBox.gameObject.SetActive(true);
        sound.Play();
        while(currentVisibleCharacterIndex < textBox.textInfo.characterCount + 1)
        {
            textBox.maxVisibleCharacters++; //exibe, um a um, cada caractere presente no texto de textBox
            yield return new WaitForSeconds(textVelocity);
            currentVisibleCharacterIndex++;
        }
        sound.Stop();
        yield return null;
    }
}