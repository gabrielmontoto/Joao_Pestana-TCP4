using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHighScore : MonoBehaviour
{

    [SerializeField]
    private Text[] textoHighScore;
    Highscores highscores;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i<textoHighScore.Length;i++)
        {
            textoHighScore[i].text = i + 1 + ". Carregando...";
        }

        highscores = GetComponent<Highscores>();
        StartCoroutine("RecarregarHighScore");
    }
    public void QuandoForBaixadoOHighScore(Highscore[] highscoreList)
    {
        for (int i = 0; i < textoHighScore.Length; i++)
        {
            textoHighScore[i].text = i + 1 + ". ";
            if(highscoreList.Length>i)
            {
                textoHighScore[i].text += highscoreList[i].nome + " - " + highscoreList[i].pontos;
            }
        }
    }

    IEnumerator RecarregarHighScore()
    {
        while(true)
        {
            highscores.RetornaQuadroVencedores();
            yield return new WaitForSeconds(30);
        }
    }

}
