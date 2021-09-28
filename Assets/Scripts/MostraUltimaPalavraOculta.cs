using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MostraUltimaPalavraOculta : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = "A palavra era: " + PlayerPrefs.GetString("palavraOculta"); // Mostra a última palavra oculta juntando um texto com a PlayerPref "palavraOculta"
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
