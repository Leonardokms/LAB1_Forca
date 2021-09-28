using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class DefinirVolume : MonoBehaviour
{
    public AudioMixer mixer;        // AudioMixer que controla as músicas da aplicação
    public Slider slider;           // Slider que controla o volume
    float volume;                   // Variável que armazena o volume em float

    void Start()
    {
        if (PlayerPrefs.HasKey("Volume"))               // Verifica se existe a PlayerPref "Volume"
        {           
            volume = PlayerPrefs.GetFloat("Volume");    // Define a variável "volume" como a PlayerPref "Volume" 
            slider.value = volume;                      // Define o valor do slider de acordo com a variável "volume"
        }
    }
    public void defineVolume(float valorVolume)
    {
        volume = slider.value;                                      // Define a variável "volume" de acordo com o valor do slider

        mixer.SetFloat("Volume", Mathf.Log10(valorVolume) * 20);    // Define o volume/valor do mixer de acordo com uma conversão do valor do slider,
                                                                    // de forma que o valor é convertido em log de base 10 e multiplicado por 20, fazendo com que 
                                                                    // a variação entre o menor e maior valor do slider sejam mais coerentes com sua posição atual

        PlayerPrefs.SetFloat("Volume", volume);                     // Define a PlayerPref "Volume" como a variável "volume"

    }
}