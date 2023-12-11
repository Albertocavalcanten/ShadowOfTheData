//Script de função do Efeito de Lâmpada Piscando
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLampLight : MonoBehaviour
{
    [SerializeField] private float minTime, maxTime; //tempos mínimo e máximo de duração do estado da lâmpada
    private float Timer;
    private Light lampLight;
    private Material lampMaterial;

    void Start()
    {
        lampLight = GetComponentInChildren<Light>();
        lampMaterial = GetComponent<Renderer>().material;
        lampMaterial.SetColor("_EmissionColor", Color.white);

        Timer = Random.Range(minTime, maxTime);
    }

    void Update()
    {
        FlickerLight();
    }

    void FlickerLight()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            lampLight.enabled = !lampLight.enabled; //troca de estado da lâmpada (ligada ou desligada)
            Timer = Random.Range(minTime, maxTime);
        }
    }
}