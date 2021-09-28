using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class DefinirVolume : MonoBehaviour
{
    public AudioMixer mixer;        // AudioMixer que controla as m�sicas da aplica��o
    public Slider slider;           // Slider que controla o volume
    float volume;                   // Vari�vel que armazena o volume em float

    void Start()
    {
        if (PlayerPrefs.HasKey("Volume"))               // Verifica se existe a PlayerPref "Volume"
        {           
            volume = PlayerPrefs.GetFloat("Volume");    // Define a vari�vel "volume" como a PlayerPref "Volume" 
            slider.value = volume;                      // Define o valor do slider de acordo com a vari�vel "volume"
        }
    }
    public void defineVolume(float valorVolume)
    {
        volume = slider.value;                                      // Define a vari�vel "volume" de acordo com o valor do slider

        mixer.SetFloat("Volume", Mathf.Log10(valorVolume) * 20);    // Define o volume/valor do mixer de acordo com uma convers�o do valor do slider,
                                                                    // de forma que o valor � convertido em log de base 10 e multiplicado por 20, fazendo com que 
                                                                    // a varia��o entre o menor e maior valor do slider sejam mais coerentes com sua posi��o atual

        PlayerPrefs.SetFloat("Volume", volume);                     // Define a PlayerPref "Volume" como a vari�vel "volume"

    }
}