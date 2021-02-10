using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{

    public Slider slider; //slider


    public void ColocarAVidaMaxima(float vidaMaxima) //colocar o valor maximo do slider
    {
        slider.maxValue = vidaMaxima;
    }
    public void AtualizarVidaSlider(float vida) // atualizar o valor atual do slider
    {
        slider.value = vida;
    }
}
