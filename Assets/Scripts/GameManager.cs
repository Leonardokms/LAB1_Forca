using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int numTentativas;              // Armazena as tentativas válidas da rodada
    private int maxNumTentativas;           // Número máximo de tentativas para Forca ou Salvação
    private int score;                      // Pontuação

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
        centro = GameObject.Find("centroDaTela");   // Encontra o GameObject "centroDaTela" e define-o para a variável "centro"
        InitGame();
        InitLetras();
        maxNumTentativas = 10;                      // Define o número máximo de tentativas como 10
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
            novaPosicao = new Vector3(centro.transform.position.x + ((i - numLetras / 2.0f) * 70), centro.transform.position.y, centro.transform.position.z); // Define a distância entre as letras para cada letra no eixo X
            GameObject l = (GameObject)Instantiate(letra, novaPosicao, Quaternion.identity);    // Define uma instancia do objeto l (letra) com os parâmetros letra (texto), posição e rotação (sem rotação)
            l.name = "letra" + (i + 1);                                     // Nomeia-se na hierarquia a GameObject com letra (iésima+1), i = 1...numLetras 
            l.transform.SetParent(GameObject.Find("Canvas").transform);     // Posiciona-se como filho do GameObject Canvas 
        }
    }
    void InitGame()
    {
        //palavraOculta = "Elefante";                            // Definição da palavra oculta a ser descoberta (Usado no LAB1 - parte A)
        //int numeroAleatorio = Random.Range(0, palavrasOcultas.Length);  //Sorteamos um número dentro do número de palavras da array (Usado no LAB1 - parte 2.A)
        //palavraOculta = palavrasOcultas[numeroAleatorio];       // Selecionamos uma palavra sorteada

        palavraOculta = PegaPalavraArquivo();                   // Pega palavra da função PegaPalavraArquivo
        tamanhoPalavraOculta = palavraOculta.Length;            // Determina-se o número de letras da palavra oculta
        palavraOculta = palavraOculta.ToUpper();                // Transforma a palavra em maiúscula
        letrasOcultas = new char[tamanhoPalavraOculta];         // Instanciamos o array char das letras da palavra
        letrasDescobertas = new bool[tamanhoPalavraOculta];     // Instanciamos o array bool do indicador de acertos
        letrasOcultas = palavraOculta.ToCharArray();            // Copia-se a palavra num array de letras
    }

    void checkTeclado()
    {
        if (Input.anyKeyDown)                                               // Lê qualquer tecla teclada
        {
            char letraTeclada = Input.inputString.ToCharArray()[0];         // Define uma variável que representa o valor inserido pelo teclado
            int letraTecladaComoInt = System.Convert.ToInt32(letraTeclada); // Checa consistência do valor de entrada, primeiramente definindo o valor entrado como um int32

            if (letraTecladaComoInt >= 97 && letraTecladaComoInt <= 122)    // Caso o valor entrado esteja dentro desse intervalo, ele é considerado válido
            {
                numTentativas++;                                            // Aumenta o contador de tentativas em 1
                UpdateNumTentativas();                                      // Chama a função UpdateNumTentativas
                if (numTentativas > maxNumTentativas)
                {
                    SceneManager.LoadScene("Lab1_Forca");                   // Carrega a tela de Forca (Game over) caso o número de tentativas seja maior que o número máximo de tentativas
                }
                for (int i = 0; i < tamanhoPalavraOculta; i++)
                {
                    if (!letrasDescobertas[i])                              // Verifica se uma letra foi acertada 
                    {
                        letraTeclada = System.Char.ToUpper(letraTeclada);   // Define as letras tecladas como maiúsculas
                        if (letrasOcultas[i] == letraTeclada)               // Caso a letra teclada seja igual a alguma letra das ocultas
                        { 
                            letrasDescobertas[i] = true;                    // Define a letra chutada como descoberta
                            GameObject.Find("letra" + (i + 1)).GetComponent<Text>().text = letraTeclada.ToString();  // Encontra e define o GameObject letra n-ésima como a letra acertada
                            score = PlayerPrefs.GetInt("score");            // Recupera o valor inteiro do score da PlayerPrefs e define para a variável score
                            score++;                                        // Aumenta o score em 1
                            PlayerPrefs.SetInt("score", score);             // Define o score da PlayerPrefs como a variável score
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
            condicao = condicao && letrasDescobertas[i];    // Define a condição "condicao" como a soma de todas as booleanas do array "letrasDescobertas"
        }
        if(condicao)                                        // Caso todas booleanas da array "letrasDescobertas" sejam verdadeiras, segue
        {
            PlayerPrefs.SetString("palavraOculta", palavraOculta);  // Define a PlayerPref palavraOculta como a variável palavraOculta
            SceneManager.LoadScene("Lab1_Salvo");                   // Abre a tela de Salvo (Vitória)
        }
    }

    string PegaPalavraArquivo()
    {
        TextAsset t1 = (TextAsset)Resources.Load("palavras", typeof(TextAsset));    // Carrega um asset de texto com base no asset "palavras"
        string s = t1.text;                                                         // Define uma variável "s" para armazenar o texto do asset "t1"
        string[] palavras = s.Split(' ');                                           // Separa o texto da string "s" por espaços em branco e coloca numa array "palavras"
        int palavraAleatoria = Random.Range(0, palavras.Length - 1);                // Define um inteiro "palavraAleatoria" como um valor aleatório entre 0 e o tamanho 
        return (palavras[palavraAleatoria]);                                        // Retorna a uma string da array "palavras" na posicao "palavraAleatoria"
    }

}
