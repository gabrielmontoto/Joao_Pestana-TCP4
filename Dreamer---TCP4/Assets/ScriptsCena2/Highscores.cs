using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Highscores : MonoBehaviour
{
    [SerializeField]
     string privateCode = "ApyZN1l0zUi_UHgymU_9JA2lXiEdsRwEy_cEc3_cnj9A";
    [SerializeField]
      string publicCode = "5ef62ae6377eda0b6c799e6a";
     const string webURL = "http://dreamlo.com/lb/";

    public Highscore[] highscoreList;
    UIHighScore uIHighScore;
    static Highscores instance;

    private void Awake()
    {
        instance = this;
        uIHighScore = GetComponent<UIHighScore>();
    }

    IEnumerator UploadNewHighscore(string username, int score)
     {

         UnityWebRequest www = new UnityWebRequest(webURL + privateCode + "/add/" + UnityWebRequest.EscapeURL(username) + "/" + score);// o www eh obsoleto, foi trocado pelo unityWebRequest
        // WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score); //por meio do WWW que se acessa pagina na internet
         // o resultado disso é ->  http://dreamlo.com/lb/ApyZN1l0zUi_UHgymU_9JA2lXiEdsRwEy_cEc3_cnj9A/add/(nomeDoUsuario)/Score
         yield return www.SendWebRequest(); //a utilizacao do ienumrator é pra esperar o tempo de retorno pra ser upload no site e pra poder checar isso


         if(string.IsNullOrEmpty(www.error)) //nao sendo nulo ou vazio ele diz que foi adicionado no servidor com sucesso
         {
             print("upload foi um sucesso");
            RetornaQuadroVencedores();
         }
         else
         {
             print("Error" + www.error);
         }
     }



    IEnumerator GetRequest(string url) //parece que pode rolar uma treta quando vincula isso ao celular
    {
        // Something not working? Try copying/pasting the url into your web browser and see if it works.
        // Debug.Log(url);

        using (UnityWebRequest www = UnityWebRequest.Get(url)) //necessario pra fazer o retorno, baixar do banco de dados (pagina na internet)
        {
            yield return www.SendWebRequest(); //aguardar resposta do site
            FormatarTexto(www.downloadHandler.text); //envia o download pra poder formatar o texto que vem tudo junto em uma linha so
            uIHighScore.QuandoForBaixadoOHighScore(highscoreList);
        }
    }


    public void RetornaQuadroVencedores() //metodos publicos para acessarem o conteudo do script
    {
       // highScores = "";
        StartCoroutine(GetRequest(webURL + publicCode + "/pipe"));
    }
    public static void AdicionaVencedor(string username,int score)
    {
        instance.StartCoroutine(instance.UploadNewHighscore(username, score));
    }




    private void FormatarTexto(string texto) //pra formatar o texto
    {
        print(texto);
        String[] entradas = texto.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        //separa o texto em longas strings onde tem o \n que é o pulo de linha
        highscoreList = new Highscore[entradas.Length];//diz quantas linhas tem no total

        for (int i = 0; i < entradas.Length; i++)
        {
            string[] infoDeEntradas = entradas[i].Split(new char[] { '|' }); //procura essa barra vertical pra fazer a separaçao

            string[] nomeUsuario = infoDeEntradas[0].Split(new char[] { '+' }); //a primeira parte da separaçao vai ser armazenada no nome de usuario
            String nomefinal = "";// = new string("a");

            for (int p = 0; p < nomeUsuario.Length; p++)
            {
                nomefinal += nomeUsuario[p] + " ";
            }


            int pontosUsuario = int.Parse(infoDeEntradas[1]); // a segunda parte como pontos
            highscoreList[i] = new Highscore(nomefinal, pontosUsuario); // envia esses valores pra lista de highscores
            print(highscoreList[i].nome + ": " + highscoreList[i].pontos); // pra poder ser printada e aparecer na tela nesse caso ou pra poder ser manipulada quando necessaro
        }
    }

}

public struct Highscore //nao conheço o funcionamento do struct ate o momento
{
    public string nome;
    public int pontos;

    public Highscore(string _nome,int _Pontos)
    {
        nome = _nome;
        pontos = _Pontos;
    }
}
