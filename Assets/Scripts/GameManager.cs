using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int numTentativas;              // Armazena as tentativas v�lidas da rodada
    private int maxNumTentativas;           // N�mero m�ximo de tentativas para Forca ou Salva��o
    private int score;                      // Pontua��o

    public GameObject letra;                // Prefab da letra no jogo
    public GameObject centro;               // Objeto de texto que indica o centro da tela

    private string palavraOculta = "";      // Palavra a ser oculta
    //private string[] palavrasOcultas = new string[] { "carro", "elefante", "futebol" }; // Array de palavras ocultas (Usado no LAB1 - parte 2.A)
    private int tamanhoPalavraOculta;       // Tamanho desta palavra oculta
    char[] letrasOcultas;                   // Letras da palavra oculta
    bool[] letrasDescobertas;               // Indicador de quais letras foram descobertas

    // Start is called before the first frame update
    void Start()
    {
        centro = GameObject.Find("centroDaTela");   // Encontra o GameObject "centroDaTela" e define-o para a vari�vel "centro"
        InitGame();
        InitLetras();
        maxNumTentativas = 10;                      // Define o n�mero m�ximo de tentativas como 10
        UpdateNumTentativas();
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        checkTeclado();
    }

    void InitLetras()
    {
        int numLetras = tamanhoPalavraOculta;
        for (int i = 0; i < numLetras; i++)
        {
            Vector3 novaPosicao;
            novaPosicao = new Vector3(centro.transform.position.x + ((i - numLetras / 2.0f) * 70), centro.transform.position.y, centro.transform.position.z); // Define a dist�ncia entre as letras para cada letra no eixo X
            GameObject l = (GameObject)Instantiate(letra, novaPosicao, Quaternion.identity);    // Define uma instancia do objeto l (letra) com os par�metros letra (texto), posi��o e rota��o (sem rota��o)
            l.name = "letra" + (i + 1);                                     // Nomeia-se na hierarquia a GameObject com letra (i�sima+1), i = 1...numLetras 
            l.transform.SetParent(GameObject.Find("Canvas").transform);     // Posiciona-se como filho do GameObject Canvas 
        }
    }
    void InitGame()
    {
        //palavraOculta = "Elefante";                            // Defini��o da palavra oculta a ser descoberta (Usado no LAB1 - parte A)
        //int numeroAleatorio = Random.Range(0, palavrasOcultas.Length);  //Sorteamos um n�mero dentro do n�mero de palavras da array (Usado no LAB1 - parte 2.A)
        //palavraOculta = palavrasOcultas[numeroAleatorio];       // Selecionamos uma palavra sorteada

        palavraOculta = PegaPalavraArquivo();                   // Pega palavra da fun��o PegaPalavraArquivo
        tamanhoPalavraOculta = palavraOculta.Length;            // Determina-se o n�mero de letras da palavra oculta
        palavraOculta = palavraOculta.ToUpper();                // Transforma a palavra em mai�scula
        letrasOcultas = new char[tamanhoPalavraOculta];         // Instanciamos o array char das letras da palavra
        letrasDescobertas = new bool[tamanhoPalavraOculta];     // Instanciamos o array bool do indicador de acertos
        letrasOcultas = palavraOculta.ToCharArray();            // Copia-se a palavra num array de letras
    }

    void checkTeclado()
    {
        if (Input.anyKeyDown)                                               // L� qualquer tecla teclada
        {
            char letraTeclada = Input.inputString.ToCharArray()[0];         // Define uma vari�vel que representa o valor inserido pelo teclado
            int letraTecladaComoInt = System.Convert.ToInt32(letraTeclada); // Checa consist�ncia do valor de entrada, primeiramente definindo o valor entrado como um int32

            if (letraTecladaComoInt >= 97 && letraTecladaComoInt <= 122)    // Caso o valor entrado esteja dentro desse intervalo, ele � considerado v�lido
            {
                numTentativas++;                                            // Aumenta o contador de tentativas em 1
                UpdateNumTentativas();                                      // Chama a fun��o UpdateNumTentativas
                if (numTentativas > maxNumTentativas)
                {
                    SceneManager.LoadScene("Lab1_Forca");                   // Carrega a tela de Forca (Game over) caso o n�mero de tentativas seja maior que o n�mero m�ximo de tentativas
                }
                for (int i = 0; i < tamanhoPalavraOculta; i++)
                {
                    if (!letrasDescobertas[i])                              // Verifica se uma letra foi acertada 
                    {
                        letraTeclada = System.Char.ToUpper(letraTeclada);   // Define as letras tecladas como mai�sculas
                        if (letrasOcultas[i] == letraTeclada)               // Caso a letra teclada seja igual a alguma letra das ocultas
                        { 
                            letrasDescobertas[i] = true;                    // Define a letra chutada como descoberta
                            GameObject.Find("letra" + (i + 1)).GetComponent<Text>().text = letraTeclada.ToString();  // Encontra e define o GameObject letra n-�sima como a letra acertada
                            score = PlayerPrefs.GetInt("score");            // Recupera o valor inteiro do score da PlayerPrefs e define para a vari�vel score
                            score++;                                        // Aumenta o score em 1
                            PlayerPrefs.SetInt("score", score);             // Define o score da PlayerPrefs como a vari�vel score
                            UpdateScore();                          
                            VerificaPalavraDescoberta();
                        }   
                    }
                }
            }
        }
    }

    void UpdateNumTentativas()
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = "Tentativas: " + numTentativas + " / " + maxNumTentativas;  // Encontra o GameObject de contagem de tentativas e atualiza a contagem de tentativas 
    }

    void UpdateScore()
    {
        GameObject.Find("scoreUI").GetComponent<Text>().text = "Score: " + score;   // Encontra o GameObject de score e atualiza o score 
    }

    void VerificaPalavraDescoberta()
    {
        bool condicao = true;
        for(int i = 0; i < tamanhoPalavraOculta; i++)
        {
            condicao = condicao && letrasDescobertas[i];    // Define a condi��o "condicao" como a soma de todas as booleanas do array "letrasDescobertas"
        }
        if(condicao)                                        // Caso todas booleanas da array "letrasDescobertas" sejam verdadeiras, segue
        {
            PlayerPrefs.SetString("palavraOculta", palavraOculta);  // Define a PlayerPref palavraOculta como a vari�vel palavraOculta
            SceneManager.LoadScene("Lab1_Salvo");                   // Abre a tela de Salvo (Vit�ria)
        }
    }

    string PegaPalavraArquivo()
    {
        TextAsset t1 = (TextAsset)Resources.Load("palavras", typeof(TextAsset));    // Carrega um asset de texto com base no asset "palavras"
        string s = t1.text;                                                         // Define uma vari�vel "s" para armazenar o texto do asset "t1"
        string[] palavras = s.Split(' ');                                           // Separa o texto da string "s" por espa�os em branco e coloca numa array "palavras"
        int palavraAleatoria = Random.Range(0, palavras.Length - 1);                // Define um inteiro "palavraAleatoria" como um valor aleat�rio entre 0 e o tamanho 
        return (palavras[palavraAleatoria]);                                        // Retorna a uma string da array "palavras" na posicao "palavraAleatoria"
    }

}
