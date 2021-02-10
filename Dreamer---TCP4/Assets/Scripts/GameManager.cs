using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  
    //public TextMeshPro pontos;
    public TextMeshProUGUI pontos,pontosFimdeJogo,pontosPause;
    public int pontosFeitos;

    [SerializeField]
    GameObject jogoRodando, PreencherSeuNome;

    public Text nome, placeholder;
    public InputField inputTexto;
   // public string nomeFinal;
   [SerializeField] Jogador jogador;

    bool jogadorMorto = true;

    public bool testeMode;
    public float testModeFator;

    [Header("Joystick ajuste")][Space]
    [SerializeField] GameObject joystickMover;
    [SerializeField] GameObject joysticMira;
    // Start is called before the first frame update

    [Header("Canvas")]
    [Space]
    [SerializeField] GameObject TelaInicial;
    [SerializeField] GameObject Pause, HighScore,volumeTela,instrucoesTela, botoesTelaInicial,TelasTelaInicial, botaoSair,BotaoReiniciar,volumePause;

    [Header("Som")]
    [Space]
    [SerializeField] Slider MusicaSlider;
    [SerializeField] Slider EfeitoSlider;

    [Header("colocacoes")]
    [Space]
    [SerializeField]GameObject colocacoes1;
    [SerializeField]GameObject colocacoes2;

    [SerializeField] LevelLoader leveLoader;
    [SerializeField] bool ScanControle;
    private void Awake()
    {
   
        if (SceneManager.GetActiveScene().name == "Jogo")
        {
            
            tela();
        }
  

    }
    void Start()
    {
        if (SceneManager.GetSceneByName("Inicio") == SceneManager.GetSceneAt(0))
            SoundManager.Instance.PlayMusica("MusicaFundo");
        if (SceneManager.GetSceneByName("Jogo") == SceneManager.GetSceneAt(0))
        {
            
            SoundManager.Instance.PlayMusica("MusicaJogo");

            // TelaInicial.SetActive(true);
            jogoRodando.SetActive(true);
            Pause.SetActive(false);
            
           // volumeTela.SetActive(false);
            PreencherSeuNome.SetActive(false);
           // instrucoesTela.SetActive(false);
            HighScore.SetActive(false);
            Time.timeScale = 1;
            jogador = FindObjectOfType<Jogador>();
            pontos.text = "Pontos: 0000";


            // jogador.gameObject.SetActive(false);
        }
        else
        {
            HighScore.SetActive(false);
            SoundManager.Instance.PlayMusica("MusicaFundo");
        }
        leveLoader = FindObjectOfType<LevelLoader>();

    }
    
    // Update is called once per frame
    void Update()
    {
        if(ScanControle)
        {
            AstarPath.active.Scan();
            ScanControle = false;
        }
        if(SceneManager.GetActiveScene().name == "Jogo") // ou jogador.gameobject.activeself is true caso o objeto fique invisivel em vez de destruido
        {// o que pode ser mais interessante em vez de destruir deixar ele inativo pra guardar os valores de armas tiros e coisas

            if (jogador.gameObject.activeSelf == false)
            {
                if (jogadorMorto)
                {
                    GameOver();

                    jogadorMorto = false;
                }
            }
        }



    }

    public void AdicionaPontos(short pontosAdicionados)
    {
        pontosFeitos += pontosAdicionados;
        AtualizaTextoPontos();
        Debug.Log("teste");
        jogador.AdicionaPontosEspecial();
    }
    public void AtualizaTextoPontos()
    {
        pontos.text = "Pontos:" + pontosFeitos;
    }

    public void GameOver()
    {
        jogoRodando.SetActive(false);
        PreencherSeuNome.SetActive(true);
        string tempNome;
        
        tempNome = PlayerPrefs.GetString("nome");

        inputTexto.text = tempNome;
        
        //nome

        Time.timeScale = 0;
        pontosFimdeJogo.text = pontosFeitos.ToString();
    }

    public void BotaoEnviarClicado()
    {
        PlayerPrefs.SetString("nome", nome.text);
        PlayerPrefs.Save();
        PreencherSeuNome.SetActive(false);
        HighScore.SetActive(true);
      //  botaoSair.SetActive(true);
       // BotaoReiniciar.SetActive(true);
        Highscores.AdicionaVencedor(nome.text, pontosFeitos);


        //aqui
     /*   colocacoes1 = GameObject.Find("Colocacoes1");
        colocacoes2 = GameObject.Find("Colocacoes2");
        Text[] texto = new Text[10];
        UIHighScore uIHighScore = new UIHighScore();
        for (short i = 0; i < colocacoes1.transform.childCount; i++)
        {
            texto[i] = colocacoes1.transform.GetChild(i).GetComponent<Text>();
           
        }
        for (short i = 0; i < colocacoes2.transform.childCount; i++)
        {
            texto[i+colocacoes1.transform.childCount] = colocacoes1.transform.GetChild(i).GetComponent<Text>();

        }
        uIHighScore.ReceberUI(texto);*/
    }

    private void tela()
    {

        float width = Screen.width;
        float height = Screen.height;
        float temp = (width / height);



        joystickMover.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 3f / temp, Screen.height / 3f);
        joystickMover.GetComponentsInChildren<RectTransform>()[1].sizeDelta = new Vector2((Screen.width / 5) / temp, Screen.height / 5);
        joystickMover.GetComponent<RectTransform>().anchorMin = new Vector2(0.05f,0.05f);
        joystickMover.GetComponent<RectTransform>().anchorMax = new Vector2(0.05f, 0.05f);
        //  print(joystickMover.GetComponentsInChildren<RectTransform>()[1].name);
        joysticMira.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 3 / temp, Screen.height / 3);
        joysticMira.GetComponentsInChildren<RectTransform>()[1].sizeDelta = new Vector2((Screen.width / 5) / temp, Screen.height / 5);

     //   print(joystickMover.GetComponent<RectTransform>().sizeDelta + " ----------------------");

    }

    public void StartGame()
    {
        Time.timeScale = 1;
        leveLoader.LoadNexLevel();

        
        //SceneManager.LoadScene(1);

      /*  TelaInicial.SetActive(false);
        jogoRodando.SetActive(true);
        Pause.SetActive(false);
        volumeTela.SetActive(false);
        PreencherSeuNome.SetActive(false);
        instrucoesTela.SetActive(false);
        HighScore.SetActive(false);
        jogador.gameObject.SetActive(true);
        Time.timeScale = 1;*/
    }
    public void Instrucoes()
    {

        instrucoesTela.SetActive(!instrucoesTela.activeSelf);
        volumeTela.SetActive(false);
    }
    public void VolumeTela()
    {
        volumeTela.SetActive(!volumeTela.activeSelf);
        if (volumeTela.activeSelf == true)
        {
            MusicaSlider = GameObject.Find("SliderMusica").GetComponent<Slider>();
            EfeitoSlider = GameObject.Find("SliderEfeito").GetComponent<Slider>();
            MusicaSlider.value = SoundManager.Instance.RetornarValorMusica();
            EfeitoSlider.value = SoundManager.Instance.RetornarValorEfeito();

        }

        instrucoesTela.SetActive(false);
    }
    public void VolumePause()
    {

        volumePause.SetActive(!volumePause.activeSelf);
        if (volumePause.activeSelf == true)
        {
            MusicaSlider = GameObject.Find("SliderMusica").GetComponent<Slider>();
            EfeitoSlider = GameObject.Find("SliderEfeito").GetComponent<Slider>();
            MusicaSlider.value = SoundManager.Instance.RetornarValorMusica();
            EfeitoSlider.value = SoundManager.Instance.RetornarValorEfeito();

        }
    }
    public void HabilitarHighscore()
    {
        HighScore.SetActive(true);
       // jogoRodando.SetActive(false);
        // TelaInicial.SetActive(false);
        botoesTelaInicial.SetActive(false);
      //  TelasTelaInicial.SetActive(false);
        botaoSair.SetActive(false);
        BotaoReiniciar.SetActive(false);
    }
    public void Sair()
    {
        Application.Quit();
    }
    public void MenuInicial()
    {
        Time.timeScale = 1;
       // SceneManager.LoadScene(0);
        leveLoader.loadPreviusLevel();
      /*  TelaInicial.SetActive(true);
        botoesTelaInicial.SetActive(true);
        TelasTelaInicial.SetActive(true);


        jogoRodando.SetActive(false);
        Pause.SetActive(false);
        volumeTela.SetActive(false);
        PreencherSeuNome.SetActive(false);
        instrucoesTela.SetActive(false);
        HighScore.SetActive(false);*/

    }

    public void Reiniciar()
    {
        SceneManager.LoadScene(1);

    }

    public void PausarPartida()
    {
        Pause.SetActive(true);
        volumePause.SetActive(false);
        pontosPause.text = pontosFeitos.ToString();
        Time.timeScale = 0;
    }
    public void RetornarPartida()
    {
        Time.timeScale = 1;
        Pause.SetActive(false);
    }

    public void SliderEfeito(float valor)
    {
        SoundManager.Instance.AumentarVolumeEfeito(valor);
       // print(valor);
    }
    public void SliderMusica(float valor)
    {
        SoundManager.Instance.AumentarVolumeMusica(valor);
        // print(valor);
    }

    public void resetJogador()
    {
        jogoRodando.SetActive(true);
        Pause.SetActive(false);

        // volumeTela.SetActive(false);
        PreencherSeuNome.SetActive(false);
        // instrucoesTela.SetActive(false);
        HighScore.SetActive(false);
        Time.timeScale = 1;
        DestroyAll();
        jogadorMorto = true;
    }
    void DestroyAll()
    {
        Inimigo[] enemies = FindObjectsOfType<Inimigo>();// FindGameObjectsWithTag(tag);
        armaScript[] armas = FindObjectsOfType<armaScript>();
        PowerUpsScript[] power = FindObjectsOfType<PowerUpsScript>();
     //   print(enemies.Length + "--------------");
        for (int i = 0; i < enemies.Length; i++)
        {
            //enemies[i].ReceberDano(999);
            Destroy(enemies[i].GetComponentInChildren<AtaqueInimigo>().pai);
        }
        for (int i = 0; i < armas.Length; i++)
        {
            Destroy(armas[i].pai);
        }
        for (int i = 0; i < power.Length; i++)
        {
            Destroy(power[i].pai);
        }

    }
}
