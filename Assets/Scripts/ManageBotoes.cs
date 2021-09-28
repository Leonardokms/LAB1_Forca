using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageBotoes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("score", 0); // Define a PlayerPref "score" como 0 sempre que iniciar a ManageBotoes pela primeira vez
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMundoGame()
    {
        SceneManager.LoadScene("Lab1");             // Carrega a cena "Lab1"
    }

    public void StartCreditos()
    {
        SceneManager.LoadScene("Lab1_Creditos");    // Carrega a cena "Lab1_Creditos"
    }

    public void StartInicio()
    {
        SceneManager.LoadScene("Lab1_Start");       // Carrega a cena "Lab1_Start"
    }   
}
